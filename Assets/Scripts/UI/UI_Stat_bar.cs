using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stat_bar : MonoBehaviour
{
    Slider slider;

    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void SetStat(float newStatValue)
    {
        slider.value = newStatValue;
    }

    public void SetMaxStat(float maxStatValue)
    {
        slider.maxValue = maxStatValue;
    }
}
