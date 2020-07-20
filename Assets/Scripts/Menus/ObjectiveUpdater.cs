using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Writte by Oliver Vennike Riisager. Modified by Kimberly Burke
/// Makes sure that onclick updates values in ObjectiveUtility
/// Also makes sure that text on objects have been set correctly.
/// </summary>
public class ObjectiveUpdater : MonoBehaviour
{
    Button button;
    private string levelObjective;
    private Text[] children;
    // private MenusManager menuManager;
    private Chap1MenuManager chap1Manager;
    private Chap2MenuManager chap2Manager;
    private GameObject starImage;
    private GameObject masterStarImage;

    [SerializeField] Sprite filledStar;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        levelObjective = transform.GetComponent<Objective>().GetObjectiveName();
        button.onClick.AddListener(() => UpdateObjective(levelObjective));
        // menuManager = GameObject.FindGameObjectWithTag("StartMenu").GetComponent<MenusManager>();
        if (CompareTag("Chapter1"))
            chap1Manager = GameObject.FindGameObjectWithTag("Chapter1").GetComponent<Chap1MenuManager>();
        if (CompareTag("Chapter2"))
            chap2Manager = GameObject.Find("Chapter 2 Menu").GetComponent<Chap2MenuManager>();
        UpdateText();
    }

    private void OnEnable()
    {
        if (CompareTag("Chapter1"))
            chap1Manager = GameObject.FindGameObjectWithTag("Chapter1").GetComponent<Chap1MenuManager>();
        if (CompareTag("Chapter2"))
            chap2Manager = GameObject.Find("Chapter 2 Menu").GetComponent<Chap2MenuManager>();
    }

    /// <summary>
    /// Updates the currentObjective in the ObjectiveUtility
    /// </summary>
    /// <param name="tag"></param>
    private void UpdateObjective(string ObjectiveName)
    {
        ObjectiveUtility.SetObjective(ObjectiveName);
        switch (ObjectiveUtility.CurrentChapter)
        {
            case ObjectiveUtility.Chapter.None:
                Debug.Log("No chapter found.");
                break;
            case ObjectiveUtility.Chapter.Chapter1:
                chap1Manager.OpenLevelStats();
                break;
            case ObjectiveUtility.Chapter.Chapter2:
                chap2Manager.OpenOriginSelect();
                break;
            default:
                break;
        }
        // menuManager.UpdateSelectedLevel();

    }

    /// <summary>
    /// Updates the text on the GO's level and score text.
    /// </summary>
    private void UpdateText()
    {
        children = transform.GetComponentsInChildren<Text>();
        children[0].text = levelObjective.Replace('_', ' ');

        /* int currentScore = GameSaveUtility.GetLevelScoreInt(levelObjective, ObjectiveUtility.ScoreMultiplier);
        if (currentScore == -1)
            children[1].text = "New";
        else
            children[1].text = "" + currentScore;
        if (children.Length > 2)
        {
            int masterScore = GameSaveUtility.GetMasterLevelScoreInt(levelObjective, ObjectiveUtility.ScoreMultiplier * ObjectiveUtility.MasterModeMulti);
            if (masterScore == -1)
                children[2].text = "Master New";
            else
                children[2].text = "M " + masterScore;
        } */

        // set starImage visibility based on Objective star boolean
        starImage = transform.Find("Star").gameObject;
        if (GameSaveUtility.GetStarBoolean(levelObjective, "Perfect")) {
            starImage.GetComponent<Image>().sprite = filledStar;
        }

        // set star images for Chapter 2 master mode
        masterStarImage = transform.Find("Second Star").gameObject;
        if (GameSaveUtility.GetStarBoolean(levelObjective, "Perfect Master")) {
            masterStarImage.GetComponent<Image>().sprite = filledStar;
        }
    }
}
