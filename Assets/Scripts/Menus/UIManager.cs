using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke. Edited by Niccolo Pucci.
/// Controls UI for game screens including buttons and feedback images (correct/wrong borders, score, etc.). 
/// Controls for chapter 1 and chapter 2 only.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject endGameScreen;
    [SerializeField] Text gameScoreText;
    [SerializeField] GameObject flaggingObjectivesPanel;
    [SerializeField] Text objectiveNameText;
    [SerializeField] Text scoreText;
    [SerializeField] RawImage miniMapImage;
    [SerializeField] Button skipSliceButton;
    [SerializeField] Image redBorder;
    [SerializeField] Image greenBorder;
    [SerializeField] Sprite filledStar;
    [SerializeField] Sprite emptyStar;

    [Header("Hint")]
    [SerializeField] Image hintModeIcon;
    [SerializeField] Sprite hintActiveIcon;
    [Header("Offline")]
    [SerializeField] GameObject offlinePopup;
    [SerializeField] Image startBG;
    [SerializeField] Button[] buttons;

    private GameObject[] hintTexts;

    PauseMenu pauseMenu;

    bool trainingMode = ObjectiveUtility.CurrentGameMode == ObjectiveUtility.GameMode.Training ? true : false;

    void Start()
    {
        pauseMenu = GetComponent<PauseMenu>();
        ShowActivityEndScreen(false, false);
        ShowActivityHUD(true);

        gameScoreText.gameObject.SetActive(true);
        if (ObjectiveUtility.CurrentGameMode == ObjectiveUtility.GameMode.Training)
        {
            gameScoreText.text = "TRAINING";
            gameScoreText.alignment = TextAnchor.MiddleCenter;
            gameScoreText.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            gameScoreText.text = "0";
            gameScoreText.alignment = TextAnchor.MiddleLeft;
            gameScoreText.transform.GetChild(0).gameObject.SetActive(true);
        }

        ShowGameObjectPanel(flaggingObjectivesPanel, true);

        hintTexts = GameObject.FindGameObjectsWithTag("Hint Text");
        StartSequence();

        // hide hint button when in training mode
        if (trainingMode)
            hintModeIcon.gameObject.SetActive(false);

        // alert player if playing offline
        if (GameSaveUtility.OfflineMode == 1)
        {
            startBG.raycastTarget = false;
            ToggleButtons(false);
            offlinePopup.SetActive(true);
        }
    }

    /// <summary>
    /// Called from Game Manager - hides all hint text and resumes timer
    /// </summary>
    public virtual void ShowActivityStartScreen()
    {
        //ShowActivityTimer ( true );
        for (int i = 0; i < hintTexts.Length; i++)
        {
            hintTexts[i].SetActive(false);
        }
        if (ObjectiveUtility.CurrentGameMode == ObjectiveUtility.GameMode.Training)
            return;
        else
            gameScoreText.text = "" + ObjectiveUtility.GetScore();
    }

    /// <summary>
    /// Called by end level trigger - sets the text for the current score and determines display of star image
    /// </summary>
    /// <param name="show"></param>
    /// <param name="perfectGame"></param>
    public virtual void ShowActivityEndScreen(bool show, bool perfectGame)
    {
        gameScoreText.gameObject.SetActive(false);
        if (ObjectiveUtility.CurrentGameMode == ObjectiveUtility.GameMode.Training)
        {
            endGameScreen.transform.Find("Score Text").gameObject.SetActive(false);
            endGameScreen.transform.Find("Cerebuck").gameObject.SetActive(false);
            endGameScreen.transform.Find("Star").gameObject.SetActive(false);
            endGameScreen.transform.Find("Challenge Button").gameObject.SetActive(true);
        }
        else
        {
            endGameScreen.transform.Find("Score Text").gameObject.SetActive(true);
            endGameScreen.transform.Find("Cerebuck").gameObject.SetActive(true);
            endGameScreen.transform.Find("Star").gameObject.SetActive(true);
            endGameScreen.transform.Find("Challenge Button").gameObject.SetActive(false);

            endGameScreen.transform.Find("Score Text").GetComponent<Text>().text = "" + ObjectiveUtility.GetScore();
            if (perfectGame)
            {
                endGameScreen.transform.Find("Star").GetComponent<Image>().sprite = filledStar;
            } else
            {
                endGameScreen.transform.Find("Star").GetComponent<Image>().sprite = emptyStar;
            }
        }
        ShowGameObjectPanel(endGameScreen, show);
    }

    public virtual void UpdateScoreUI()
    {
        if (ObjectiveUtility.CurrentGameMode == ObjectiveUtility.GameMode.Training)
            return;

        string scoreString = "" + ObjectiveUtility.GetScore();
        gameScoreText.text = scoreString;
    }

    public virtual void UpdateObjectiveName()
    {
        objectiveNameText.text = ObjectiveUtility.CurrentObjective.Replace("_", " ");
    }

    public virtual void ShowActivityHUD(bool show)
    {
        ShowGameObjectPanel(flaggingObjectivesPanel, show);
    }

    private void ShowGameObjectPanel(GameObject gameObjectPanel, bool show)
    {
        if (gameObjectPanel == null)
        {
            return;
        }

        gameObjectPanel.SetActive(show);
        //TODO - make particle Cerebucks thing
    }

    public static void EndLevel()
    {
        LevelLoaderUtility.GoToLevelSelect();
    }

    public virtual void UpdateMiniMapUI(Texture miniMapTexture)
    {
        if (miniMapImage == null)
        {
            return;
        }

        if (miniMapTexture == null)
        {
            return;
        }

        miniMapImage.texture = miniMapTexture;
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
    /// Pauses start gameplay and displays all hint text UI
    /// </summary>
    public virtual void StartSequence()
    {
        for (int i = 0; i < hintTexts.Length; i++)
        {
            hintTexts[i].SetActive(true);
        }
        ToggleButtons(false);
    }

    /// <summary>
    /// Called by Level Manager - when player activates Hint mode by tapping the map, switch the Hint Icon to active sprite
    /// </summary>
    public virtual void HintModeIcon()
    {
        hintModeIcon.sprite = hintActiveIcon;
    }

    public virtual void PauseGame()
    {
        pauseMenu.PauseLevel();
    }

    public virtual void ResumeGame()
    {
        pauseMenu.ResumeGame();
    }

    public void EndLevelWrapper()
    {
        EndLevel();
    }

    /// <summary>
    ///  Allows player to jump into the challenge mode directly from end screen.
    /// </summary>
    public void OpenChallengeMode()
    {
        ObjectiveUtility.SetGameMode(ObjectiveUtility.GameMode.Regular);
        GetComponent<PauseMenu>().RestartLevel();
    }

    public void RetryConnection()
    {
        startBG.raycastTarget = true;
        DBManager.Instance.ConnectionTest();
        if (GameSaveUtility.OfflineMode == 1)
        {
            offlinePopup.SetActive(true);
            startBG.raycastTarget = false;
        }
    }

    public void ConfirmOffline()
    {
        GameSaveUtility.SetConnectionMode(2);
        startBG.raycastTarget = true;
    }

    public void ToggleButtons(bool toggle)
    {
        foreach (Button bttn in buttons)
        {
            bttn.interactable = toggle;
        }
    }
}
