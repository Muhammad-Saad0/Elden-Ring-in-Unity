using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Character_Save_Slot : MonoBehaviour
{
    //  THIS VARIABLE IDENTIFIES WHICH SLOT IT IS
    [SerializeField] private CharacterSaveSlot slot;
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private TMP_Text timePlayed;

    private string fileName;
    private SaveFileDataWriter saveFileWriter;

    //  IT WILL NOT BE ENABLED BY DEFAULT WE WILL ENABLE IT WHEN WE OPEN LOAD MENU.
    //  So we can use onEnable.
    private void OnEnable()
    {
        fileName = SaveGameManager.instance.DecideCharacterSaveFileNameBasedOnSelectedCharacterSlot(slot);
        saveFileWriter = new SaveFileDataWriter();

        saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileWriter.saveDataFileName = fileName;

        if (saveFileWriter.CheckIfFileExists())
        {
            if (slot == CharacterSaveSlot.CharacterSaveSlot_01)
            {
                characterName.text = SaveGameManager.instance.characterSaveSlot_01.CharacterName;
            }
            if (slot == CharacterSaveSlot.CharacterSaveSlot_02)
            {
                characterName.text = SaveGameManager.instance.characterSaveSlot_02.CharacterName;
            }
            if (slot == CharacterSaveSlot.CharacterSaveSlot_03)
            {
                characterName.text = SaveGameManager.instance.characterSaveSlot_03.CharacterName;
            }
            if (slot == CharacterSaveSlot.CharacterSaveSlot_04)
            {
                characterName.text = SaveGameManager.instance.characterSaveSlot_04.CharacterName;
            }
            if (slot == CharacterSaveSlot.CharacterSaveSlot_05)
            {
                characterName.text = SaveGameManager.instance.characterSaveSlot_05.CharacterName;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    //  THIS FUNCTION IS CALLED
    public void HandleLoadGameOnSlotPressed()
    {
        //  SET THIS SLOT AS CURRENT SLOT
        //  While loading the game we are using current slot to load.
        SaveGameManager.instance.currentCharacterSlot = slot;

        SaveGameManager.instance.LoadGame();
    }

    //  THIS FUNCTION IS FIRED ON THE SLOT CHANGE EVENT
    //  Everytime we select new slot (select as in hover) this function is fired
    public void HandleSelectedSlotChange()
    {
        TitleScreenManager.instance.currentSelectedCharacterSlot = slot;
    }
}
