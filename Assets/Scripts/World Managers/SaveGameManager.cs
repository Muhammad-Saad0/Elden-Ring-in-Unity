using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager instance;

    [Header("World Scene Index.")]
    [SerializeField] private int WorldSceneIndex = 1;

    private SaveFileDataWriter saveFileWriter;
    [HideInInspector] public PlayerManager player;

    [Header("Character Data")]
    [SerializeField] private CharacterSaveData currentCharacterData;
    private string fileName;

    [Header("Character Save Slots")]
    public CharacterSaveSlot currentCharacterSlot;
    public CharacterSaveData characterSaveSlot_01;
    public CharacterSaveData characterSaveSlot_02;
    public CharacterSaveData characterSaveSlot_03;
    public CharacterSaveData characterSaveSlot_04;
    public CharacterSaveData characterSaveSlot_05;

    //  THESE ARE FOR TESTING
    public bool saveGame = false;
    public bool loadGame = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadAllSavedFiles();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {

        //  THESE ARE FOR TESTING 
        if (saveGame)
        {
            saveGame = false;
            SaveGame();
        }

        if (loadGame)
        {
            loadGame = false;
            LoadGame();
        }
    }

    //  THIS FUNCTION FINDS THE FILE NAME BASED ON THE SELECTED CHARACTER SLOT
    public string DecideCharacterSaveFileNameBasedOnSelectedCharacterSlot(CharacterSaveSlot characterSaveSlot)
    {
        string saveFileName = "";

        switch(characterSaveSlot)
        {
            case CharacterSaveSlot.CharacterSaveSlot_01:
                saveFileName = "CharacterSaveSlot_01";
                break;
            case CharacterSaveSlot.CharacterSaveSlot_02:
                saveFileName = "CharacterSaveSlot_02";
                break;
            case CharacterSaveSlot.CharacterSaveSlot_03:
                saveFileName = "CharacterSaveSlot_03";
                break;
            case CharacterSaveSlot.CharacterSaveSlot_04:
                saveFileName = "CharacterSaveSlot_04";
                break;
            case CharacterSaveSlot.CharacterSaveSlot_05:
                saveFileName = "CharacterSaveSlot_05";
                break;
            default:
                break;
        }

        return saveFileName;
    }

    //  THIS FUNCTION CHECKS IF THERE IS A FREE SLOT AVAILABLE AND SETS UP THE VARIABLES FOR THE NEW GAME
    public void AttemptCreateNewGame()
    {
        saveFileWriter = new SaveFileDataWriter();
        saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

        //  CHECKING IF ANY OF THE SLOTS ARE EMPTY
        saveFileWriter.saveDataFileName = 
            DecideCharacterSaveFileNameBasedOnSelectedCharacterSlot(CharacterSaveSlot.CharacterSaveSlot_01);
        if (!saveFileWriter.CheckIfFileExists())
        {
            currentCharacterSlot = CharacterSaveSlot.CharacterSaveSlot_01;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        saveFileWriter.saveDataFileName =
            DecideCharacterSaveFileNameBasedOnSelectedCharacterSlot(CharacterSaveSlot.CharacterSaveSlot_02);
        if (!saveFileWriter.CheckIfFileExists())
        {
            currentCharacterSlot = CharacterSaveSlot.CharacterSaveSlot_02;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        saveFileWriter.saveDataFileName =
            DecideCharacterSaveFileNameBasedOnSelectedCharacterSlot(CharacterSaveSlot.CharacterSaveSlot_03);
        if (!saveFileWriter.CheckIfFileExists())
        {
            currentCharacterSlot = CharacterSaveSlot.CharacterSaveSlot_03;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        saveFileWriter.saveDataFileName =
            DecideCharacterSaveFileNameBasedOnSelectedCharacterSlot(CharacterSaveSlot.CharacterSaveSlot_04);
        if (!saveFileWriter.CheckIfFileExists())
        {
            currentCharacterSlot = CharacterSaveSlot.CharacterSaveSlot_04;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        saveFileWriter.saveDataFileName =
            DecideCharacterSaveFileNameBasedOnSelectedCharacterSlot(CharacterSaveSlot.CharacterSaveSlot_05);
        if (!saveFileWriter.CheckIfFileExists())
        {
            currentCharacterSlot = CharacterSaveSlot.CharacterSaveSlot_05;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }

        //  IF NONE OF THE SLOTS ARE EMPTY
        //  PROMPT USER THAT ALL SLOTS ARE TAKEN
        TitleScreenManager.instance.OpenNoCharacterSlotPopUp();
    }
    
    public void SaveGame()
    {
        //  FIND THE NAME OF THE FILE WE WILL SAVE TO
        fileName = DecideCharacterSaveFileNameBasedOnSelectedCharacterSlot(currentCharacterSlot);

        saveFileWriter = new SaveFileDataWriter();

        saveFileWriter.saveDataFileName = fileName;
        saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

        //  HERE WE WILL FIND THE CURRENT CHARACTER DATA AND THEN WE WILL SAVE IT.
        SaveCharacterDataToCurrentCharacterData();
        saveFileWriter.CreateNewCharacterSaveFile(currentCharacterData);
    }

    public void LoadGame()
    {
        //  FIND THE NAME OF FILE WE WANT TO LOAD FROM
        fileName = DecideCharacterSaveFileNameBasedOnSelectedCharacterSlot(currentCharacterSlot);

        saveFileWriter = new SaveFileDataWriter();

        saveFileWriter.saveDataFileName = fileName;
        saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

        currentCharacterData = saveFileWriter.LoadSaveFile();

        StartCoroutine(LoadWorldScene());
        LoadCharacterDataFromCurrentCharacterSaveData();
    }

    public void DeleteGame(CharacterSaveSlot characterSlot)
    {
        fileName = DecideCharacterSaveFileNameBasedOnSelectedCharacterSlot(characterSlot);

        saveFileWriter = new SaveFileDataWriter();

        saveFileWriter.saveDataFileName = fileName;
        saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileWriter.DeleteSaveFile();
    }

    //  THIS FUNCTION FILLS IN THE FIELDS OF currentCharacterSaveData
    private void SaveCharacterDataToCurrentCharacterData()
    {
        currentCharacterData.CharacterName = player.playerNetworkManager.characterName.Value.ToString();

        //  SET CHARACTER POSITION
        currentCharacterData.xPosition = player.transform.position.x;
        currentCharacterData.yPosition = player.transform.position.y;
        currentCharacterData.zPosition = player.transform.position.z;
    }

    //  THIS FUNCTION FILLS IN THE CHARACTER DATA FROM currentCharacterSaveData VARIABLE
    private void LoadCharacterDataFromCurrentCharacterSaveData()
    {
        player.playerNetworkManager.characterName.Value = currentCharacterData.CharacterName;

        //  GET THE PLAYER POSITION FROM currentCharacterData
        Vector3 playerPosition = new Vector3
            (currentCharacterData.xPosition,
            currentCharacterData.yPosition,
            currentCharacterData.zPosition);

        player.transform.position = playerPosition;
    }

    //  THIS FUNCTION GETS ALL SAVE FILES AND STORES IT IN THE VARIABLES
    //  This function will be called when the game start.
    public void LoadAllSavedFiles()
    {
        saveFileWriter = new SaveFileDataWriter();
        saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

        //  WE ARE READING EACH SAVE FILE THEN LOADING IT INTO ITS PARTICULAR SLOT

        saveFileWriter.saveDataFileName = DecideCharacterSaveFileNameBasedOnSelectedCharacterSlot
            (CharacterSaveSlot.CharacterSaveSlot_01);
        characterSaveSlot_01 = saveFileWriter.LoadSaveFile();

        saveFileWriter.saveDataFileName = DecideCharacterSaveFileNameBasedOnSelectedCharacterSlot
            (CharacterSaveSlot.CharacterSaveSlot_02);
        characterSaveSlot_02 = saveFileWriter.LoadSaveFile();

        saveFileWriter.saveDataFileName = DecideCharacterSaveFileNameBasedOnSelectedCharacterSlot
            (CharacterSaveSlot.CharacterSaveSlot_03);
        characterSaveSlot_03 = saveFileWriter.LoadSaveFile();

        saveFileWriter.saveDataFileName = DecideCharacterSaveFileNameBasedOnSelectedCharacterSlot
            (CharacterSaveSlot.CharacterSaveSlot_04);
        characterSaveSlot_04 = saveFileWriter.LoadSaveFile();

        saveFileWriter.saveDataFileName = DecideCharacterSaveFileNameBasedOnSelectedCharacterSlot
            (CharacterSaveSlot.CharacterSaveSlot_05);
        characterSaveSlot_05 = saveFileWriter.LoadSaveFile();
    }

    //  THIS FUNCTION IS SUPPOSED TO LOAD THE WORLD SCENE.
    public IEnumerator LoadWorldScene() {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(WorldSceneIndex);

        yield return null;
    }

    public int GetWorldSceneIndex()
    {
        return WorldSceneIndex;
    }
}
