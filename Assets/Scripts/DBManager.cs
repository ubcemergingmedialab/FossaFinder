using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.IO;
using System;

/// <summary>
/// Created by Cindy Shi and Wayland Bang. Modified by Kimberly Burke.
/// Connects Unity front end UI with PHP database back end.
/// </summary>
public class DBManager : MonoBehaviour
{

    const string MASTER_MODE = "Master";
    const string PERFECT_GAME = "Perfect";

    [SerializeField] Canvas loginCanvas;
    [SerializeField] Canvas signupCanvas;
    [SerializeField] GameObject loginPopup;
    [SerializeField] GameObject signupPopup;

    public static DBManager Instance;
    public static LeaderboardData lbData;
    public static MenuData menuData;
    private CanvasManager canvasManager;

    private bool status = false;
    private bool coroutineRunning = false;

    private string path = "https://projects.thecdm.ca/brainbros/";

    private void Awake()
    {
        if (canvasManager == null)
        {
            canvasManager = GameObject.FindGameObjectWithTag("CanvasManager").GetComponent<CanvasManager>();
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Debug.Log("Error: No DBManager found");
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        Instance.GetLeaderboard(); // get the most current leaderboard (used when the game first starts)

        ConnectionTest();
    }

    public void ConnectionTest()
    {
        StartCoroutine(TestConnection());
    }

    private void OnApplicationPause(bool pause)
    {
        if (!Application.isEditor)
        {
            StartCoroutine(StorePlayerDataRoutine(pause));
        }
    }

    private void OnApplicationQuit()
    {
        StartCoroutine(StorePlayerDataRoutine(false));
    }

    /// <summary>
    /// Clears all PlayerPrefs and when player is logged out - stores levels in database for the previously active user
    /// </summary>
    public void LogoutUser()
    {
        // Clear PlayerPrefs and store player money one more time to PlayerInfo database
        StartCoroutine(StorePlayerDataRoutine(true));
        canvasManager.SwitchCanvas("signup");
    }

    /// <summary>
    /// Creates new account entry in database and intializes all levels for 
    /// </summary>
    public void CreateNewUser()
    {
        StartCoroutine(CreateNewUserRoutine());
    }

    /// <summary>
    /// Retrieve levels from PlayerLevels and store to PlayerPref
    /// Database returns a JSON formatted as objective : { score, star, chapter, mode }
    /// Format in database
    /// Retrieve all player information(player name, player team, player first time, player money) from PlayerInfo and store to PlayerPrefs
    /// </summary>
    public void LoginUser()
    {
        StartCoroutine(LoginUserRoutione());
    }

    public void UpdatePlayerData()
    {
        StartCoroutine(UploadPlayerRoutine());
    }

    public void UpdateLevelData(bool star)
    {
        StartCoroutine(UpdateLevelDataRoutine(star));
    }

    public void GetLeaderboard()
    {
        StartCoroutine(GetLeaderboardRoutine());
    }
   
    public void UpdateLeaderboard()
    {
        StartCoroutine(UpdateLeaderboardRoutine());
    }

    /// <summary>
    /// Take the current chapter, current mode, current difficulty, objective name, objective score and objective stars 
    /// Store to the PlayerLevels
    /// </summary>
    /// <param name="objective"></param>
    /// <param name="score"></param>
    /// <param name="star"></param>
    public void UpdateLevelScore(string objective, int score, int star)
    {
        StartCoroutine(StoreLevelDataRoutine(objective, score, star));
    }

    public LeaderboardData GetLeaderboardData()
    {
        return lbData;
    }

    /// <summary>
    /// Checks if the login/sign up process has gone through successfully.
    /// If errors, does not open menus screen and creates a popup filled with information about the error
    /// </summary>
    /// <param name="type"></param>
    public void CheckStatus()
    {
        if (status)
        {
            status = false; // reset
            this.gameObject.GetComponent<CanvasManager>().SwitchCanvas("menus");
        } 
    }

    /// <summary>
    /// Using the Signup Canvas input fields to post account name, account password and email to PHP. Returns the account ID to store in GameSaveUtility
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateNewUserRoutine()
    {
        string[] dbText;

        WWWForm form = new WWWForm();
        form.AddField("account_name", signupCanvas.transform.GetChild(0).GetComponent<InputField>().text);
        form.AddField("account_pass", signupCanvas.transform.GetChild(1).GetComponent<InputField>().text);
        form.AddField("confirm_pass", signupCanvas.transform.GetChild(2).GetComponent<InputField>().text);
        form.AddField("email", signupCanvas.transform.GetChild(3).GetComponent<InputField>().text);

        using (UnityWebRequest www = UnityWebRequest.Post(path + "createaccount.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form load complete!");
                StringBuilder sb = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                {
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                }

                // Print Headers
#if (UNITY_EDITOR)
                Debug.Log(sb.ToString());
#endif
                if (www.GetResponseHeaders().Count > 0 && www.GetResponseHeaders().ContainsKey("UserCreation"))
                {
                    if (www.GetResponseHeaders()["UserCreation"] == "true")
                    {
                        Debug.Log("New account successful.");
                        dbText = www.downloadHandler.text.Split('\n');
                        status = true;
                        GameSaveUtility.SaveID(int.Parse(dbText[2])); // know that the ID is echoed on the 3rd line of the db text body
                        canvasManager.SwitchCanvas("input");
                    }
                    else
                    {
                        signupPopup.transform.GetChild(0).GetComponent<Text>().text = "ERROR: Create Account";
                        string bodyError = "Create account failed: ";
                        if (www.GetResponseHeaders().ContainsKey("PasswordMatch")) { bodyError += "Passwords do not match.\n"; }
                        if (www.GetResponseHeaders().ContainsKey("AccountFail")) { bodyError += "This account name has been taken. Please choose another.\n"; }
                        if (www.GetResponseHeaders().ContainsKey("EmailFail")) { bodyError += "There is already an account under this email.\n"; }
                        if (www.GetResponseHeaders().ContainsKey("EmailFormat")) { bodyError += "This is not a valid email.\n"; }
                        signupPopup.transform.GetChild(1).GetComponent<Text>().text = bodyError;
                        signupPopup.SetActive(true);
#if (UNITY_EDITOR)
                        Debug.Log(www.downloadHandler.text);
#endif
                    }
                }
                else
                {
                    // Failed
                    signupPopup.transform.GetChild(0).GetComponent<Text>().text = "ERROR: Create Account";
                    signupPopup.transform.GetChild(1).GetComponent<Text>().text = "Unable to connect to server.";
                    signupPopup.SetActive(true);
#if (UNITY_EDITOR)
                    Debug.Log(www.downloadHandler.text);
#endif
                }  
            }
        }
    }

