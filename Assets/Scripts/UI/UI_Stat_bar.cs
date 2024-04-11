using UnityEngine;
using UnityEngine.UI;

public class UI_Stat_bar : MonoBehaviour
{
    [SerializeField] protected bool ScaleBarLengthWithStatValue = true;
    [SerializeField] protected float WidthScaleMultiplyer = 1f;

    Slider slider;
    RectTransform rectTransform;

    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetStat(float newStatValue)
    {
        slider.value = newStatValue;
    }

    public void SetMaxStat(float maxStatValue)
    {
        slider.maxValue = maxStatValue;

        if (ScaleBarLengthWithStatValue)
        {
            rectTransform.sizeDelta = new Vector2(maxStatValue * WidthScaleMultiplyer, rectTransform.sizeDelta.y);

            //  WE NEED TO REFRESH THE HUD BECAUSE THE UI IS NOT STICKING TO THE LEFT.
            PlayerUIManager.instance.hudManager.RefreshHUD();
        }
    }
}
