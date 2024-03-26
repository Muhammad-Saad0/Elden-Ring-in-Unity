using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;

    [SerializeField] private int staminaMultiplier = 10;

    //  DELAY BEFORE STAMINA STARTS REGENERATING
    [SerializeField] private float staminaRegenerationDelay = 2f;
    [SerializeField] private float staminaRegenerationInterval = 0.1f;
    [SerializeField] private float staminaRegenerationAmount = 5f;

    private float staminaRegenerationDelayTimer = 0f;
    private float staminaRegenerationIntervalTimer = 0f;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    //  THIS FUNCTION IS TO BE USED WHEN OUR PLAYER INITIALLY SPAWNS
    //  To calculate the maximum stamina based on endurance stat.
    public int CalculateMaxStaminaBasedOnEnduranceLevel(int endurance)
    {
        int maxStamina;

        maxStamina = endurance * staminaMultiplier;
        return maxStamina;
    }

    public void RegenerateStamina()
    {
        //  ONLY OWNER SHOULD REGENERATE STAMINA AND WE ARE CHECKING ISOWNER BEFORE CALLING THIS FUNCTION
        if (character.isPerformingAction)
            return;

        if (character.characterNetworkManager.sprintingValue.Value)
            return;

        //  DELAY TIMER GIVES DELAY BEFORE REGENERATING STAMINA
        if(staminaRegenerationDelayTimer >= staminaRegenerationDelay)
        {
            //  WE DONT WANT STAMINA TO BE INCREASED EVERY FRAME
            //  So to introduce an interval in regenration we use staminaRegenerationIntervalTimer
            if (staminaRegenerationIntervalTimer >= staminaRegenerationInterval)
            {
                staminaRegenerationIntervalTimer = 0f;

                //  ONLY INCREASE STAMINA IF STAMINA IS LESS THAN MAXIMUM STAMINA
                if(character.characterNetworkManager.currentStamina.Value 
                    < character.characterNetworkManager.maximumStamina.Value)
                character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
            }
            else
            {
                staminaRegenerationIntervalTimer += Time.deltaTime;
            }
        }
        else
        {
            staminaRegenerationDelayTimer += Time.deltaTime;
        }
    }

    public void ResetStaminaDelayTimer(float oldValue, float newValue)
    {
        //  IF THE STAMINA DECREASED THEN WE WILL RESET THE DELAY TIMER
        //  we want to give a delay when the stamina was decreased not when it was restoring
        if(oldValue > newValue)
        {
            staminaRegenerationDelayTimer = 0;
        }
    }
}
