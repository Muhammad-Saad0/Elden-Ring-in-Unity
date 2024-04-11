using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  IT SETS UP ALL THE EFFECTS
//  LIKE ASSIGNING IDs TO EFFECTS ETC

public class WorldEffectsManager : MonoBehaviour
{
    public static WorldEffectsManager instance;
    public List<InstantCharacterEffects> instantEffects;

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

        GenerateEffectsIDs();
    }

    private void GenerateEffectsIDs()
    {
        for (int i = 0; i < instantEffects.Count; i++)
        {
            instantEffects[i].effectID = i; 
        }
    }
}
