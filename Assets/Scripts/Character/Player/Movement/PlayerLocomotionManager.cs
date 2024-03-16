using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float rotationSpeed = 12f;

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
                //  PERFORM A ROLL

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

    private void HandleSprintAction()
    {
        if (PlayerInputManager.instance.sprintInput && !playerManager.isPerformingAction)
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
    }

    //  THIS FUNCTION FETCHES INPUT VALUES FROM PLAYER INPUT MANAGER
    private void GetInputValues()
    {
        verticalInput = PlayerInputManager.instance.verticalInput;
        horizontalInput = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }
}