    /// <summary>
    /// Using the Signup Canvas input fields to post account name, account password and email to PHP. Returns the account ID to store in GameSaveUtility
    /// </summary>
    /// <returns></returns>
    IEnumerator LoginUserRoutione()
    {
        WWWForm form = new WWWForm();
        form.AddField("account_name", loginCanvas.transform.GetChild(0).GetComponent<InputField>().text);
        form.AddField("account_pass", loginCanvas.transform.GetChild(1).GetComponent<InputField>().text);

        string[] dbText;
        bool successfulLogin = false;

        using (UnityWebRequest www = UnityWebRequest.Post(path + "verifylogin.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form load complete!");
                StringBuilder sb = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                {
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                }

                // Print Headers
#if (UNITY_EDITOR)
                Debug.Log(sb.ToString());
#endif

                if (www.GetResponseHeaders().Count > 0 && www.GetResponseHeaders().ContainsKey("LoginSucceed")) {

                    if (www.GetResponseHeaders()["LoginSucceed"] == "true")
                    {
                        Debug.Log("Login succesful");
                        dbText = www.downloadHandler.text.Split('\n'); // splits all echoed text from PHP
                        status = true;
                        GameSaveUtility.SaveID(int.Parse(dbText[1])); // know that the ID is echoed on the 2nd line of the db text body"
                        successfulLogin = true; // boolean trigger to retrieve all player data
                    } else { 
                        loginPopup.transform.GetChild(0).GetComponent<Text>().text = "ERROR: Login";
                        string bodyError = "Login failed: ";
                        if (www.GetResponseHeaders().ContainsKey("PasswordFail")) { bodyError += "Inccorect password.\n"; }
                        if (www.GetResponseHeaders().ContainsKey("AccountFail")) { bodyError += "Inccorect account name.\n";  }
                        loginPopup.transform.GetChild(1).GetComponent<Text>().text = bodyError;
                        loginPopup.SetActive(true);
#if (UNITY_EDITOR)
                        Debug.Log(www.downloadHandler.text);
#endif
                    }
                } else {
                    // Failed
                    loginPopup.transform.GetChild(0).GetComponent<Text>().text = "ERROR: Login";
                    loginPopup.transform.GetChild(1).GetComponent<Text>().text = "Unable to connect to server.";
                    loginPopup.SetActive(true);
#if (UNITY_EDITOR)
                    Debug.Log(www.downloadHandler.text);
#endif
                }
            }
        }

