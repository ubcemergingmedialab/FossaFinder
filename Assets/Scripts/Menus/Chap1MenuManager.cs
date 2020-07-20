using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke
/// Controls all UI elements for chapter 1 menu navigation
/// </summary>
public class Chap1MenuManager : MonoBehaviour
{
    [SerializeField] GameObject diffSelect;
    [SerializeField] GameObject levelSelect;
    [SerializeField] GameObject levelStat;

    [SerializeField] GameObject basicScroll;
    [SerializeField] GameObject mediumScroll;
    [SerializeField] GameObject expertScroll;

    [SerializeField] Sprite filledStar;
    [SerializeField] Sprite emptyStar;

    private void OnEnable()
    {
        diffSelect.SetActive(true);
        levelSelect.SetActive(false);
        levelStat.SetActive(false);
    }

    /// <summary>
    /// Called by Back button from any level selection - enables difficulty selection content
    /// </summary>
    public void OpenDiffSelect()
    {
        diffSelect.SetActive(true);
        levelSelect.SetActive(false);
        levelStat.SetActive(false);
    }

    /// <summary>
    /// Called by the Basics button on the difficulty screen or Back button from the Level Stat screen - enables easy level selection content
    /// </summary>
    public void OpenBasics()
    {
        diffSelect.SetActive(false);
        levelSelect.SetActive(true);
        levelStat.SetActive(false);

        basicScroll.SetActive(true);
        mediumScroll.SetActive(false);
        expertScroll.SetActive(false);
    }

    /// <summary>
    /// Called by the Medium button on the difficulty screen or Back button from the Level Stat screen - enables easy level selection content
    /// </summary>
    public void OpenMedium()
    {
        diffSelect.SetActive(false);
        levelSelect.SetActive(true);
        levelStat.SetActive(false);

        basicScroll.SetActive(false);
        mediumScroll.SetActive(true);
        expertScroll.SetActive(false);
    }

    /// <summary>
    /// Called by the Expert button on the difficulty screen or Back button from the Level Stat screen - enables easy level selection content
    /// </summary>
    public void OpenExpert()
    {
        diffSelect.SetActive(false);
        levelSelect.SetActive(true);
        levelStat.SetActive(false);

        basicScroll.SetActive(false);
        mediumScroll.SetActive(false);
        expertScroll.SetActive(true);
    }

    /// <summary>
    /// Used for back button on the Level Stats screen
    /// </summary>
    public void OpenLevelSelect()
    {
        switch (ObjectiveUtility.CurrentDifficulty)
        {
            case ObjectiveUtility.Difficulty.Easy:
                OpenBasics();
                break;
            case ObjectiveUtility.Difficulty.Medium:
                OpenMedium();
                break;
            case ObjectiveUtility.Difficulty.Hard:
                OpenExpert();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Called once a level is selected from the scroll view - enables the level stat content
    /// </summary>
    public void OpenLevelStats()
    {
        diffSelect.SetActive(false);
        levelSelect.SetActive(false);
        levelStat.SetActive(true);

        SetLevelStats();
    }

    /// <summary>
    /// Sets the Level Stat UI for the selected level using the ObjectiveUtility to retrieve Current Objective
    /// and GameSaveUtility to retrieve the Score and Star for said objective
    /// </summary>
    private void SetLevelStats()
    {
        string levelName = ObjectiveUtility.CurrentObjective;
        int levelScore = GameSaveUtility.GetLevelScoreInt(levelName, 100);

        levelStat.transform.GetChild(0).GetComponent<Text>().text = levelName.Replace('_', ' ');
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
    }

    /// <summary>
    /// Called by either the Training or Challenge buttons
    /// </summary>
    public void PlaySelectedLevel()
    {
        LevelLoaderUtility.LoadLevel();
    }
}
