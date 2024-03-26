using System;
using System.IO;
using UnityEngine;

public class SaveFileDataWriter
{
    //  THIS IS THE PATH OF DIRECTORY WHERE THE FILE SHOULD BE 
    [HideInInspector] public string saveDataDirectoryPath = "";

    //  THIS IS THE NAME OF THE FILE
    /*  We basically have slots for characters (10 in case of Elden Ring)
     *  so this name basically decides what slot we are saving the character
     *  data to */
    [HideInInspector] public string saveDataFileName = "";

    //  USED TO CHECK IF THE FILE ALREADY EXISTS
    public bool CheckIfFileExists()
    {
        if(File.Exists(Path.Combine(saveDataDirectoryPath, saveDataFileName)))
        {
            return true;
        }

        return false;
    }

    //  USED TO DELETE CHARACTER SAVE FILES
    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(Path.Combine(saveDataDirectoryPath, saveDataFileName)));
    }

    //  CREATES THE NEW FILE IF IT DOESNT ALREADY EXISTS
    public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
    {
        string savePath = Path.Combine(saveDataDirectoryPath, saveDataFileName);

        try
        {
            string directoryPath = Path.GetDirectoryName(savePath);

            //  CREATE THE DIRECTORY IF IT DOES NOT EXISTS
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            //  CONVERT DATA TO JSON
            string dataToStore = JsonUtility.ToJson(characterData);

            //  CREATING THE FILE AND WRITING DATA TO IT
            using(FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using(StreamWriter saveFileWriter = new StreamWriter(stream)) 
                {
                    saveFileWriter.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("ERROR SAVING THE NEW FILE");
        }
    }

    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;

        //  CREATING THE PATH 
        string loadPath = Path.Combine(saveDataDirectoryPath, saveDataFileName);

        //  CHECK IF FILE EXISTS
        if(File.Exists(loadPath))
        {
            string dataToRead = "";

            //  USING TRY CATCH SO THAT ISSUES WITH OPENING THE STREAM AND READING FROM FILE MAY BE CAUGHT
            try
            {
                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader saveFileReader = new StreamReader(stream))
                    {
                        dataToRead = saveFileReader.ReadToEnd();
                    }

                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToRead);
                }
            }
            catch (Exception e)
            {
                Debug.Log("ERROR LOADING THE SAVE FILE.");
            }
        }

        return characterData;
    }
}
