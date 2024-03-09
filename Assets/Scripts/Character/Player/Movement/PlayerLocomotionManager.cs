using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    private float verticalInput;
    private float horizontalInput;
    private float moveAmount;
    private Vector3 moveDirection;

    [SerializeField] private float walkingSpeed = 2f;
    [SerializeField] private float runningSpeed = 5f;

    private PlayerManager playerManager;

    override protected void Awake()
    {
        base.Awake();

        playerManager = GetComponent<PlayerManager>();
    }

    override protected void Update()
    {
        base.Update();
    }

    //  THIS FUNCTION GETS CALLED IN PLAYER MANAGER
    public void HandleAllMovement()
    {
        Debug.Log("Handle All movements");
        HandleGroundedMovement();
        //  HANDLE AERIEL MOVEMENT
    } 

    private void HandleGroundedMovement()
    {
        GetInputValues();

        //  MOVING THE CHARACTER BASED ON INPUT

        //  FINDING THE DIRECTION IN WHICH WE SHOULD MOVE
        //  WE WILL MOVE OUR CHARACTER WITH RESPECT TO THE CAMERA
        moveDirection = PlayerCamera.instance.transform.forward * verticalInput;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalInput;
        moveDirection.y = 0;

        moveDirection.Normalize();

        //  MOVING THE PLAYER USING CHARACTER CONTROLLER ON PLAYER
        if(moveAmount <= 0.5f)
        {
            playerManager.playerController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }
        else if(moveAmount > 0.5f)
        {
            playerManager.playerController.Move(moveDirection * runningSpeed * Time.deltaTime);
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
