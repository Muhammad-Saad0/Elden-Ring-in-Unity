using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] private UI_Stat_bar staminaBar;

    public void setNewStaminaValue(float oldStaminaValue, float newStaminaValue)
    { 
        staminaBar.SetStat(newStaminaValue);
    }

    public void setMaxStaminaValue(float maxStaminaValue)
    {
        staminaBar.SetMaxStat(maxStaminaValue);
    }
}
