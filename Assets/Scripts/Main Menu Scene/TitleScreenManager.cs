using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager instance;

    [SerializeField] private GameObject titleScreenBackground;
    [SerializeField] private GameObject loadScreenBackground;

    [Header("Buttons")]
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button returnToMainMenuButton;
    [SerializeField] private Button noCharacterSlotButton;
    [SerializeField] private Button deleteCharacterConfirmButton;

    [Header("Pop Ups")]
    [SerializeField] private GameObject noCharacterSlotPopUp;
    [SerializeField] private GameObject deleteCharacterConfirmPopUp;

    [Header("Character Slots")]
    public CharacterSaveSlot currentSelectedCharacterSlot = CharacterSaveSlot.NO_SLOT;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ConnectToNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    //  THIS IS CALLED WHEN NEW GAME BUTTON IS PRESSED
    public void StartNewGame()
    {
        SaveGameManager.instance.AttemptCreateNewGame();
    }

    public void OpenLoadGameMenu()
    {
        titleScreenBackground.SetActive(false);
        loadScreenBackground.SetActive(true);

        returnToMainMenuButton.Select();
    }

    public void CloseLoadGameMenu()
    {
        titleScreenBackground.SetActive(true);
        loadScreenBackground.SetActive(false);

        loadGameButton.Select();
    }

    public void OpenNoCharacterSlotPopUp()
    {
        noCharacterSlotPopUp.SetActive(true);
        noCharacterSlotButton.Select();
    }

    public void CloseNoCharacterSlotPopUp()
    {
        noCharacterSlotPopUp.SetActive(false);
        newGameButton.Select();
    }

    public void SetCurrentSlotToNoSlot()
    {
        TitleScreenManager.instance.currentSelectedCharacterSlot = CharacterSaveSlot.NO_SLOT;
    }

    public void AttemptToDeleteCurrentSelectedCharacterSlot(InputAction.CallbackContext context)
    {
        if (currentSelectedCharacterSlot == CharacterSaveSlot.NO_SLOT)
            return;

        //  OPEN CONFIRMATION POPUP
        OpenDeleteCharacterConfirmPopUp();
    }

    public void DeleteCharacterSlot()
    {
        SaveGameManager.instance.DeleteGame(currentSelectedCharacterSlot);
        CloseDeleteCharacterConfirmPopUp();

        //  REFRESH THE LOAD MENU
        loadScreenBackground.SetActive(false);
        loadScreenBackground.SetActive(true);

        returnToMainMenuButton.Select();
    }

    private void OpenDeleteCharacterConfirmPopUp()
    {
        deleteCharacterConfirmPopUp.SetActive(true);
        deleteCharacterConfirmButton.Select();
    }

    public void CloseDeleteCharacterConfirmPopUp()
    {
        deleteCharacterConfirmPopUp.SetActive(false);
        returnToMainMenuButton.Select();
    }
}
