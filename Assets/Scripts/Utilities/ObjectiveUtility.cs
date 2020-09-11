using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written By Oliver Vennike Riisager. Modified by Kimberly Burke
/// Mediator inbetween scenes for all things level related.
/// </summary>
public static class ObjectiveUtility
{
    //Enums
    public enum GameMode { Training, Regular, Master }
    public static GameMode CurrentGameMode { get; private set; } = GameMode.Regular;
    public enum Difficulty { Easy, Medium, Hard }
    public static Difficulty CurrentDifficulty { get; private set; }
    public enum Chapter { None, Chapter1, Chapter2, Chapter3 }
    public static Chapter CurrentChapter { get; private set; } = Chapter.None;

    //Level
    public static string CurrentObjective { get; private set; }
    public static int EarnedStars { get; private set; } = 1;
    private static List<SliceController> sliceReset;
    public static bool IsMotor { get; set; } = false;

    public static Queue<SliceController> Slices { get; private set; }

    public static GameObject[] Objectives { get; private set; }
    public static Dictionary<Chapter, Dictionary<Difficulty, string[]>> ChapterSortedLevels { get; private set; }

    //Score
    public static int ScoreMultiplier { get; private set; }
    public static int MasterModeMulti { get; private set; } = 2;
    public static int Score { get; set; }

    //Setters
    public static void SetGameMode(GameMode gameMode)
    {
        CurrentGameMode = gameMode;
    }
    public static void SetChapter(Chapter chapter)
    {
        CurrentChapter = chapter;
    }
    public static void SetDifficulty(Difficulty difficulty)
    {
        CurrentDifficulty = difficulty;
        switch (difficulty)
        {
            case Difficulty.Easy:
                ScoreMultiplier = 10;
                break;
            case Difficulty.Medium:
                if (CurrentChapter == Chapter.Chapter1)
                    ScoreMultiplier = 10;
                else
                    ScoreMultiplier = 15;
                break;
            case Difficulty.Hard:
                ScoreMultiplier = 10;
                break;
        }
    }
    public static void SetObjective(string level)
    {
        CurrentObjective = level;
    }

    /// <summary>
    /// Gets all slices in a scene and puts them into a queue, used by the levelmanager
    /// </summary>
    public static void SetSlices()
    {
        GameObject[] sceneSlices = GameObject.FindGameObjectsWithTag("Slice");
        List<SliceController> SlicesList = new List<SliceController>(sceneSlices.Length); //Find and create a list to hold all slices
        sliceReset = new List<SliceController>(sceneSlices.Length); //Create a list from which we can reset
        Slices = new Queue<SliceController>(); //Create a new queue
        for (int i = 0; i < sceneSlices.Length; i++)
        {
            sliceReset.Add(sceneSlices[i].GetComponent<SliceController>());
            SlicesList.Add(sceneSlices[i].GetComponent<SliceController>());
        }
        if (!IsMotor)
        {
            SlicesList.Sort(); //Sort if not motor
        }
        else //Sort AND reverse if its motor
        {
            SlicesList.Sort();
            SlicesList.Reverse();
        }

        sliceReset.Sort();

        foreach (var slice in SlicesList)
        {
            Slices.Enqueue(slice);
        }
    }

    /// <summary>
    /// Resets the score and all slices.
    /// </summary>
    public static void Restart()
    {
        Score = 0;
        ResetSlices();
    }

    /// <summary>
    /// Resets the queue of slices for future use. Can be used if there is need for restart or infinite mode.
    /// </summary>
    public static void ResetSlices()
    {
        Slices = new Queue<SliceController>(Slices.Count);//Create a new queue with same count as orig
        foreach (SliceController slice in sliceReset)
        {
            slice.ResetSlice();
            Slices.Enqueue(slice);
        }
    }

    /// <summary>
    /// Initializes a chapter by going through every child under a specific chapter
    /// Every child is then sorted depending on their parents difficulty, this is sorted in a 2 layer dictionary
    /// </summary>
    /// <param name="difficulty">The key in the innermost dictionary</param>
    /// <param name="objectives">The objectives we want to store, our values</param>
    /// <param name="chapterTag">Key to outermost dictionary</param>
    public static void InitializeChapter(Difficulty difficulty, string[] objectives, Chapter chapterTag)
    {
        if (ChapterSortedLevels == null)
            ChapterSortedLevels = new Dictionary<Chapter, Dictionary<Difficulty, string[]>>(); //If we havent initialized the dict

        if (!ChapterSortedLevels.ContainsKey(chapterTag)) //If the chapter has not yet been added
        {
            var initialLevel = new Dictionary<Difficulty, string[]>(); //Create a placeholder for initial entry
            initialLevel.Add(difficulty, objectives); //Create and add new entry for placeholder

            ChapterSortedLevels.Add(chapterTag, initialLevel); //Add entry to outermost
        }
        else
        {
            if (!ChapterSortedLevels[chapterTag].ContainsKey(difficulty)) //If the given difficulty is not in the current chapter, add it
                ChapterSortedLevels[chapterTag].Add(difficulty, objectives);
#if UNITY_ENGINE
            else
                Debug.Log("That difficulty is already saved in this chapter : " + chapterTag + " Difficulty " + difficulty + " Skipping");
#endif
        }
    }

    /// <summary>
    /// Gets the current score
    /// </summary>
    /// <returns>Current score</returns>
    public static int GetScore()
    {
        int currentScore = Score; // score changes in PlayerListener
        int scoreMult = ScoreMultiplier;
        if (CurrentGameMode != GameMode.Master)
            return currentScore * scoreMult;
        else
            return currentScore * scoreMult * MasterModeMulti;
    }

    /// <summary>
    /// Static setter for the Objectives property
    /// </summary>
    /// <param name="sceneObjectives"></param>
    public static void SetObjectives(GameObject[] sceneObjectives)
    {
        Objectives = sceneObjectives;
    }

    /// <summary>
    /// Gets a Objective component connected to the transform
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static Objective GetObjective(Transform transform)
    {
        return transform.GetComponent<Objective>();
    }

    public static string GetSpecificObjective(string wanted)
    {
        foreach (Chapter chapter in System.Enum.GetValues(typeof(Chapter))) //Go through all chapter enums
        {
            if (chapter.Equals(Chapter.None))
                continue;
            foreach (Difficulty diff in System.Enum.GetValues(typeof(Difficulty))) //Go through all chapter enums
            {
                if (!ChapterSortedLevels[chapter].ContainsKey(diff))
                    continue;
                var current = ChapterSortedLevels[chapter][diff];
                for (int i = 0; i < current.Length; i++)
                {
                    if (current[i] == wanted)
                        return current[i];
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Returns a jagged array, 1st dim is difficulty, 2nd is levels
    /// </summary>
    /// <returns>All levels contained in each difficulty </returns>
    public static string[][] CurrentLevels()
    {
        int numOfDiffs = System.Enum.GetValues(typeof(Difficulty)).Length; //Get the amount of diffs
        string[][] levels = new string[numOfDiffs][]; //Init array with that many rows
        for (int i = 0; i < numOfDiffs; i++)
        {
            if (!ChapterSortedLevels[CurrentChapter].ContainsKey((Difficulty)i))
                continue;
            levels[i] = ChapterSortedLevels[CurrentChapter][(Difficulty)i]; //For each row, add corresponding elements
        }
        return levels;
    }
}
