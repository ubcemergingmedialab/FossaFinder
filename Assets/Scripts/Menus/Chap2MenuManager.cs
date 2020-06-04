using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke
/// Controls all UI elements for chapter 2 menu navigation
/// </summary>
public class Chap2MenuManager : MonoBehaviour
{
    [SerializeField] GameObject levelSelect;
    [SerializeField] GameObject originSelect;
    [SerializeField] GameObject levelStat;

    [SerializeField] Sprite filledStar;
    [SerializeField] Sprite emptyStar;

    [SerializeField] GameObject loading;
    
    [Header("Origin Test")]
    [SerializeField] Text trackName;
    [SerializeField] Text tryAgain;
    [SerializeField] Image topButton;
    [SerializeField] Image bottomButton;
    [SerializeField] Sprite originButton;

    [Header("Loading Screen")]
    [SerializeField] Sprite[] originDiagrams;
    [SerializeField] Image loadingEntry;
    [SerializeField] Text loadingText;
 
    private void OnEnable()
    {
        levelSelect.SetActive(true);
        originSelect.SetActive(false);
        levelStat.SetActive(false);
    }

    /// <summary>
    /// Called by Back button from any Origin screen - enables tract level selection content
    /// </summary>
    public void OpenLevelSelect()
    {
        levelSelect.SetActive(true);
        originSelect.SetActive(false);
        levelStat.SetActive(false);
    }

    /// <summary>
    /// Called once a tract level is selected from the scroll view or Back button from the level stats screen - enables the origination quiz content
    /// </summary>
    public void OpenOriginSelect()
    {
        levelSelect.SetActive(false);
        originSelect.SetActive(true);
        levelStat.SetActive(false);

        if (tryAgain.gameObject.activeInHierarchy)
            tryAgain.gameObject.SetActive(false);
        trackName.text = ObjectiveUtility.CurrentObjective.Replace('_', ' ');
        topButton.sprite = originButton;
        bottomButton.sprite = originButton;
    }

    /// <summary>
    /// Called once the originization quiz is answered correctly - enables the level stats content
    /// </summary>
    public void OpenLevelStats()
    {
        levelSelect.SetActive(false);
        originSelect.SetActive(false);
        levelStat.SetActive(true);

        SetLevelStats();
    }

    /// <summary>
    /// Called by the Top or Bottom buttons - quick quiz for player depending on the tract level selected
    /// If correct, sets the origin diagram image for the loading screen and opens the level stats content
    /// </summary>
    /// <param name="origin"></param>
    public void OriginTest(string origin)
    {
        switch (ObjectiveUtility.CurrentObjective)
        {
            case "PCML_System_L":
                if (origin == "bottom")
                {
                    loadingEntry.sprite = originDiagrams[0];
                    loadingEntry.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 500);
                    ObjectiveUtility.IsMotor = false;
                    OpenLevelStats();
                } else
                {
                    tryAgain.gameObject.SetActive(true);
                }
                break;
            case "PCML_System_R":
                if (origin == "bottom")
                {
                    loadingEntry.sprite = originDiagrams[1];
                    loadingEntry.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 500);
                    ObjectiveUtility.IsMotor = false;
                    OpenLevelStats();
                }
                else
                {
                    tryAgain.gameObject.SetActive(true);
                }
                break;
            case "Spinothalamic_Tract_L":
                if (origin == "bottom")
                {
                    loadingEntry.sprite = originDiagrams[2];
                    loadingEntry.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 500);
                    ObjectiveUtility.IsMotor = false;
                    OpenLevelStats();
                }
                else
                {
                    tryAgain.gameObject.SetActive(true);
                }
                break;
            case "Spinothalamic_Tract_R":
                if (origin == "bottom")
                {
                    loadingEntry.sprite = originDiagrams[3];
                    loadingEntry.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 500);
                    ObjectiveUtility.IsMotor = false;
                    OpenLevelStats();
                }
                else
                {
                    tryAgain.gameObject.SetActive(true);
                }
                break;
            case "Lateral_Corticospinal_Tract_L":
                if (origin == "top")
                {
                    loadingEntry.sprite = originDiagrams[4];
                    loadingEntry.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 640);
                    ObjectiveUtility.IsMotor = true;
                    OpenLevelStats();
                }
                else
                {
                    tryAgain.gameObject.SetActive(true);
                }
                break;
            case "Lateral_Corticospinal_Tract_R":
                if (origin == "top")
                {
                    loadingEntry.sprite = originDiagrams[5];
                    loadingEntry.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 640);
                    ObjectiveUtility.IsMotor = true;
                    OpenLevelStats();
                }
                else
                {
                    tryAgain.gameObject.SetActive(true);
                }
                break;
            default:
                Debug.LogError("No tract selected");
                break;
        }
    }

    /// <summary>
    /// Sets the Level Stat UI for the selected level using the ObjectiveUtility to retrieve Current Objective
    /// and GameSaveUtility to retrieve the Score and Star for said objective (for both Challenge and Master mode)
    /// </summary>
    private void SetLevelStats()
    {
        string levelName = ObjectiveUtility.CurrentObjective;
        int levelScore = GameSaveUtility.GetLevelScoreInt(levelName, 100);
        int masterScore = GameSaveUtility.GetLevelScoreInt("Master" + levelName, 100);

        levelStat.transform.GetChild(0).GetComponent<Text>().text = levelName.Replace('_', ' ');

        // Update stats for challenge mode
        if (levelScore < 0)
        {
            levelStat.transform.GetChild(2).GetComponent<Text>().text = "New - 0 Score";
        }
        else
        {
            levelStat.transform.GetChild(2).GetComponent<Text>().text = levelScore.ToString();
        }
        bool star = GameSaveUtility.GetStarBoolean(levelName, "Perfect");
        if (star)
            levelStat.transform.GetChild(3).GetComponent<Image>().sprite = filledStar;
        else
            levelStat.transform.GetChild(3).GetComponent<Image>().sprite = emptyStar;

        // Update stats for master mode
        if (masterScore < 0)
        {
            levelStat.transform.GetChild(5).GetComponent<Text>().text = "New - 0 Score";
        } else
        {
            levelStat.transform.GetChild(5).GetComponent<Text>().text = masterScore.ToString();
        }
        star = GameSaveUtility.GetStarBoolean(levelName, "Perfect Master");
        if (star)
            levelStat.transform.GetChild(6).GetComponent<Image>().sprite = filledStar;
        else
            levelStat.transform.GetChild(6).GetComponent<Image>().sprite = emptyStar;
    }

    /// <summary>
    /// Called by either the Training, Challenge or Master buttons
    /// </summary>
    public void PlaySelectedLevel()
    {
        levelStat.SetActive(false);
        loading.SetActive(true);

        LevelLoaderUtility.LoadLevel(true); // Async
    }
}
