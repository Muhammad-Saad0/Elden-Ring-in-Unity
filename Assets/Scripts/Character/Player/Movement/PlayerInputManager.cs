using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    PlayerControls playerControls;

    [Header("Player Input Variables")]
    [SerializeField] private Vector2 playerMovementInput;
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

    private void assignMovementInputValue(InputAction.CallbackContext context)
    {
        playerMovementInput = context.ReadValue<Vector2>();
    }
}
