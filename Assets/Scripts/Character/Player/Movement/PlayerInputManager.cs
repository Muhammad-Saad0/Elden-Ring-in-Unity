using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    PlayerControls playerControls;

    //THIS IS SET UP IN ONNETWORKSPAWN IN PLAYERMANAGER
    [HideInInspector] public PlayerManager playerManager;

    [Header("Player Movement Variables")]
    public float horizontalInput;
    public float verticalInput;
    public float moveAmount;

    [Header("Player Camera Variables")]
    public float horizontalCameraInput;
    public float verticalCameraInput;

    [Header("Player Action Variables")]
    public bool performDodge = false;
    public bool sprintInput = false;
    public bool jumpInput = false;

    private void Awake()
    {
        //MAKE THE CLASS SINGLETON
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        /*DONT DESTROY SHOULD COME BEFORE DISABLING THE SCRIPT 
         OTHERWISE IT WOULD NOT HAVE ANY */
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += HandleSceneChange;
        //WE DONT WANT IT ENABLED ON TITLE SCREEN
        instance.enabled = false;
    }

    private void Update()
    {
        HandleJumpInput();
    }

    private void HandleSceneChange(Scene oldScene, Scene newScene)
    {
        //ENABLE THE INPUTS ONLY ON WORLD SCENE
        if(newScene.buildIndex == SaveGameManager.instance.GetWorldSceneIndex())
        {
            instance.enabled = true;
        }
        else
        {
            instance.enabled = false;
        }
    }

    private void OnEnable()
    {
        //ASSIGN VALUE IF IT IS NULL
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += AssignMovementInputValue;
            playerControls.CameraMovement.Movement.performed += AssignCameraInputValue;
            playerControls.PlayerActions.Dodge.performed += AssignDodgeInputValue;

            //  SPRINTING INPUT
            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

            //  JUMP INPUT
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
        }

        //SUBSCRIBE TO THE ACTION EVENT
        playerControls.Enable();
    }

    /* THIS IS FOR TESTING
       enable the inputs only if the window is focused
       so that the inputs dont register on both windows
    */
    private void OnApplicationFocus(bool focus)
    {
        if(enabled)
        {
            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void AssignMovementInputValue(InputAction.CallbackContext context)
    {
        Vector2 playerMovementInput = context.ReadValue<Vector2>();
        horizontalInput = playerMovementInput.x;
        verticalInput = playerMovementInput.y;

        //  THIS IS REQUIRED TO DETERMINE HOW MUCH OF THE JOYSTICK IS MOVED
        //  CLAMP01 CLAMPS THE VALUE BETWEEN 0 AND 1
        moveAmount = Mathf.Clamp01(Math.Abs(horizontalInput) + Math.Abs(verticalInput));

        if(moveAmount > 0 && moveAmount <= 0.5f)
        {
            moveAmount = 0.5f;
        }
        else if(moveAmount > 0.5f)
        {
            moveAmount = 1f;
        }
    }

    private void AssignCameraInputValue(InputAction.CallbackContext context)
    {
        Vector2 playerCameraMovementInput = context.ReadValue<Vector2>();
        horizontalCameraInput = playerCameraMovementInput.x;
        verticalCameraInput = playerCameraMovementInput.y;
    }

    private void AssignDodgeInputValue(InputAction.CallbackContext context)
    {
        performDodge = true;
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;

            playerManager.playerLocomotionManager.HandleJumpAction();
        }
    }
}
