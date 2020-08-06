using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke
/// Populates the leaderboard UI with panels for each team/university - data comes from the DBManager as an iteration through LeaderboardData
/// </summary>
public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] Transform uiRoot;
    [SerializeField] LeaderboardUI _uiPrefab;

    [SerializeField] PodiumUI firstPlace;
    [SerializeField] PodiumUI secondPlace;
    [SerializeField] PodiumUI thirdPlace;

    [SerializeField] Text playerName;
    [SerializeField] Text playerStars;
    [SerializeField] Text playerTeam;

    [SerializeField] GameObject offlineText;

    private List<GameObject> teamUIList = new List<GameObject>();

    private void Awake()
    {
        // Set individual player UI
        playerName.text = GameSaveUtility.GetPlayerName();
        playerStars.text = GameSaveUtility.GetTotalStar().ToString();
        playerTeam.text = GameSaveUtility.GetUniversity();

        if (GameSaveUtility.OfflineMode != 2)
        {
            offlineText.SetActive(false);
            UpdateLeaderboard(DBManager.Instance.GetLeaderboardData()); // get most recent leaderboard results - used at the opening of the game
        } else
        {
            ClearLeaderboard();
            offlineText.SetActive(true);
        }
    }

    /// <summary>
    /// Calls methods to update the leaderboard UI
    /// </summary>
    /// <param name="lbData">LeaderboardData contains list of team stats</param>
    public void UpdateLeaderboard(LeaderboardData lbData)
    {
        ClearLeaderboard();

        // set first place podium
        if (lbData.teamList.Count > 0 && lbData.teamList[0] != null)
            firstPlace.UpdatePodiumData(lbData.teamList[0].teamName, lbData.teamList[0].teamStars.ToString());

        if (lbData.teamList.Count > 1 && lbData.teamList[1] != null)
            secondPlace.UpdatePodiumData(lbData.teamList[1].teamName, lbData.teamList[1].teamStars.ToString());

        if (lbData.teamList.Count > 2 && lbData.teamList[2] != null)
            thirdPlace.UpdatePodiumData(lbData.teamList[2].teamName, lbData.teamList[2].teamStars.ToString());

        if (lbData.teamList.Count > 3)
        {
            for (int i = 0; i < lbData.teamList.Count; i++)
            {
                Debug.Log(lbData.teamList[i].teamName);

                // instantiate and add new leaderboard row
                LeaderboardUI lbUI = Instantiate(_uiPrefab, uiRoot);

                // update leaderboard row info
                lbUI.UpdateTeamData(lbData.teamList[i].teamName, lbData.teamList[i].teamStars.ToString(), (i + 1).ToString());

                // save leaderboard row to a list for later refresh cleanup
                teamUIList.Add(lbUI.gameObject);
            }
        }
    }

    private void ClearLeaderboard()
    {
        // clear and clean leaderboard row info in order to update
        for (int i = 0; i < teamUIList.Count; i++)
        {
            Destroy(teamUIList[i]);
        }
        firstPlace.ClearPodiumData();
        secondPlace.ClearPodiumData();
        thirdPlace.ClearPodiumData();
        teamUIList.Clear();
    }
}
