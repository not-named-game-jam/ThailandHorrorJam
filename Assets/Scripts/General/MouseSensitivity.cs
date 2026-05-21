using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivity : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI mousesensText;
    void Start()
    {
        if (slider != null)
        {
            sliderChanger(slider.value);
        }
    }
    
    public void sliderChanger(float value)
    {
        mousesensText.text = value.ToString("F1");
        KeepingSettingValues.savedcameraSpeed = value;
    }
}
