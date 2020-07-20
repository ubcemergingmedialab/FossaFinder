using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke
/// Controls larger UI elements for start screen (Chapter Select, Start, Settings, etc.) 
/// </summary>
public class MenuManager : MonoBehaviour
{

    [SerializeField] GameObject startContent;
    [SerializeField] GameObject optionContent;
    [SerializeField] GameObject chapSelect;
    [SerializeField] GameObject chap1Content;
    [SerializeField] GameObject chap2Content;
    [SerializeField] GameObject chap3Content;
    [SerializeField] GameObject playerPanel;

    [SerializeField] CanvasManager canvasManager;


    // Start is called before the first frame update
    void Start()
    {
        // Check if player is logged in
        Time.timeScale = 1;
        SetOnlineStatus();
        Debug.Log(GameSaveUtility.GetID());
        if (GameSaveUtility.GetID() <= 0 && GameSaveUtility.GetID() != -2)
        {
            canvasManager.SwitchCanvas("signup");
        }
        //SetPlayerInfo();
    }

    private void OnEnable()
    {
        SetPlayerInfo();
        switch (ObjectiveUtility.CurrentChapter)
        {
            case ObjectiveUtility.Chapter.None:
                OpenStart();
                playerPanel.SetActive(false);
                break;
            case ObjectiveUtility.Chapter.Chapter1:
                OpenChap1();
                playerPanel.SetActive(true);
                break;
            case ObjectiveUtility.Chapter.Chapter2:
                OpenChap2();
                playerPanel.SetActive(true);
                break;
            case ObjectiveUtility.Chapter.Chapter3:
                OpenChap3();
                playerPanel.SetActive(true);
                break;
            default:
                Debug.Log("No chapter previously selected.");
                OpenStart();
                break;
        }
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
        Text star = playerPanel.transform.Find("Player Stars").gameObject.GetComponent<Text>();
        star.text = GameSaveUtility.GetTotalStar().ToString();
    }

    public void SetOnlineStatus()
    {
        DBManager.Instance.ConnectionTest();
        Image online = playerPanel.transform.Find("Online").gameObject.GetComponent<Image>();
        if (GameSaveUtility.OfflineMode != 0)
            online.color = new Color(0, 0, 0);
        else
            online.color = new Color(1, 1, 1);
    }

    /// <summary>
    /// Called from Back button from Chapter Select or Settings Content - enables Start Content
    /// </summary>
    public void OpenStart()
    {
        DisableOthers();
        startContent.SetActive(true);
        if (playerPanel.activeInHierarchy)
            playerPanel.SetActive(false);
    }

    /// <summary>
    /// Called from Settings button on Start Content - enables Options Content
    /// </summary>
    public void OpenOptions()
    {
        DisableOthers();
        optionContent.SetActive(true);
    }

    /// <summary>
    /// Called from Start button on Start Content or Back button from any of the Chapter menus - enables Chapter Select menu
    /// </summary>
    public void OpenChapSelect()
    {
        DisableOthers();
        chapSelect.SetActive(true);
        playerPanel.SetActive(true);
    }

    /// <summary>
    /// Called from Chapter 1 button on Chapter Select menu or Back button from any of the Ch. 1 level selects - enables difficulty selection screen for chapter 1
    /// </summary>
    public void OpenChap1()
    {
        DisableOthers();
        chap1Content.SetActive(true);
    }

    /// <summary>
    /// Called form Chapter 2 button on Chapter Select menu or Back button from the Ch. 2 origin test screen - enables level selection screen for chatper 2
    /// </summary>
    public void OpenChap2()
    {
        DisableOthers();
        chap2Content.SetActive(true);
    }

    /// <summary>
    /// Called form Chapter 3 button on Chapter Select menu
    /// </summary>
    public void OpenChap3()
    {
        DisableOthers();
        chap3Content.SetActive(true);
    }

    public void LoadChap3()
    {
        LevelLoaderUtility.LoadLevel();
    }

    /// <summary>
    /// Toggles all UI elements off
    /// </summary>
    private void DisableOthers()
    {
        if (startContent.activeInHierarchy)
            startContent.SetActive(false);
        if (optionContent.activeInHierarchy)
            optionContent.SetActive(false);
        if (chapSelect.activeInHierarchy)
            chapSelect.SetActive(false);
        if (chap1Content.activeInHierarchy)
            chap1Content.SetActive(false);
        if (chap2Content.activeInHierarchy)
            chap2Content.SetActive(false);
        if (chap3Content.activeInHierarchy)
            chap3Content.SetActive(false);
        SetOnlineStatus();
    }
}
