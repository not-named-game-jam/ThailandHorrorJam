using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsChanger : MonoBehaviour
{
    public enum SettingType
    {
        Music,
        SFX
    }

    [Header("Choose Type")]
    [SerializeField] SettingType settingType;

    [Header("Slider Things")]
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI textValue;

    void Start()
    {
        if (slider != null)
        {
            valueChanger(slider.value);
        }
    }

    public void valueChanger(float value)
    {
        textValue.text = (value*100).ToString("F1");

        switch (settingType)
        {
            case SettingType.SFX:
                SoundManager.instance.sfxVolume = value;
                break;
            case SettingType.Music:
                SoundManager.instance.musicVolume = value;
                break;

        }
        
    }

}

