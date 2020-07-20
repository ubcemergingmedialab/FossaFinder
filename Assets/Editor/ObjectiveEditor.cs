using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor.SceneManagement;

/// <summary>
/// Written by Oliver Vennike Riisager - Editor script for objectives
/// </summary>
[CustomEditor(typeof(Objective))]
[CanEditMultipleObjects]
public class ObjectiveEditor : Editor
{
    public enum Chapter { Chapter1, Chapter2 };
    public Chapter currentChapter;
    public enum LevelTypes { Easy, Medium, Hard, Tracts };
    private LevelTypes currentlySelected; //The leveltype

    const string tractPath = "Assets/Levels/C2Levels.txt";
    public static string[] tractLevels;
    const string easyC1Levels = "Assets/Levels/C1Easy.txt";
    public static string[] easyLevels;
    const string medC1Levels = "Assets/Levels/C1Medium.txt";
    public static string[] mediumLevels;
    const string hardC1Levels = "Assets/Levels/C1Hard.txt";
    public static string[] hardLevels;

    public static bool isSet = false; //Used to check wether or not we need to load in text files.
    private int currentIndex; //The currentindex of a level choice
    SerializedProperty objectiveAttribute; //The current objectiveAttribute that we are working with

    public override void OnInspectorGUI()
    {
        if (!isSet)
            SetLevels();
        serializedObject.Update();
        if (EditorSceneManager.GetActiveScene().name.Contains("Menus"))
        {
            EditorGUILayout.LabelField("Objective : ", serializedObject.FindProperty("objectiveAttribute").FindPropertyRelative("objectiveName").stringValue); //If in menus, draw label.
            return;
        }
        
        objectiveAttribute = serializedObject.FindProperty("objectiveAttribute");
        //Get the current leveltype for the picked objective
        try
        {
            currentlySelected = (LevelTypes)serializedObject.FindProperty("objectiveAttribute").FindPropertyRelative("difficultyIndex").intValue; //Find the currently selected chapter
        }
        catch (Exception e)//If it doesnt exist as an enum
        {
            currentlySelected = LevelTypes.Easy;
        }
        LevelTypes newSelected = (LevelTypes)EditorGUILayout.EnumPopup("LevelType : ", currentlySelected);  //What chapter do we want to create levels for

        if (currentlySelected != newSelected) //If changed
        {
            serializedObject.FindProperty("objectiveAttribute").FindPropertyRelative("difficultyIndex").intValue = (int)newSelected; //Change it to new
            currentlySelected = newSelected; //Update current
        }

        switch (currentlySelected)
        {
            case LevelTypes.Easy:
                DrawLevels(easyLevels);
                currentChapter = Chapter.Chapter1;
                break;
            case LevelTypes.Medium:
                DrawLevels(mediumLevels);
                currentChapter = Chapter.Chapter1;
                break;
            case LevelTypes.Hard:
                DrawLevels(hardLevels);
                currentChapter = Chapter.Chapter1;
                break;
            case LevelTypes.Tracts:
                DrawLevels(tractLevels);
                currentChapter = Chapter.Chapter2;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Draws levels using the objectiveAttributes saved index
    /// </summary>
    /// <param name="levels">The levels to draw from</param>
    private void DrawLevels(string[] levels)
    {
        currentIndex = objectiveAttribute.FindPropertyRelative("typeIndex").intValue; //Get the index
        int newIndex = EditorGUILayout.Popup(currentIndex, levels); //Set the popup init value and grab if new
        
        if (currentIndex != newIndex) //Did it change ?
        {
            objectiveAttribute.FindPropertyRelative("typeIndex").intValue = newIndex; //Update index
            currentIndex = newIndex;
        }

        try //OOB catch - logs which obj we are failing on
        {
            objectiveAttribute.FindPropertyRelative("objectiveName").stringValue = levels[currentIndex];
        }
        catch (IndexOutOfRangeException e)
        {
            objectiveAttribute.FindPropertyRelative("objectiveName").stringValue = levels[0];
            Debug.Log("Name of object " + serializedObject.context.name);
        }
        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Reads all levels and sets isSet to true
    /// </summary>
    public static void SetLevels()
    {
        tractLevels = ReadLevels(tractPath);
        easyLevels = ReadLevels(easyC1Levels);
        mediumLevels = ReadLevels(medC1Levels);
        hardLevels = ReadLevels(hardC1Levels);
        isSet = true;
    }

    /// <summary>
    /// Reads levels using regex and streamreader
    /// </summary>
    /// <param name="path">Where is the txt file</param>
    /// <returns></returns>
    private static string[] ReadLevels(string path)
    {
        StreamReader sr = new StreamReader(path);
        string levelText = sr.ReadToEnd();

        return Regex.Split(levelText, "\n|\r\n");
    }
}
