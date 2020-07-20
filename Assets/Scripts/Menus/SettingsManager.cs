using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke
/// Controls all UI for settings and passing commands to GameSaveUtility to store settings
/// </summary>
public class SettingsManager: MonoBehaviour
{
    [SerializeField] CanvasManager canvasManager;
    [SerializeField] Toggle vibration;
    [SerializeField] Toggle effects;
    [SerializeField] Toggle music;

    void Start()
    {
        // Initalizes setting UI
        int onOff = SettingSaveUtility.GetVibrationToggle();
        if (onOff == 1)
            vibration.isOn = true;
        else
            vibration.isOn = false;

        int volume = SettingSaveUtility.GetMusicVolume();
        if (volume >  0)
            music.isOn = true;
        else
             music.isOn = false;
        AudioUtility.MusicVolume = music.isOn;

        volume = SettingSaveUtility.GetEffectsVolume();
        if (volume > 0)
            effects.isOn = true;
        else
            effects.isOn = false;
        AudioUtility.EffectVolume = effects.isOn;
    }

    /// <summary>
    /// Opens website browser to neuroanatomy.ca content
    /// </summary>
    /// <param name="target"></param>
    public void OpenWebsite(string target)
    {
        // TODO: More thorough security for taking user out of application and into external website
        switch (target)
        {
            case "home":
                Application.OpenURL("http://neuroanatomy.ca/");
                break;
            case "syllabus":
                Application.OpenURL("http://neuroanatomy.ca/syllabusM412.html");
                break;
            default:
                Debug.Log("No URL was found.");
                break;
        }
    }

    /// <summary>
    /// Called from Vibration checkbox
    /// </summary>
    public void ToggleVibration()
    {
        SettingSaveUtility.ChangeVibration(vibration.isOn);
    }

    /// <summary>
    /// Called from Music checkbox - stores to both SettingSaveUtility and AudioUtility
    /// </summary>
    public void ToggleMusic()
    {
        AudioUtility.MusicVolume = music.isOn;
        SettingSaveUtility.ChangeMusic(music.isOn);
    }

    /// <summary>
    /// Called from Sound Effects checkbox - stores to both SettingSaveUtility and AudioUtility
    /// </summary>
    public void ToggleEffects()
    {
        AudioUtility.EffectVolume = effects.isOn;
        SettingSaveUtility.ChangeEffects(effects.isOn);
    }

    /// <summary>
    /// Called from Tutorial button - loads the tutorial scene
    /// </summary>
    public void StartTutorial()
    {
        LevelLoaderUtility.LoadTutorial();
    }

    /// <summary>
    /// GameSaveUtility Reset erases PlayerPrefs local storage and forces player to Signup screen
    /// </summary>
    public void Logout()
    {
        if (GameSaveUtility.OfflineMode == 0)
            DBManager.Instance.LogoutUser();
        GameSaveUtility.Reset();
        canvasManager.SwitchCanvas("signup");
    }
}
