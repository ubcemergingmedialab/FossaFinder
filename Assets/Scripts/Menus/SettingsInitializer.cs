using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsInitializer : MonoBehaviour
{
    void Start()
    {
        switch (tag)
        {
            case "Vibration":
                Toggle Vibration = GetComponent<Toggle>();
                int onOff = SettingSaveUtility.GetVibrationToggle();
                if (onOff == 1)
                    Vibration.isOn = true;
                else
                    Vibration.isOn = false;
                break;
            case "MasterVolume":
                // GetComponent<Slider>().value = SettingSaveUtility.GetMasterVolume();
                // AudioUtility.MasterVolume = GetComponent<Slider>().value;
                break;
            case "MusicVolume":
                GetComponent<Slider>().value = SettingSaveUtility.GetMusicVolume();
                // AudioUtility.MusicVolume = GetComponent<Slider>().value;
                break;
            case "EffectsVolume":
                GetComponent<Slider>().value = SettingSaveUtility.GetEffectsVolume();
                // AudioUtility.EffectVolume = GetComponent<Slider>().value;
                break;
        }
    }
}
