using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  
 *  Now we have this script for taking stamina damage so whenever we change the mechnics of stamina
 *  damage we can just edit it here instead of changing in every single point where we are registering
 *  the stamina damage
 */

/*  
 *  Classes that are marked scriptable we can use "CreateAssetMenu" to give us the option in menu to create
 *  a scriptable object of this class so that it can be easily instantiated and are stored in the project
 *  as assets
 */

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]

public class TakeStaminaDamageEffect : InstantCharacterEffects
{
    [SerializeField] public float staminaDamage = 15f;

    public override void processEffect(CharacterManager character)
    {
        CalculateStaminaDamage(character);
    }

    private void CalculateStaminaDamage(CharacterManager character)
    {
        if (character.IsOwner)
        {
            character.characterNetworkManager.currentStamina.Value -= staminaDamage;
        }
    }
}
