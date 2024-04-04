using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenLoadMenuInputManager : MonoBehaviour
{
    private PlayerControls playerControls;

    /*  WE COULD ALSO USE AWAKE BUT SINCE THIS GAME OBJECT WILL NOT BE ENABLED ALL THE TIME
        WE CAN SAVE A BIT OF MEMORY */
    private void OnEnable()
    {
        //  SETTING UP PLAYER CONTROLS
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
        }

        playerControls.UI.Delete.performed += TitleScreenManager.instance.AttemptToDeleteCurrentSelectedCharacterSlot;
        playerControls.UI.Enable();
    }

    private void OnDisable()
    {
        playerControls.UI.Disable();
    }
}
