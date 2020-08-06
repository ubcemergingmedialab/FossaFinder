using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Created by Niccolo Pucci. Edited by Oliver Riisager
/// Provides utility methods for loading levels between Scenes using ObjectiveUtility as reference
/// </summary>
public static class LevelLoaderUtility
{
    private const int MAIN_MENU_SCENE_BUILD_INDEX = 0;
    private const int BRAIN_SLICE_SCENE_BUILD_INDEX = 1;
    private const int SPINAL_TRACT_SCENE_BUILD_INDEX = 2;
    private const int CORTEX_ARTERY_SCENE_BUILD_INDEX = 3;

    public static AsyncOperation Async { get; private set; }

    public static void GoToLevelSelect()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Loads scene depending on which chapter we are currently in
    /// </summary>
    public static void LoadLevel()
    {
        ObjectiveUtility.Score = 0;
        int sceneBuildIndex = GetSceneBuildNumber(ObjectiveUtility.CurrentChapter);
        SceneManager.LoadScene(sceneBuildIndex);
    }

    /// <summary>
    /// Loads tutorial scene
    /// </summary>
    public static void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    /// <summary>
    /// Loads scene depending on which chapther we are currently in
    /// </summary>
    /// <param name="IsAsync">Async load, default true</param>
    public static void LoadLevel(bool IsAsync = true)
    {
        ObjectiveUtility.Score = 0;
        int sceneBuildIndex = GetSceneBuildNumber(ObjectiveUtility.CurrentChapter);
        Async = SceneManager.LoadSceneAsync(sceneBuildIndex);
        Async.allowSceneActivation = false;
    }

    /// <summary>
    /// Determines the Scene Build Index for the desired level 
    /// </summary>
    private static int GetSceneBuildNumber(ObjectiveUtility.Chapter levelChapter)
    {
        // Will be expanded once more levels/scenes are added.
        if (levelChapter == ObjectiveUtility.Chapter.Chapter1)
        {
            return BRAIN_SLICE_SCENE_BUILD_INDEX;
        }
        else if (levelChapter == ObjectiveUtility.Chapter.Chapter2)
        {
            return SPINAL_TRACT_SCENE_BUILD_INDEX;
        } else if (levelChapter == ObjectiveUtility.Chapter.Chapter3)
        {
            return CORTEX_ARTERY_SCENE_BUILD_INDEX;
        }

        string debugWarningStr = "No Scene Build Index Specified for: " + levelChapter.ToString();
        Debug.LogWarning(debugWarningStr);

        return MAIN_MENU_SCENE_BUILD_INDEX;
    }
}
