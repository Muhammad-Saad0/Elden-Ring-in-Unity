using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIHudManager : MonoBehaviour
{
    [SerializeField] private UI_Stat_bar staminaBar;
    [SerializeField] private UI_Stat_bar healthBar;

    public void RefreshHUD()
    {
        staminaBar.gameObject.SetActive(false);
        staminaBar.gameObject.SetActive(true);

        healthBar.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
    }

    public void SetNewStaminaValue(float oldStaminaValue, float newStaminaValue)
    { 
        staminaBar.SetStat(newStaminaValue);
    }

    public void SetMaxStaminaValue(float maxStaminaValue)
    {
        staminaBar.SetMaxStat(maxStaminaValue);
    }

    public void SetNewHealthValue(float oldHealthValue, float newHealthValue)
    {
        healthBar.SetStat(newHealthValue);
    }

    public void SetMaxHealthValue(float maxHealthValue)
    {
        healthBar.SetMaxStat(maxHealthValue);
    }
}
