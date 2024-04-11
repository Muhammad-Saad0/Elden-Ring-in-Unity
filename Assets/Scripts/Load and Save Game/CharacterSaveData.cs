using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  THIS SCRIPT SERVES AS THE TEMPLATE FOR CHARACTER DATA THAT SHOULD BE SAVED
[System.Serializable]
public class CharacterSaveData
{
    [Header("Character Name")]
    public string CharacterName;

    [Header("Time Played")]
    public float secondsPlayed;

    [Header("Character Position")]
    public float xPosition;
    public float yPosition;
    public float zPosition;

    [Header("Stats")]
    public int endurance;
    public int vitality;
}
