using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Created by Oliver Riisager. Edited by Kimberly Burke.
/// </summary>
public class SaveFirstTimePlayer : MonoBehaviour
{
    [SerializeField] InputField playerField;
    [SerializeField] Dropdown dropdownField;
    [SerializeField] GameObject backButton;

    [SerializeField] CanvasManager canvasManager;

    private void Start()
    {
        if (GameSaveUtility.GetID() > 0)
        {
            backButton.SetActive(false);
        }
    }

    private void OnEnable()
    {
        // default settings
        playerField.text = "Robert";
        dropdownField.value = 0; 
    }

    public void SaveNewPlayerName()
    {
        GameSaveUtility.SavePlayerName(playerField.text);
        GameSaveUtility.SaveUniversity(dropdownField.options[dropdownField.value].text);
        PlayerPrefs.Save();
    }

    public void SwitchCanvas()
    {
        canvasManager.SwitchCanvas("menus");
    }
}
