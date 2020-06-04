using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke.
/// Controls for navigating the various start menus
/// </summary>

public class MenusManager : MonoBehaviour
{
    [SerializeField] CanvasManager canvasManager;

    [SerializeField] GameObject gameTitle;
    [SerializeField] GameObject tractLoading;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject startContent;
    [SerializeField] GameObject optionsContent;
    [SerializeField] GameObject chapterMenu;
    [SerializeField] GameObject indexMenu;
    [SerializeField] GameObject tractMenu;
    [SerializeField] GameObject loadScreen;
    [SerializeField] Button practiceButton;
    [SerializeField] Button playButton;
    [SerializeField] GameObject leaderboardButton;
    [Header("Player Info")]
    [SerializeField] GameObject playerPanel;
    [Header("Chapter 1")]
    [SerializeField]
    GameObject easyQuestsPanel;
    [SerializeField]
    GameObject mediumQuestsPanel;
    [SerializeField]
    GameObject hardQuestsPanel;
    [Header("Chapter 2")]
    [SerializeField]
    GameObject minimapPanel;
    [SerializeField]
    Sprite[]
    originBttnImages;
    [SerializeField] GameObject masterButton;
    [SerializeField]
    Button[]
    originButtons;
    [SerializeField]
    GameObject chapTwoInstructText;
    [SerializeField]
    GameObject chapTwoWrongText;
    [Header("Options")]
    [SerializeField]
    GameObject popupConfirm;

    string selectedLevel;

    void Start()
    {
        // Check if player is logged in
        if (GameSaveUtility.GetID() <= 0)
        {
            canvasManager.SwitchCanvas("signup");
        } 

        SwitchScreens(startMenu);
        SetPlayerInfo();
        switch (ObjectiveUtility.CurrentChapter)
        {
            case ObjectiveUtility.Chapter.None:
                OpenStart();
                break;
            case ObjectiveUtility.Chapter.Chapter1:
                OpenIndex();
                break;
            case ObjectiveUtility.Chapter.Chapter2:
                OpenTracts();
                break;
            default:
                Debug.Log("No chapter previously selected.");
                OpenStart();
                break;
        }
    }

    void Awake()
    {
        SetPlayerInfo();
    }

    private void OnEnable()
    {
        SetPlayerInfo();
    }

    /// <summary>
    /// Toggle active status of play buttons
    /// </summary>
    /// <param name="show">Active status of the play buttons</param>
    private void ShowChapterPlayButtons(bool show)
    {
        if (practiceButton != null)
            practiceButton.gameObject.SetActive(show);

        if (playButton != null)
            playButton.gameObject.SetActive(show);

        if (masterButton != null)
            masterButton.SetActive(show);
    }

    /// <summary>
    /// Toggles the interactive property of the play buttons
    /// </summary>
    /// <param name="interactable">Interactable property of the play buttons</param>
    private void MakeInteractableChapterPlayButtons(bool interactable)
    {
        if (practiceButton != null)
        {
            practiceButton.interactable = interactable;
        }

        if (playButton != null)
        {
            playButton.interactable = interactable;
        }
    }

    /// <summary>
    /// Switches the content on the start menu screen to the start content
    /// </summary>
    public void OpenOptions()
    {
        optionsContent.SetActive(true);
        startContent.SetActive(false);
    }

    /// <summary>
    /// Switches the content on the start menu screen to the options content
    /// </summary>
    public void CloseOptions()
    {
        optionsContent.SetActive(false);
        startContent.SetActive(true);
    }

    /// <summary>
    /// Opens the start menu as the current screen and hides the previous screen
    /// </summary>
    public void OpenStart()
    {
        SwitchScreens(startMenu);
        startContent.SetActive(true);
        gameTitle.SetActive(true);
        if (playerPanel.activeInHierarchy)
            playerPanel.SetActive(false);
    }

    /// <summary>
    /// Opens the chapter select menu as the current screen and hides the previous screen
    /// </summary>
    public void OpenChapters()
    {
        CheckFirstTime();
        SwitchScreens(chapterMenu);
        chapterMenu.SetActive(true);
        RemoveSelectedLevel();
        playerPanel.SetActive(true);
        leaderboardButton.SetActive(true);
    }

    /// <summary>
    /// Opens the index select menu for chapter one as the current screen and hides the previous screen
    /// </summary>
    public void OpenIndex()
    {
        SwitchScreens(indexMenu);
        ShowChapterPlayButtons(true);
        masterButton.SetActive(false);
        leaderboardButton.SetActive(true);
    }

