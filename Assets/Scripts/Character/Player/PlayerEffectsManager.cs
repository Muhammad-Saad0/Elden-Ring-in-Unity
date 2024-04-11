using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    [SerializeField] private InstantCharacterEffects effectToTest;
    [SerializeField] private bool testEffect = false;

    private void Update()
    {
        if (testEffect)
        {
            testEffect = false;

            //  WE ARE CREATING A NEW INSTANCE SO THAT WE DO NOT CHANGE THE VALUES ON OUR SCRIPTABLE OBJECT (THAT IS GLOBAL)
            TakeStaminaDamageEffect effect = Instantiate(effectToTest) as TakeStaminaDamageEffect;
            effect.staminaDamage = 30f;
            ProcessInstantEffect(effect);
        }
    }
}
