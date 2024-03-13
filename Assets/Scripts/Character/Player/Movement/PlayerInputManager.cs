using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    PlayerControls playerControls;

    [Header("Player Input Variables")]
    public float horizontalInput;
    public float verticalInput;
    public float moveAmount;

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
            playerControls.PlayerMovement.Movement.performed += assignMovementInputValue;
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

    private void assignMovementInputValue(InputAction.CallbackContext context)
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
}