    /// <summary>
    /// Opens the tract select menu for chapter two as the current screen and hides the previous screen
    /// </summary>
    public void OpenTracts()
    {
        SwitchScreens(tractMenu);
        ShowChapterPlayButtons(false);
        leaderboardButton.SetActive(true);
    }

    /// <summary>
    /// Switches between the quest panels for chapter 1 based on level difficulty selected
    /// </summary>
    /// <param name="levelDifficulty">The level difficulty selected</param>
    public void UpdateSelectedDifficulty()
    {
        switch (ObjectiveUtility.CurrentDifficulty)
        {
            case ObjectiveUtility.Difficulty.Easy:
                easyQuestsPanel.SetActive(true);
                mediumQuestsPanel.SetActive(false);
                hardQuestsPanel.SetActive(false);
                break;
            case ObjectiveUtility.Difficulty.Medium:
                easyQuestsPanel.SetActive(false);
                mediumQuestsPanel.SetActive(true);
                hardQuestsPanel.SetActive(false);
                break;
            case ObjectiveUtility.Difficulty.Hard:
                easyQuestsPanel.SetActive(false);
                mediumQuestsPanel.SetActive(false);
                hardQuestsPanel.SetActive(true);
                break;
            default:
                break;
        }
        RemoveSelectedLevel();
    }

    /// <summary>
    /// Depending on which chapter is selected, specific menus items will be enabled/disabled.
    /// And the Play and training buttons will activate.
    /// </summary>
    /// <param name="loadLevelInfo">contains level chapter, level name, locatin name, game mode, level difficulty and objective 
    /// name to pass onto the game scene when loading</param>
    public void UpdateSelectedLevel()
    {
        selectedLevel = ObjectiveUtility.CurrentObjective;


        MakeInteractableChapterPlayButtons(true);
        // if a level is seleted for chapter two, start the minimap origination sequence
        if (ObjectiveUtility.CurrentChapter == ObjectiveUtility.Chapter.Chapter2)
        {
            // reset status of tract screen so chapter buttons are hidden and top OR bottom origin must be selected
            minimapPanel.SetActive(true);
            ShowChapterPlayButtons(false);
            for (int i = 0; i < originButtons.Length; i++)
            {
                originButtons[i].GetComponent<Image>().sprite = originBttnImages[0]; // set previous button selected to inactive images
            }
            chapTwoInstructText.SetActive(true);
        }
        else
        {
            // if chapter two is not selected automatically see the play buttons
            ShowChapterPlayButtons(true);
            if (masterButton != null)
                masterButton.SetActive(false);
        }
    }

    /// <summary>
    /// Called by Tutorial button or first time play that loads the tutorial scene
    /// </summary>
    public void PlayTutorial()
    {
        LevelLoaderUtility.LoadTutorial();
    }

    /// <summary>
    /// Called by practice/"Play" button that starts the loading sequence for a training mode
    /// </summary>
    public void LoadSelectedLevelPractice()
    {
        StartSelectedLevel();
    }

    /// <summary>
    /// Called by the play/"Challenge" button that starts the loading sequence for a challenge mode
    /// </summary>
    public void LoadSelectedLevelRegular()
    {
        StartSelectedLevel();
    }

    /// <summary>
    /// Loading based on chapter cases. Passes information onto the LevelLoaderUtility. 
    /// </summary>
    /// <param name="gameMode">The game mode of either practice or challenge.</param>
    public void StartSelectedLevel()
    {
        SwitchScreens(loadScreen);

        switch (ObjectiveUtility.CurrentChapter)
        {
            case ObjectiveUtility.Chapter.Chapter1:
                if (tractLoading.activeInHierarchy)
                    tractLoading.SetActive(false);
                LevelLoaderUtility.LoadLevel();
                break;
            case ObjectiveUtility.Chapter.Chapter2:
                // Chapter 2 loads via async and contains a dynamic load screen
                tractLoading.SetActive(true);
                // tractLoading.GetComponent<ChapterTwoLoading>().SelectedTract(ObjectiveUtility.CurrentObjective);
                LevelLoaderUtility.LoadLevel(true);
                break;
        }

    }

    /// <summary>
    /// Sets so no quests are selected on the index screens and no level information is stored.
    /// </summary>
    private void RemoveSelectedLevel()
    {
        MakeInteractableChapterPlayButtons(false);
    }

