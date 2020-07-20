using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Written by Oliver Vennike Riisager- Edited by Kimberly Burke
/// Saves settings.
/// </summary>
public class SettingSaveUtility : MonoBehaviour
{
    public static string EffectsVolume { get { return "Effects"; } }
    public static string MusicVolume { get { return "Music"; } }
    public static string Vibration { get { return "Vibration"; } }

    public static UnityEvent volumeChanged = new UnityEvent();

    // Start is called before the first frame update - start the game with the set sound settings
    void Start()
    {
        // initialize settings from previous session
        if (GetEffectsVolume() > 0)
            ChangeEffectsVolumeWrapper(true);
        else
            ChangeEffectsVolumeWrapper(false);

        if (GetMusicVolume() > 0)
            ChangeMusicVolumeWrapper(true);
        else
            ChangeMusicVolumeWrapper(false);

    }
    
    /// <summary>
    /// Changes the saved vibration preference
    /// </summary>
    public static void ChangeVibration(bool enabled)
    {
        if (enabled)
            PlayerPrefs.SetInt(Vibration, 1);
        else
            PlayerPrefs.SetInt(Vibration, 0);

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Changes the saved vibration preference
    /// </summary>
    public static void ChangeEffects(bool enabled)
    {
        if (enabled)
            PlayerPrefs.SetInt(EffectsVolume, 1);
        else
            PlayerPrefs.SetInt(EffectsVolume, 0);
        volumeChanged.Invoke();
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Changes the saved vibration preference
    /// </summary>
    public static void ChangeMusic(bool enabled)
    {
        if (enabled)
            PlayerPrefs.SetInt(MusicVolume, 1);
        else
            PlayerPrefs.SetInt(MusicVolume, 0);
        volumeChanged.Invoke();
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Returns the saved Music volume
    /// if none has been saved, returns 1
    /// </summary>
    /// <returns></returns>
    public static int GetMusicVolume()
    {
        return PlayerPrefs.GetInt(MusicVolume, 1);
    }

    /// <summary>
    /// Returns the saved effects volume
    /// if none has been saved, returns 1
    /// </summary>
    /// <returns></returns>
    public static int GetEffectsVolume()
    {
        return PlayerPrefs.GetInt(EffectsVolume, 1);
    }

    /// <summary>
    /// Returns the saved effects volume
    /// if none has been saved, returns 1
    /// </summary>
    /// <returns></returns>
    public static int GetVibrationToggle()
    {
        return PlayerPrefs.GetInt(Vibration, 1);
    }


    /// <summary>
    /// Changes the saved music volume
    /// </summary>
    /// <param name="percent"></param>
    public void ChangeMusicVolumeWrapper(bool toggle)
    {
        ChangeMusic(toggle);
        volumeChanged.Invoke();
    }

    /// <summary>
    /// Changes the saved effects volume
    /// </summary>
    public void ChangeEffectsVolumeWrapper(bool toggle)
    {
        ChangeEffects(toggle);
        volumeChanged.Invoke();
    }

    /// <summary>
    /// Changes the saved effects volume
    /// </summary>
    public void ChangeVibrationWrapper(bool enabled)
    {
        ChangeVibration(enabled);
    }

    /// <summary>
    /// Returns the saved Music volume
    /// if none has been saved, returns 1
    /// </summary>
    /// <returns></returns>
    public float GetMusicVolumeWrapper()
    {
        return GetMusicVolume();
    }

    /// <summary>
    /// Returns the saved effects volume
    /// if none has been saved, returns 1
    /// </summary>
    /// <returns></returns>
    public float GetEffectsVolumeWrapper()
    {
        return GetEffectsVolume();
    }
}
