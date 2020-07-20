using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to determine when to drop a "crumb" based on a timer. Used to draw the line behind the player.
/// </summary>
public static class CrumbUtility
{
    public static float crumbTimer;
    public static float crumbOrigTimer;

    public static List<Vector3> Crumbs { get; set; }

    public static void Start(float crumbSpeed)
    {
        Crumbs = new List<Vector3>((int)(crumbSpeed / 2 + 1));
        crumbOrigTimer = crumbSpeed;
        crumbTimer = crumbOrigTimer;
    }
    public static bool DecreaseTimer(float deltaTime)
    {
        crumbTimer -= deltaTime;
        if(crumbTimer <= 0)
        {
            crumbTimer = crumbOrigTimer;
            return true;
        }
        else
        {
            return false;
        }
    }
}
