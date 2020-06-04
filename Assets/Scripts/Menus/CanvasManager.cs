using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Kimberly Burke.
/// Switches active canvases for the start menus (including credits, player info input and cutscene)
/// </summary>
public class CanvasManager : MonoBehaviour
{
    [SerializeField] GameObject menus;
    [SerializeField] GameObject cutscene;
    [SerializeField] GameObject input;
    [SerializeField] GameObject credits;
    [SerializeField] GameObject leaderboard;
    [SerializeField] GameObject signup;
    [SerializeField] GameObject login;
    [SerializeField] GameObject privacy;


    // Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        if (GameSaveUtility.GetID() <= 0)
            SwitchCanvas("signup");
        else
            SwitchCanvas("menus");
    }

    /// <summary>
    /// Switches between canvases for the start menus
    /// </summary>
    /// <param name="target">The target canvas to activate</param>
    public void SwitchCanvas(string target)
    {
        menus.SetActive(false);
        cutscene.SetActive(false);
        input.SetActive(false);
        credits.SetActive(false);
        leaderboard.SetActive(false);
        signup.SetActive(false);
        login.SetActive(false);
        privacy.SetActive(false);

        switch(target)
        {
            case "menus":
                menus.SetActive(true);
                break;
            case "cutscene":
                cutscene.SetActive(true);
                break;
            case "input":
                input.SetActive(true);
                break;
            case "credits":
                credits.SetActive(true);
                break;
            case "leader":
                leaderboard.SetActive(true);
                break;
            case "login":
                login.SetActive(true);
                break;
            case "signup":
                signup.SetActive(true);
                break;
            case "privacy":
                privacy.SetActive(true);
                break;
            default:
                menus.SetActive(true);
                break;
        }
    }

    public void OpenPrivacy(string currentPage)
    {
        privacy.GetComponent<PrivacyManager>().prevPage = currentPage;
        privacy.GetComponent<PrivacyManager>().OpenPrivacy();
    }

    public void OpenTerms(string currentPage)
    {
        privacy.GetComponent<PrivacyManager>().prevPage = currentPage;
        privacy.GetComponent<PrivacyManager>().OpenService();

    }

    public void OpenStart()
    {
        menus.GetComponent<MenuManager>().OpenStart();
    }

    public void ExitApplication()
    {
        Application.Quit();
        PlayerPrefs.DeleteAll();
    }
}
