using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written by Oliver Vennike Riisager
/// Used as a static holder for MinimapCamFly values across scenes.
/// </summary>
public static class MinimapCamUtility
{
    public static Transform Target { get; set; }
    public static Transform LookTarget { get; set; }

    private static bool endGame;
    public static bool EndGame { get { return endGame; } set { endGame = value; } }
}
