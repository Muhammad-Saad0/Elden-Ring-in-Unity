using UnityEngine;

//  THIS IS THE BASE CLASS FOR ALL THE INSTANT EFFECTS
/*  
 *  We are making this a scriptable object so that it acts like a gameObject 
 *  but is not part of the scene heirarchy we can instantiate it etc
 */

public class InstantCharacterEffects : ScriptableObject
{
    [Header("Effect ID")]
    public int effectID;

    //  THIS FUNCTION NEEDS TO BE OVERRIDEN BY EVERY EFFECT.
    public virtual void processEffect(CharacterManager character)
    {

    }
}