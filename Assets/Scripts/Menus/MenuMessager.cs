using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written by Oliver Vennike Riisager - Used to set the up the game, ie. chapter, level, gamemode, difficulty.
/// Also feeds information to the gameobjectiveutility.
/// </summary>
public class MenuMessager : MonoBehaviour
{
    public void SetChapter1()
    {
        ObjectiveUtility.SetChapter(ObjectiveUtility.Chapter.Chapter1);
    }
    public void SetChapter2()
    {
        ObjectiveUtility.SetDifficulty(ObjectiveUtility.Difficulty.Easy);
        ObjectiveUtility.SetChapter(ObjectiveUtility.Chapter.Chapter2);
    }
    public void SetChapter3()
    {
        ObjectiveUtility.SetDifficulty(ObjectiveUtility.Difficulty.Easy);
        ObjectiveUtility.SetGameMode(ObjectiveUtility.GameMode.Regular);
        ObjectiveUtility.SetChapter(ObjectiveUtility.Chapter.Chapter3);
    }
    public void SetTraining()
    {
        ObjectiveUtility.SetGameMode(ObjectiveUtility.GameMode.Training);
    }
    public void SetRegular()
    {
        ObjectiveUtility.SetGameMode(ObjectiveUtility.GameMode.Regular);
    }
    public void SetMaster()
    {
        ObjectiveUtility.SetGameMode(ObjectiveUtility.GameMode.Master);
    }
    public void EasyDifficulty()
    {
        ObjectiveUtility.SetDifficulty(ObjectiveUtility.Difficulty.Easy);
    }
    public void MediumDifficulty()
    {
        ObjectiveUtility.SetDifficulty(ObjectiveUtility.Difficulty.Medium);
    }
    public void HardDifficulty()
    {
        ObjectiveUtility.SetDifficulty(ObjectiveUtility.Difficulty.Hard);
    }
    private void Awake()
    {
        //GetLevels();
    }

    /// <summary>
    /// Gets all current levels in menus and sends them to ObjectiveUtility
    /// </summary>
    private void GetLevels()
    {
        foreach (ObjectiveUtility.Difficulty difficulty in Enum.GetValues(typeof(ObjectiveUtility.Difficulty))) // Go through each difficulty
        {
            GameObject[] foundParents = GameObject.FindGameObjectsWithTag(difficulty.ToString()); //Find objects with that difficulty tag
            for (int i = 0; i < foundParents.Length; i++)
            {
                ObjectiveUtility.Chapter ChapterTag = FindChapterParent(foundParents[i]); //Get the chapter for those structures
                string[] childrenInFoundParents = ReadChildren(foundParents[i], ChapterTag); //Get all children -- Structures
                ObjectiveUtility.InitializeChapter(difficulty, childrenInFoundParents, ChapterTag); // initialize the chapter
            }
        }
    }

    /// <summary>
    /// Finds the wrapping parent that is the "chapter"
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    private ObjectiveUtility.Chapter FindChapterParent(GameObject gameObject)
    {
        string chapterParent = gameObject.transform.parent.parent.tag; //We have to go through two layers with current menus
        ObjectiveUtility.Chapter chapter = ObjectiveUtility.Chapter.None; //The value to return

        foreach (ObjectiveUtility.Chapter value in Enum.GetValues(typeof(ObjectiveUtility.Chapter))) //Go through all chapter enums
        {
            if (chapterParent == value.ToString())
                return value;
        }
        return chapter;
    }

    /// <summary>
    /// Reads all children of a given parent, and saves their tag.
    /// </summary>
    /// <param name="parent">The parent we want to read from</param>
    /// <returns></returns>
    private string[] ReadChildren(GameObject parent, ObjectiveUtility.Chapter chapterTag)
    {
        if (parent == null)
            Debug.LogError("Remember to activate the gameObjects (Chapter menus)");

        int childCount = parent.transform.childCount;
        string[] childTags = new string[childCount];

        for (int i = 0; i < childCount; i++)
        {
            childTags[i] = parent.transform.GetChild(i).GetComponent<Objective>().GetObjectiveName();
        }
        return childTags;
    }
}
