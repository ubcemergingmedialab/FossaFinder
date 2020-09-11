using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke
/// Udpates leaderboard UI prefab - data comes from the DBManager as an iteration through LeadeboardData
/// </summary>
public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text starText;
    [SerializeField] private Text rankText;

    /// <summary>
    /// Constructor for the UniversityPrefab
    /// </summary>
    /// <param name="tName">team name</param>
    /// <param name="tStars">number of stars for the team</param>
    /// <param name="tRank">rank of team on leaderboard</param>
    public void UpdateTeamData(string tName, string tStars, string tRank)
    {
        rankText.text = tRank + ".";
        nameText.text = "Team " + tName;
        starText.text = tStars;
    }
}
