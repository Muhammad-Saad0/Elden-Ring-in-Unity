using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    CharacterManager character;

    private void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    public void ProcessInstantEffect(InstantCharacterEffects effect)
    {
        effect.processEffect(character);
    }
}
