using UnityEngine;
using Unity.Netcode;
using System;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    CharacterNetworkManager networkManager;

    private float verticalInput;
    private float horizontalInput;
    private float moveAmount;
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    private Vector3 rollDirection;

    [SerializeField] private float walkingSpeed = 2f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float rotationSpeed = 12f;

    //  SPRINTING VARIABLES
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float sprintingStaminaCost = 3f;
    [SerializeField] private float sprintingStaminaReductionTimer = 0f;
    [SerializeField] private float sprintingStaminaReductionInterval = 0.1f;

    //  JUMP VARIABLES
    [SerializeField] private float jumpStaminaCost = 15f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpForwardMovementSpeed = 4f;
    [SerializeField] private float freeFallMovementSpeed = 2f;

    private Vector3 jumpDirection;
    private Vector3 freeFallMoveDirection;

    private PlayerManager playerManager;

    override protected void Awake()
    {
        base.Awake();

        playerManager = GetComponent<PlayerManager>();
        networkManager = GetComponent<CharacterNetworkManager>();
    }

    override protected void Update()
    {
        base.Update();
    }

    //  THIS FUNCTION GETS CALLED IN PLAYER MANAGER
    public void HandleAllMovement()
    {
        if (playerManager == null)
            return;

        //  HANDLE MOVEMENT
        HandleGroundedMovement();
        HandleRotationMovement();
        HandleJumpMovement();
        HandleFreeFallMovement();

        //  HANDLE PLAYER ACTIONS
        HandlePlayerActions();
    }

    private void HandleGroundedMovement()
    {
        if (!playerManager.canMove)
            return;

        GetInputValues();

        //  MOVING THE CHARACTER BASED ON INPUT

        //  FINDING THE DIRECTION IN WHICH WE SHOULD MOVE
        //  WE WILL MOVE OUR CHARACTER WITH RESPECT TO THE CAMERA
        moveDirection = PlayerCamera.instance.transform.forward * verticalInput;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        //  MOVING THE PLAYER USING CHARACTER CONTROLLER ON PLAYER
        if (playerManager.playerNetworkManager.sprintingValue.Value)
        {
            playerManager.characterController.Move(moveDirection * sprintSpeed * Time.deltaTime);
        }
        else
        {
            if (moveAmount <= 0.5f)
            {
                playerManager.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }
            else if (moveAmount > 0.5f)
            {
                playerManager.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
        }
        
        

        //  ANIMATING THE PLAYER
        /*  WE ARE PASSING 0 FOR HORIZONTAL MOVEMENT BECAUSE WHEN WE ARE NOT LOCKED ON HORIZONTAL MOVEMENT IS 
        NOT REQUIRED */
        playerManager.playerAnimationController.UpdateCharacterAnimatorParameters(0f, moveAmount);
    }

    private void HandleRotationMovement()
    {
        if (!playerManager.canRotate)
            return;

        //  FIND DIRECTION TO ROTATE TOWARDS
        targetRotationDirection = Vector3.zero;
        targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalInput;
        targetRotationDirection = targetRotationDirection 
            + PlayerCamera.instance.cameraObject.transform.right * horizontalInput;
        targetRotationDirection.Normalize();
        targetRotationDirection.y = 0;

        /*  IF THERE WAS NO TARGET ROTATION DIRECTION (VERTICAL AND HORIZONTAL INPUTS WERE 0)
            set the direction as current look direction */
        if(targetRotationDirection == Vector3.zero)
        {
            targetRotationDirection = transform.forward;
        }

        //ROTATING TOWARDS THE NEW DIRECTION
        Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
        Quaternion lookRotation = Quaternion.Slerp
            (transform.rotation, 
            newRotation,
            rotationSpeed * Time.deltaTime);

        transform.rotation = lookRotation; 
    }

    //  THIS FUNCTION HANDLES WHICH DIRECTION PLAYER SHOULD BE MOVING IN WHEN IT IS IN MID JUMP
    private void HandleJumpMovement()
    {
        if (playerManager.isJumping)
        {
            playerManager.characterController.Move(jumpDirection * jumpForwardMovementSpeed * Time.deltaTime);
        }
    }

    //  THIS FUNCTION HANDLES MID AIR MOVEMENTS INPUTED BY THE USER.
    private void HandleFreeFallMovement()
    {
        if (!playerManager.isGrounded)
        {
            freeFallMoveDirection = Vector3.zero;
            freeFallMoveDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            freeFallMoveDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;

            freeFallMoveDirection.y = 0;
            freeFallMoveDirection.Normalize();
            
            playerManager.characterController.Move(freeFallMoveDirection * freeFallMovementSpeed * Time.deltaTime);
        }
    }

    //  PLAYER ACTIONS
    private void HandlePlayerActions()
    {
        HandleDodgeAction();
        HandleSprintAction();
    }

    private void HandleDodgeAction()
    {
        if (playerManager.isPerformingAction)
            return;

        if (PlayerInputManager.instance.performDodge)
        {
            PlayerInputManager.instance.performDodge = false;

            if (moveAmount > 0)
            {
                //  Rotate the player in direction where we want to roll
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRollRotation = Quaternion.LookRotation(rollDirection);
                transform.rotation = playerRollRotation;

                //  After Rotating play the roll animation
                playerManager.playerAnimationController.PlayTargetAnimation("Forward_Roll_01", true);
                networkManager.NotifyServerOfPlayerActionAnimationServerRpc
                    (NetworkManager.Singleton.LocalClientId,
                    "Forward_Roll_01",
                    true);
            }
            else
            {
                //  PERFORM A BACKSTEP
                playerManager.playerAnimationController.PlayTargetAnimation("Back_Step_01", true);
            }
        }
    }

    public void HandleJumpAction()
    {
        if (playerManager.isPerformingAction)
                return;

        if (playerManager.playerNetworkManager.currentStamina.Value < jumpStaminaCost)
                return;

        if (!playerManager.isGrounded)
                return;

        if (playerManager.isJumping)
                return;

        //  2ND PARAMETER IS APPLYING ROOT MOTION
        playerManager.playerAnimationController.PlayTargetAnimation("Main_Jump_Start_01", false);
        playerManager.isJumping = true;

        playerManager.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

        //  FINDING THE JUMP DIRECTION JUST WHEN WE JUMPED
        //  We will keep moving in this direction mid jump.
        if (playerManager.isJumping)
        {
            jumpDirection = Vector3.zero;

            jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
            jumpDirection.y = 0;

            if (jumpDirection != Vector3.zero)
            {
                if (playerManager.playerNetworkManager.sprintingValue.Value)
                {
                    //  APPLY THE VELOCITY TO FULLEST WHEN SPRINTING
                    jumpDirection *= 1;
                }
                //  PLAYER IS RUNNING (NOT SPRINTING).
                else if (moveAmount > 0.5)
                {
                    jumpDirection *= 0.5f;
                }
                //  PLAYER IS WALKING.
                else if (moveAmount <= 0.5)
                {
                    jumpDirection *= 0.25f;
                }
            }
        }
    }

    //  THIS FUNCTION WILL RUN ON ANIMATION EVENTS
    /*  
     *  Whenever we are at the point in animation where the player 
     *  is about to go up we want an upward velocity to lift
        our player upwards so we will add this to "ANIMATION EVENT" 

        @ ANIMATION EVENT:
        If we give the name of this function in animation event then every time 
        that animation is playing and the object has a script attached to it which has a 
        PUBLIC function name the same as we entered on the event then that function will run 
    */
    public void ApplyJumpVelocity()
    {
        //  WE ARE FINDING VELOCITY TO MOVE THE CHARACTER RESPECTIVE OF THE JUMP HEIGHT SET.
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
    }

    private void HandleSprintAction()
    {
        if (PlayerInputManager.instance.sprintInput 
            && !playerManager.isPerformingAction 
            && playerManager.playerNetworkManager.currentStamina.Value > 0)
        {
            if(moveAmount > 0.5f)
            {
                playerManager.playerNetworkManager.sprintingValue.Value = true;
            }
            else
            {
                playerManager.playerNetworkManager.sprintingValue.Value = false;
            }
        }
        else
        {
            playerManager.playerNetworkManager.sprintingValue.Value = false;
        }

        //  DECREASING THE STAMINA WHEN SPRINTING
        if (playerManager.playerNetworkManager.sprintingValue.Value)
        {
            if(sprintingStaminaReductionTimer >= sprintingStaminaReductionInterval)
            {
                sprintingStaminaReductionTimer = 0;
                playerManager.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost;
            }
            else
            {
                sprintingStaminaReductionTimer += Time.deltaTime;
            }
        }
    }

    //  THIS FUNCTION FETCHES INPUT VALUES FROM PLAYER INPUT MANAGER
    private void GetInputValues()
    {
        verticalInput = PlayerInputManager.instance.verticalInput;
        horizontalInput = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }
}