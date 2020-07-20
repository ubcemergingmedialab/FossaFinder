using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Oliver Vennike Riisager - Edited by Kimberly Burke
/// Saves scores and playinfo.
/// </summary>
public static class GameSaveUtility
{
    const string DEFAULT_SCORE = "New";
    const string FIRST_TIME_PLAYER = "FirstTimePlayer";
    const string MASTER_MODE = "Master";
    const string TOTAL_SCORE = "TotalScore";
    const string TOTAL_BALANCE = "TotalBalance";
    const string TOTAL_STAR = "StarTotal";
    const string PERFECT_GAME = "Perfect";
    const string PERFECT_MASTER = "Perfect Master";
    const string PLAYER_NAME = "PlayerName";
    const string PLAYER_UNI = "PlayerUniversity";
    const string PLAYER_ID = "PlayerID";

    const string CHAP_ONE = "ChapterOneProgress";
    const string CHAP_TWO = "ChapterTwoProgress";
    const string CHAP_THREE = "ChapterThreeProgress";

    static string currentPlayerName;
    static bool playCutScene = false;

    public static int OfflineMode { get; private set; } // 0 = online, 1 = offline alert, 2 = offline confirmed


    public static bool PlayCutScene { get; private set; }

    /// <summary>
    /// Saves a score as a string. SHOULD USE OVERLOADED
    /// </summary>
    /// <param name="numCompletedObjectives">The amount of completed objectives</param>
    /// <param name="totalNumObjectives">The total amount of available objectives for the level</param>
    public static void SaveLevelScore(int numCompletedObjectives, int totalNumObjectives)
    {
        string score = numCompletedObjectives + "/" + totalNumObjectives;
        PlayerPrefs.SetString(ObjectiveUtility.CurrentObjective, score);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Saves a score as points to a string
    /// </summary>
    /// <param name="numCompletedObjectives">The number of completed objectives</param>
    /// <param name="pointValue">The value of the points</param>
    public static void SaveLevelScoreInt(int score, bool perfected)
    {
        if (ObjectiveUtility.CurrentGameMode == ObjectiveUtility.GameMode.Master)
        {
            if (PlayerPrefs.GetInt(MASTER_MODE + ObjectiveUtility.CurrentObjective, -1) < score)
                PlayerPrefs.SetInt(MASTER_MODE + ObjectiveUtility.CurrentObjective, score);
            if (perfected)
                AddPerfectStar("Perfect  Master");
        }
        else if (ObjectiveUtility.CurrentGameMode == ObjectiveUtility.GameMode.Regular)
        {
            if (PlayerPrefs.GetInt(ObjectiveUtility.CurrentObjective, -1) < score)
                PlayerPrefs.SetInt(ObjectiveUtility.CurrentObjective, score);
            if (perfected)
                AddPerfectStar("Perfect");
        }
        UpdateTotalScore(score);
        AddTotalBalance(score);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// sets the status of the star for the game mode
    /// </summary>
    /// <param name="gameType"></param>
    private static void AddPerfectStar(string gameType)
    {
        // set status of perfected game 
        if (PlayerPrefs.GetInt(gameType + ObjectiveUtility.CurrentObjective, -1) < 1)
        {
            PlayerPrefs.SetInt(gameType + ObjectiveUtility.CurrentObjective, 1); // set PERFECT_GAMETYPE for objective to 1 for star to be true
            PlayerPrefs.SetInt(TOTAL_STAR, PlayerPrefs.GetInt(TOTAL_STAR) + ObjectiveUtility.EarnedStars); // Add star to the total star count for the player
        }
    }

    /// <summary>
    /// Used to update the players totalscore
    /// </summary>
    /// <param name="score">The amount to add</param>
    private static void UpdateTotalScore(int score)
    {
        var totalScore = PlayerPrefs.GetInt(TOTAL_SCORE, -1);//If we dont have any values saved
        if (totalScore == -1)
            PlayerPrefs.SetInt(TOTAL_SCORE, score);
        else
        {
            PlayerPrefs.SetInt(TOTAL_SCORE, totalScore + score);//Else, add to the current score
        }
    }

    /// <summary>
    /// Used to get the players totalscore
    /// </summary>
    /// <returns></returns>
    public static int GetTotalScore()
    {
        var totalScore = PlayerPrefs.GetInt(TOTAL_SCORE, -1);//If we dont have any values saved
        if (totalScore == -1)
            return ObjectiveUtility.GetScore();
        else
        {
            return totalScore;
        }
    }

    /// <summary>
    /// Used to get the players total star count
    /// </summary>
    /// <returns></returns>
    public static int GetTotalStar()
    {
        var totalStar = PlayerPrefs.GetInt(TOTAL_STAR, -1); // If we don't have any values saved
        if (totalStar == -1)
            return 0;
        else
            return totalStar;
    }

    public static void SetTotalStar(int stars)
    {
        PlayerPrefs.SetInt(TOTAL_STAR, stars);
    }

    /// <summary>
    /// Used to add to the players total balance
    /// </summary>
    /// <param name="score"></param>
    public static void AddTotalBalance(int score)
    {
        var totalBalance = PlayerPrefs.GetInt(TOTAL_BALANCE, -1); //If we dont have any values saved
        if (totalBalance == -1)
            PlayerPrefs.SetInt(TOTAL_BALANCE, score);
        else
        {
            PlayerPrefs.SetInt(TOTAL_BALANCE, totalBalance + score); //Else, add to the current score
        }
    }

    /// <summary>
    /// Determines if the player has enough balance for a purchase
    /// If so, retracts from total balance and returns true
    /// Else, returns false
    /// </summary>
    /// <param name="amount">Amount to Retract</param>
    /// <returns></returns>
    public static bool RetractTotalBalance(int amount)
    {
        var totalBalance = PlayerPrefs.GetInt(TOTAL_BALANCE, -1);//If we dont have any values saved
        if (totalBalance == -1)
            return false;
        else
        {
            PlayerPrefs.SetInt(TOTAL_BALANCE, totalBalance - amount);//Else, add to the current score
            return true;
        }
    }

    /// <summary>
    /// Used to get a saved score as int
    /// </summary>
    /// <param name="objectiveName"></param>
    /// <returns></returns>
    public static int GetLevelScoreInt(string objectiveName, int pointValue)
    {
        int score = PlayerPrefs.GetInt(objectiveName, -1);
        if (score < 0)
            return score;
        else
            return score;
    }

    /// <summary>
    /// Used to get a saved score as int
    /// </summary>
    /// <param name="objectiveName"></param>
    /// <returns></returns>
    public static int GetMasterLevelScoreInt(string objectiveName, int pointValue)
    {
        int score = PlayerPrefs.GetInt(MASTER_MODE + objectiveName, -1);
        if (score < 0)
            return score;
        else
            return score;
    }

    /// <summary>
    /// Used to save a star
    /// </summary>
    /// <param name="objectiveName"></param>
    /// <returns></returns>
    public static bool GetStarBoolean(string objectiveName, string gameType)
    {
        int star = PlayerPrefs.GetInt(gameType + objectiveName, -1);
        if (star == 1)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Checks if it is the first time the player is playing the game.
    /// </summary>
    /// <returns></returns>
    public static bool GetFirstTimePlayer()
    {
        if (PlayerPrefs.GetInt(FIRST_TIME_PLAYER, 0) == 0) //If no value exists
            playCutScene = true; //Set to true
        else
            playCutScene = false; //Set to false
        return playCutScene;
    }

    public static bool GetChapProgress(int chapter)
    {
        bool result = false;
        switch (chapter)
        {
            case 1:
                if (PlayerPrefs.GetInt(CHAP_ONE, 0) == 1) //If no value exists
                    result = true; //Set to true
                break;
            case 2:
                if (PlayerPrefs.GetInt(CHAP_TWO, 0) == 1) //If no value exists
                    result = true; //Set to true
                break;
            case 3:
                if (PlayerPrefs.GetInt(CHAP_THREE, 0) == 1) //If no value exists
                    result = true; //Set to true
                break;
            default:
                break;
        }
        return result;
    }

    public static void SetFirstTimePlayer()
    {
        PlayerPrefs.SetInt(FIRST_TIME_PLAYER, 1); //Set to 1
        PlayerPrefs.Save();
    }

    public static void SetChapProgress(int chapter)
    {
        switch (chapter)
        {
            case 1:
                PlayerPrefs.SetInt(CHAP_ONE, 1); //Set to 1
                PlayerPrefs.Save();
                break;
            case 2:
                PlayerPrefs.SetInt(CHAP_TWO, 1); //Set to 1
                PlayerPrefs.Save();
                break;
            case 3:
                PlayerPrefs.SetInt(CHAP_THREE, 1); //Set to 1
                PlayerPrefs.Save();
                break;
            default:
                break;
        }
    }

    public static void SavePlayerName(string wantToSave)
    {
        string rnd = "-1,2";
        string found = PlayerPrefs.GetString(PLAYER_NAME, rnd);
        if (found == rnd)
        {
            PlayerPrefs.SetString(PLAYER_NAME, wantToSave);
            PlayerPrefs.Save();
        }
    }
   
    /// <summary>
    /// Saves the unviersity selection from the drop down
    /// </summary>
    /// <param name="university"></param>
    public static void SaveUniversity(string university)
    {
        PlayerPrefs.SetString(PLAYER_UNI, university);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Saves the playerID selection from the drop down
    /// </summary>
    /// <param name="id"></param>
    public static void SaveID(int id)
    {
        PlayerPrefs.SetInt(PLAYER_ID, id);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Used to get player name as a string
    /// </summary>
    /// <returns>string of player name</returns>
    public static string GetPlayerName()
    {
        string rnd = "-1,2";
        string found = PlayerPrefs.GetString(PLAYER_NAME, rnd);
        if (found == rnd)
        {
            return null;
        }
        else
        {
            return found;
        }
    }

    /// <summary>
    /// Used to get university team as a string
    /// </summary>
    /// <returns>string of university/team name</returns>
    public static string GetUniversity()
    {
        string uni = PlayerPrefs.GetString(PLAYER_UNI, string.Empty);
        if (uni == string.Empty)
            return null;
        return uni;
    }

    /// <summary>
    /// Used to get id as a int
    /// </summary>
    /// <returns>integer of player id</returns>
    public static int GetID()
    {
        int id = PlayerPrefs.GetInt(PLAYER_ID,-1);
        return id;
    }

    public static int GetTotalBalance()
    {
        int balance = PlayerPrefs.GetInt(TOTAL_BALANCE, 0);
        return balance;
    }

    public static void Reset()
    {
        PlayerPrefs.DeleteAll();
        ObjectiveUtility.SetChapter(ObjectiveUtility.Chapter.None);
    }

    public static void SetConnectionMode(int offline)
    {
        OfflineMode = offline;
    }
}

// Player Data Class
public class PlayerData
{
    public string playerName;
    public int playerStars;
    public string playerTeam;
    public int playerID;
    public int playerMoney;
    public int progress1;
    public int progress2;
    public int progress3;

    // Constructor
    public PlayerData(string pName, int pStars, string pTeam, int pID, int pMoney, int p1, int p2, int p3)
    {
        playerName = pName;
        playerStars = pStars;
        playerTeam = pTeam;
        playerID = pID;
        playerMoney = pMoney;
        progress1 = p1;
        progress2 = p2;
        progress3 = p3;
    }

    public void SetPlayerInfo()
    {
        if (progress1 > 0)
            GameSaveUtility.SetChapProgress(1);
        if (progress2 > 0)
            GameSaveUtility.SetChapProgress(2);
        if (progress3 > 0)
            GameSaveUtility.SetChapProgress(3);
        GameSaveUtility.SavePlayerName(playerName);
        GameSaveUtility.SaveUniversity(playerTeam);
        GameSaveUtility.AddTotalBalance(playerMoney);
        GameSaveUtility.SetTotalStar(playerStars);
        PlayerPrefs.Save();
    }

    public override string ToString()
    {
        string player = "";
        player = "Name: " + playerName + " Team: " + playerTeam + " Money: " + playerMoney + "  Stars: " + playerStars +
            " Chap 1: " + progress1 + " Chap 2: " + progress2 + " Chap 3: " + progress3;
        return player;
    }
}

// Team Data Class
[System.Serializable]
public class TeamData
{
    public string teamName;
    public int teamStars;

    // Constructor
    public TeamData(string tName, int tStars)
    {
        teamName = tName;
        teamStars = tStars;
    }
}

//leaderboardData list class
[System.Serializable]
public class LeaderboardData
{
    public List<TeamData> teamList = new List<TeamData>();
}

[System.Serializable]
public class LevelData
{
    public string objectiveName;
    public int score;
    public int star;
    public string chapter;
    public string mode;
    public string difficulty;

    public LevelData(string objective, int levelScore, int levelStar, string levelChap, string levelMode, string levelDiff)
    {
        objectiveName = objective;
        score = levelScore;
        star = levelStar;
        chapter = levelChap;
        mode = levelMode;
        difficulty = levelDiff;
    }
}

[System.Serializable]
public class MenuData
{
    public List<LevelData> levelList = new List<LevelData>();

    public void StoreLevels()
    {
        foreach (LevelData level in levelList)
        {
            switch (level.chapter)
            {
                case "Chapter1":
                    PlayerPrefs.SetInt(level.objectiveName, level.score);
                    if (level.star > 0)
                        PlayerPrefs.SetInt("Perfect" + level.objectiveName, level.star);
                    break;
                case "Chapter2":
                    if (level.mode == "Master")
                    {
                        // Chapter 2 Master Levels
                        PlayerPrefs.SetInt("Master" + level.objectiveName, level.score);
                        if (level.star > 0)
                            PlayerPrefs.SetInt("Perfect Master" + level.objectiveName, level.star);
                    } else
                    {
                        // Chapter 2 Challenge Levels 
                        PlayerPrefs.SetInt(level.objectiveName, level.score);
                        if (level.star > 0)
                            PlayerPrefs.SetInt("Perfect" + level.objectiveName, level.star);
                    }
                    break;
                default:
                    Debug.Log("Invalid level - no chapter associated with level data");
                    break;
            }
            PlayerPrefs.Save();
        }
    }
}