    /// <summary>
    /// Takes player out of the game application and to the neuroanatyom.ca website in a separate browswer
    /// </summary>
    /// /// <param name="target">Specifies the target website page.</param>
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
    /// Plays a cutscene if its the first time playing.
    /// </summary>
    public void CheckFirstTime()
    {
        if (GameSaveUtility.GetID() <= 0 || GameSaveUtility.GetFirstTimePlayer())
        {
            PlayCutScene();
        }
    }

    /// <summary>
    /// Plays the cutscene
    /// </summary>
    private void PlayCutScene()
    {
        canvasManager.SwitchCanvas("cutscene");
    }

    /// <summary>
    /// Sets the player info based on the name and cerebucks total stored
    /// </summary>
    public void SetPlayerInfo()
    {
        Text name = playerPanel.transform.Find("Player Name").gameObject.GetComponent<Text>();
        name.text = GameSaveUtility.GetPlayerName();
        Text cerebucks = playerPanel.transform.Find("Player Cerebucks").gameObject.GetComponent<Text>();
        cerebucks.text = "$" + GameSaveUtility.GetTotalBalance();
        Text university = playerPanel.transform.Find("Player University").gameObject.GetComponent<Text>();
        university.text = GameSaveUtility.GetUniversity();
    }

    /// <summary>
    /// Logs out player by resetting the game save utility and switching canvas to signup
    /// </summary>
    public void ResetPlayerInfo()
    {
        GameSaveUtility.Reset();
        canvasManager.SwitchCanvas("signup");
    }

    /// <summary>
    /// Checks whether the correct orgination point is selected based on the selected tract for the level. If correct, will display the play buttons, otherwise will hide play buttons and change UI text.
    /// </summary>
    /// <param name="top">If top button is selected, then "top" is true.</param>
    public void CheckOrigination(bool top)
    {
        chapTwoInstructText.SetActive(false);
        string motorLeft = ObjectiveUtility.GetSpecificObjective("Lateral_Corticospinal_Tract_L");
        string motorRight = ObjectiveUtility.GetSpecificObjective("Lateral_Corticospinal_Tract_R");

        if (top && (ObjectiveUtility.CurrentObjective == motorLeft || ObjectiveUtility.CurrentObjective == motorRight))
        {
            ShowChapterPlayButtons(true);
            chapTwoInstructText.SetActive(false);
            chapTwoWrongText.SetActive(false);
            ObjectiveUtility.IsMotor = true;
            return;

        }
        else if (!top && !(ObjectiveUtility.CurrentObjective == motorLeft || ObjectiveUtility.CurrentObjective == motorRight))
        {
            ShowChapterPlayButtons(true);
            chapTwoInstructText.SetActive(false);
            chapTwoWrongText.SetActive(false);
            ObjectiveUtility.IsMotor = false;
            return;
        }
        else
        {
            ShowChapterPlayButtons(false);
            chapTwoInstructText.SetActive(true);
            chapTwoWrongText.SetActive(true);
            ObjectiveUtility.IsMotor = false;
            return;
        }
    }

    /// <summary>
    ///  Swaps the sprite for highlight the top or bottom origin buttons
    /// </summary>
    /// <param name="bttn">The button clicked</param>
    public void OriginButtonHighlight(Button bttn)
    {
        for (int i = 0; i < originButtons.Length; i++)
        {
            originButtons[i].GetComponent<Image>().sprite = originBttnImages[0]; // set previous button selected to inactive images
        }
        bttn.GetComponent<Image>().sprite = originBttnImages[1]; // set current button selected to active image
    }

    /// <summary>
    /// Hides all menu objects and activates the single designated active screen
    /// </summary>
    /// <param name="activeScreen">The screen to be set as active and visible</param>
    private void SwitchScreens(GameObject activeScreen)
    {
        gameTitle.SetActive(false);
        leaderboardButton.SetActive(false);

        startMenu.SetActive(false);
        startContent.SetActive(false);
        optionsContent.SetActive(false);

        chapterMenu.SetActive(false);
        indexMenu.SetActive(false);
        tractMenu.SetActive(false);
        loadScreen.SetActive(false);
        tractLoading.SetActive(false);

        easyQuestsPanel.SetActive(false);
        mediumQuestsPanel.SetActive(false);
        hardQuestsPanel.SetActive(false);

        minimapPanel.SetActive(false);
        chapTwoInstructText.SetActive(false);
        chapTwoWrongText.SetActive(false);

        ShowChapterPlayButtons(false);
        MakeInteractableChapterPlayButtons(false);

        // set the designated active screen to be active
        activeScreen.SetActive(true);
    }
}
