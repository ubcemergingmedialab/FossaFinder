using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke.
/// Controls all UI on tutorial scene (including text prompts and buttons activations)
/// </summary>
public class TutorialUI : MonoBehaviour
{
    [SerializeField] Button skipSliceButton;
    [SerializeField] Image redBorder;
    [SerializeField] Image greenBorder;
    private GameObject[] hintTexts;
    [SerializeField] Image hintModeIcon;
    [SerializeField] Sprite hintActiveIcon;
    [SerializeField] GameObject[] uiElements; // used for activating certain UI elements when called by events in TutorialEvents

    void Start()
    {

        hintTexts = GameObject.FindGameObjectsWithTag("Hint Text");

        // hide all UI at the start
        foreach (GameObject element in uiElements)
        {
            element.SetActive(false);
        }
    }

    public static void EndTutorial()
    {
        LevelLoaderUtility.GoToLevelSelect();
        GameSaveUtility.SetFirstTimePlayer();
    }

    public virtual void EnableSkipSliceButton(bool enable)
    {
        if (skipSliceButton == null)
        {
            return;
        }

        skipSliceButton.gameObject.SetActive(enable);
    }

    /// <summary>
    /// Called by Level Manager - when player misses the objective structure provide negative feedback (UI, add time, and vibration)
    /// </summary>
    public virtual void PlayNegativeFeedback()
    {
        if (!redBorder.gameObject.activeInHierarchy)
            redBorder.gameObject.SetActive(true);
        redBorder.GetComponent<Animator>().Play("redBorder");
        // TODO - create setting with Game Manager to determine player preferences for vibration
        Handheld.Vibrate();
    }

    /// <summary>
    /// Called by Level Manager - when player hits the objective strucutre provide positive feedback (sound, UI)
    /// </summary>
    public virtual void PlayPositiveFeedback()
    {
        if (!greenBorder.gameObject.activeInHierarchy)
            greenBorder.gameObject.SetActive(true);
        greenBorder.GetComponent<Animator>().Play("greenBorder");
    }

    /// <summary>
    /// Called by Level Manager - when player activates Hint mode by tapping the map, switch the Hint Icon to active sprite
    /// </summary>
    public virtual void HintModeIcon()
    {
        hintModeIcon.sprite = hintActiveIcon;
    }

    /// <summary>
    /// Called by Tutorial Events. uiElments = [joystick, boost button, skip button, minimap and hint]
    /// </summary>
    /// <param name="index">Indexed game object in the uiElements to activate</param>
    public void ActivateUI(int index)
    {
        uiElements[index].SetActive(true);
    }

    /// <summary>
    /// Called by skip tutorial button or by reaching the end of the tutorial
    /// </summary>
    public void EndTutorialWrapper()
    {
        EndTutorial();
    }
}
