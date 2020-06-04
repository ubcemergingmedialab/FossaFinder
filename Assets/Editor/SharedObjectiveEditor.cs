using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// Written by Oliver Vennike Riisager - 
/// Editor script for SharedObjectives.
/// </summary>
[CustomEditor(typeof(SharedObjective))]
[CanEditMultipleObjects]
public class SharedObjectiveEditor : Editor
{
    private ObjectiveEditor.Chapter currentlySelected; //The currently selected chapter
    private int newSize; //The new size of the sharedObjectives collection
    private int currentSize; //The current size of the sharedObjectives collection
    
    private string[] allC1Levels; //Array of all chapter 1 levels
    private int index; //Used to fill out allC1Levels

    private int currentIndex; //The index of our current selection in the structure select
    private bool isSet; //Are all the level colection

    public override void OnInspectorGUI()
    {
        if (!ObjectiveEditor.isSet)//Is everything loaded ?
            ObjectiveEditor.SetLevels();
        if (!isSet) //Have we filled out our list of objectives ?
        {
            Init();
        }
        
        serializedObject.Update();
        currentlySelected = (ObjectiveEditor.Chapter)serializedObject.FindProperty("chapterEnum").intValue; //Find the currently selected chapter
        ObjectiveEditor.Chapter newSelected = (ObjectiveEditor.Chapter)EditorGUILayout.EnumPopup("Chapter : ", currentlySelected);  //What chapter do we want to create levels for
        if(currentlySelected != newSelected) //If changed
        {
            serializedObject.FindProperty("chapterEnum").intValue = (int)newSelected; //Change it to new
            currentlySelected = newSelected; //Update current
        }

        int currentSize = serializedObject.FindProperty("objectiveAttributes").arraySize; //What is the current size of the objectives in this object ?

        newSize = EditorGUILayout.IntField(currentSize); //The size of the collection
        if (newSize != currentSize) //Has it changed from the current ?
        {
            while (newSize > currentSize)
            {
                serializedObject.FindProperty("objectiveAttributes").InsertArrayElementAtIndex(currentSize); //Keep adding at our currentSize as index "append"
                currentSize = serializedObject.FindProperty("objectiveAttributes").arraySize;//Update the current size
            }
            while (newSize < currentSize)
            {
                serializedObject.FindProperty("objectiveAttributes").DeleteArrayElementAtIndex(currentSize - 1);//Keep removing at our currentSize -1
                currentSize = serializedObject.FindProperty("objectiveAttributes").arraySize; //Update the current size
            }
        }

        switch (currentlySelected) //What is the current chapter ?
        {
            case ObjectiveEditor.Chapter.Chapter1:
                for (int i = 0; i < currentSize; i++)
                {
                    DrawLevels(allC1Levels, i); //Draw all levels
                }
                break;
            case ObjectiveEditor.Chapter.Chapter2:
                for (int i = 0; i < currentSize; i++)
                {
                    DrawLevels(ObjectiveEditor.tractLevels,i); //Draw tracts only
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Used to fill the allC1Levels collection
    /// </summary>
    private void Init()
    {
        int c1LevelLenght = ObjectiveEditor.easyLevels.Length + ObjectiveEditor.mediumLevels.Length + ObjectiveEditor.hardLevels.Length;
        allC1Levels = new string[c1LevelLenght];
        index = AddLevels(ObjectiveEditor.easyLevels, index);
        index = AddLevels(ObjectiveEditor.mediumLevels, index);
        index = AddLevels(ObjectiveEditor.hardLevels, index);
        isSet = true;
    }

    /// <summary>
    /// Adds levels to the allC1Levels collection
    /// </summary>
    /// <param name="levels">The range we want to add</param>
    /// <param name="index">The index from which to add</param>
    /// <returns>The new index</returns>
    private int AddLevels(string[] levels, int index)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            allC1Levels[index] = levels[i];
            index++;
        }
        return index;
    }

    /// <summary>
    /// Draws each level in a dropdown menu
    /// </summary>
    /// <param name="levels">The range we want to draw</param>
    /// <param name="objectiveIndex">The index we want to draw</param>
    private void DrawLevels(string[] levels, int objectiveIndex)
    {
        SerializedProperty currentObjective = serializedObject.FindProperty("objectiveAttributes").GetArrayElementAtIndex(objectiveIndex); //Find the correct objective
        currentIndex = currentObjective.FindPropertyRelative("typeIndex").intValue; //Get the current type as index
        int newIndex = EditorGUILayout.Popup(currentIndex, levels); //Make popup use that

        if (currentIndex != newIndex) //IF a new one is selected
        {
            currentObjective.FindPropertyRelative("typeIndex").intValue = newIndex; //Set the new one
            currentIndex = newIndex; //Update current
        }
        currentObjective.FindPropertyRelative("objectiveName").stringValue = levels[currentIndex]; //Update the objective name
        serializedObject.ApplyModifiedProperties(); //Save
    }
}