using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke
/// Updates the Podium UI on the leaderbaord canvas - data comes from the DBManager as an iteration through LeaderboardData
/// (takes the first three items from the list for first, second and third place) 
/// </summary>
public class PodiumUI : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Text starText;
    [SerializeField] private Image iconImage;

    /// <summary>
    /// Updates the podium UI elements - used when no team icon is available
    /// </summary>
    /// <param name="tName">team name</param>
    /// <param name="tStars">number of stars earned by the team</param>
    public void UpdatePodiumData(string tName, string tStars)
    {
        nameText.text = tName;
        starText.text = tStars;
    }

    /// <summary>
    /// Updates the podium UI elemetns - overrides when team icon is available to be set
    /// </summary>
    /// <param name="tName">team name</param>
    /// <param name="tStars">number of stars earned by the team</param>
    /// <param name="tIcon">icon for team</param>
    public void UpdatePodiumData(string tName, string tStars, Sprite tIcon)
    {
        Debug.Log(tName);
        nameText.text = tName;
        starText.text = tStars;
        // iconImage.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 25, 75, 75), new Vector2(0.5f, 1));
        SetColor(tName);
    }

    /// <summary>
    /// Reset before loading updated leaderboard canvas - clears all podium data and replaces with default values
    /// </summary>
    public void ClearPodiumData()
    {
        nameText.text = "N/A";
        starText.text = "0";
        iconImage.color = Color.white;
    }

    private void SetColor(string teamColor)
    {
        Color color = Color.white;
        switch (teamColor)
        {
            case "Red":
                color = new Color(170, 0, 0); 
                break;
            case "Yellow":
                color = new Color(230, 200, 0);
                break;
            case "Green":
                color = new Color(0, 170, 0);
                break;
            case "Blue":
                color = new Color(0, 0, 170);
                break;
            default:
                color = Color.white;
                break;
        }
        iconImage.color = color;
    }
}