        // if successful login, retrieve all player data and store to PlayerPrefs
        if (successfulLogin) {
            form = new WWWForm();
            int playerID = GameSaveUtility.GetID();
            form.AddField("player_ID", playerID);
            form.AddField("request", "getlevels");

            using (UnityWebRequest www = UnityWebRequest.Post(path + "getplayerdata.php", form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("Form load complete!");
                    StringBuilder sb = new StringBuilder();
                    foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                    {
                        sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                    }

                    // Print Headers
#if (UNITY_EDITOR)
                    Debug.Log(sb.ToString());
#endif

                    if (www.GetResponseHeaders().Count > 0)
                    {
                        Debug.Log(www.downloadHandler.text);
                        dbText = www.downloadHandler.text.Split('\n');
                        MenuData menuData = JsonUtility.FromJson<MenuData>(dbText[0]);
                        menuData.StoreLevels(); // store level information back into local PlayerPrefs
                        string parsePlayer = dbText[1].Replace("[", "").Replace("]", ""); // format the player text outside of the array to be recognized as a PlayerData object JSON
                        PlayerData playerData = JsonUtility.FromJson<PlayerData>(parsePlayer);
                        playerData.SetPlayerInfo(); // store player information back into the local PlayerPrefs
                    }
                    else
                    {
                        // Failed
                        loginPopup.transform.GetChild(0).GetComponent<Text>().text = "ERROR: Login";
                        loginPopup.transform.GetChild(1).GetComponent<Text>().text = "Unable to connect to server.";
                        loginPopup.SetActive(true);
#if (UNITY_EDITOR)
                        Debug.Log(www.downloadHandler.text);
#endif
                    }
                }
            }

            if (successfulLogin)
                canvasManager.SwitchCanvas("menus");
        }
    }

    /// <summary>
    /// Posts the player name, player university team to the corresponding account using account ID
    /// </summary>
    /// <returns></returns>
    IEnumerator UploadPlayerRoutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("player_name", GameSaveUtility.GetPlayerName());
        form.AddField("player_team", GameSaveUtility.GetUniversity());
        form.AddField("player_ID", GameSaveUtility.GetID());

        using (UnityWebRequest www = UnityWebRequest.Post(path + "addplayer.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                StringBuilder sb = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                {
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                }

                // Print Headers
#if (UNITY_EDITOR)
                Debug.Log(sb.ToString());
#endif

                if (www.GetResponseHeaders().Count > 0 && www.GetResponseHeaders().ContainsKey("userAdded"))
                {
                    if (www.GetResponseHeaders()["userAdded"] == "true")
                        Debug.Log("User added succesful");
                    else
                        Debug.Log("User added failed");
                }
                else if (www.GetResponseHeaders().Count > 0 && www.GetResponseHeaders().ContainsKey("userUpdated"))
                {
                    if (www.GetResponseHeaders()["userUpdated"] == "true")
                        Debug.Log("User updated succesful");
                    else
                        Debug.Log("User updated failed");
                }
            }
        }
    }

    /// <summary>
    /// Updates the score the individual level by POSTing the player ID, level objective, level chapter, level difficulty, level mode to identify corect row 
    /// POSTs and score to store.
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateLevelDataRoutine(bool star)
    {
        WWWForm form = new WWWForm();

        form.AddField("player_id", GameSaveUtility.GetID());
        form.AddField("level_objective", ObjectiveUtility.CurrentObjective);
        form.AddField("level_chapter", ObjectiveUtility.CurrentChapter.ToString());
        form.AddField("level_difficulty", ObjectiveUtility.CurrentDifficulty.ToString());
        form.AddField("level_mode", ObjectiveUtility.CurrentGameMode.ToString());
        form.AddField("level_score", ObjectiveUtility.Score);
        if (star)
        {
            form.AddField("level_star", ObjectiveUtility.EarnedStars); // if a star is earned, POST the star earned
        }

        using (UnityWebRequest www = UnityWebRequest.Post(path + "storelevels.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                StringBuilder sb = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                {
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                }

                // Print Headers
#if (UNITY_EDITOR)
                Debug.Log(sb.ToString());
#endif

                if (www.GetResponseHeaders().Count > 0 && www.GetResponseHeaders().ContainsKey("storeLevel"))
                {
                    if (www.GetResponseHeaders()["storeLevel"] == "true")
                        Debug.Log("Level score successfully updated");
                    else
                        Debug.Log("Level score failed up upate");
                }
            }
        }
    }

    /// <summary>
    /// Used to update the Leaderboard UI - sends request to database to retrieve all university team scores
    /// </summary>
    /// <returns></returns>
    IEnumerator GetLeaderboardRoutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("request", "getleaderboard");

        using (UnityWebRequest www = UnityWebRequest.Post(path + "getleaderboard.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form load complete!");
                StringBuilder sb = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");

                // Print Headers
#if (UNITY_EDITOR)
                Debug.Log(sb.ToString());
#endif

                // Print Body
#if (UNITY_EDITOR)
                Debug.Log(www.downloadHandler.text);
#endif

                lbData = JsonUtility.FromJson<LeaderboardData>(www.downloadHandler.text);
            }
        }
    }

    /// <summary>
    /// Posts the earned stars to the player university using the account ID
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateLeaderboardRoutine()
    {
        WWWForm form = new WWWForm();

        form.AddField("player_id", GameSaveUtility.GetID());
        form.AddField("player_team", GameSaveUtility.GetUniversity());
        form.AddField("earnedStars", ObjectiveUtility.EarnedStars); // php will add the earned stars to the total stars of the player

        using (UnityWebRequest www = UnityWebRequest.Post(path + "updateleaderboard.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                StringBuilder sb = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                {
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                }

                // Print Headers
#if (UNITY_EDITOR)
                Debug.Log(sb.ToString());
#endif

                if (www.GetResponseHeaders().Count > 0 && www.GetResponseHeaders().ContainsKey("leaderboardUpdated"))
                {
                    if (www.GetResponseHeaders()["leaderboardUpdated"] == "true")
                    {
                        Debug.Log("Team score updating succesfully");
                        // Print Body
#if (UNITY_EDITOR)
                        Debug.Log(www.downloadHandler.text);
#endif
                    }
                    else
                    {
                        Debug.Log("Score saving failed");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Posts the level objective name, chapter, mode, difficulty, score and star to the PlayerLevels table where it matches the player ID
    /// Called at the end of every level - updated simulataneously with the local storage of the level scores
    /// </summary>
    /// <returns></returns>
    IEnumerator StoreLevelDataRoutine(string objective, int score, int star)
    {
        WWWForm form = new WWWForm();
        form.AddField("player_ID", GameSaveUtility.GetID());
        form.AddField("level_name", objective);
        form.AddField("level_chapter", ObjectiveUtility.CurrentChapter.ToString());
        form.AddField("level_mode", ObjectiveUtility.CurrentGameMode.ToString());
        form.AddField("level_difficulty", ObjectiveUtility.CurrentDifficulty.ToString());
        form.AddField("level_score", score);
        form.AddField("level_star", star);

        using (UnityWebRequest www = UnityWebRequest.Post(path + "storelevels.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                StringBuilder sb = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                {
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                }

                // Print Headers
#if (UNITY_EDITOR)
                Debug.Log(sb.ToString());
                Debug.Log(www.downloadHandler.text);
#endif

                if (www.GetResponseHeaders().Count > 0 && www.GetResponseHeaders().ContainsKey("levelUpdated"))
                {
                    if (www.GetResponseHeaders()["levelUpdated"] == "true")
                    {
                        Debug.Log("Level score updating succesfull");
                        // Print Body
#if (UNITY_EDITOR)
                        Debug.Log(www.downloadHandler.text);
#endif
                    }
                    else
                    {
                        Debug.Log("Score saving failed");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Posts the player's money, team, first time boolean and name to the PlayerInfo database where it matches the player ID
    /// </summary>
    /// <param name="logout">Determines if the coroutine is being called alongside a logout action. If true, delete all player prefs</param>
    /// <returns></returns>
    IEnumerator StorePlayerDataRoutine(bool logout)
    {
        WWWForm form = new WWWForm();
        form.AddField("player_id", GameSaveUtility.GetID());
        form.AddField("player_money", GameSaveUtility.GetTotalBalance());
        if (GameSaveUtility.GetChapProgress(1))
            form.AddField("progress1", 1);
        if (GameSaveUtility.GetChapProgress(2))
            form.AddField("progress2", 1);
        if (GameSaveUtility.GetChapProgress(3))
            form.AddField("progress3", 1);


        using (UnityWebRequest www = UnityWebRequest.Post(path + "updateplayerinfo.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                StringBuilder sb = new StringBuilder();
                foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
                {
                    sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
                }

                // Print Headers
#if (UNITY_EDITOR)
                Debug.Log(sb.ToString());
#endif

                if (www.GetResponseHeaders().Count > 0 && www.GetResponseHeaders().ContainsKey("userUpdated"))
                {
                    if (www.GetResponseHeaders()["userUpdated"] == "true")
                    {
                        Debug.Log("Player data saved succesfully");
                        if (logout)
                        {
                            Debug.Log("LOGOUT");
                            GameSaveUtility.Reset(); // if updating the player is called during a logout action, delete all player pref local storage
                            canvasManager.OpenStart(); // makes sure the page when logging back in starts with the start menu and not settings
                        }
                        // Print Body
#if (UNITY_EDITOR)
                        Debug.Log(www.downloadHandler.text);
#endif
                    }
                    else
                    {
                        Debug.Log("Player data saving failed");
                        Debug.Log(www.downloadHandler.text);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Tests the connection.
    /// </summary>
    /// <returns>The connection.</returns>
    /// <param name="action">Action.</param>
    IEnumerator TestConnection()
    {
        WWWForm form = new WWWForm();

        using (UnityWebRequest www = UnityWebRequest.Post(path + "connectioncheck.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log("No connection. No progress will be saved.");
                if (GameSaveUtility.OfflineMode != 2)
                    GameSaveUtility.SetConnectionMode(1);
            }
            else
            {
                Debug.Log("Connection established.");
                GameSaveUtility.SetConnectionMode(0);
            }
        }
    }
}
