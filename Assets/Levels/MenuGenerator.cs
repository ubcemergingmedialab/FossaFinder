using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Written by Oliver Vennike Riisager
/// Used to generate menu items from .txt files.
/// </summary>
public class MenuGenerator : MonoBehaviour
{
    [SerializeField]
    TextAsset levels; //Text asset

    [SerializeField]
    GameObject menuItem; //Our menu item
    Vector2 menuItemSize; //Sie of menu objects - determined by the prefab
    float padding = 20;

    //To be used for searchmethods
    List<Objective> activeMenuItems = new List<Objective>();
    List<Objective> inActiveMenuItems;

    string[] levelNames; //Array of levelnames

    private void Awake()
    {
        string levelText = levels.text; //Get the text from the connected document

        levelNames = Regex.Split(levelText, "\n|\r\n"); //Split on newlines

        menuItemSize = menuItem.GetComponent<RectTransform>().sizeDelta;
        for (int i = 0; i < levelNames.Length; i++) //Go through each line, ie. structure
        {
            var newMenuItem = Instantiate(menuItem, this.transform); //Instantiate it
            string currentName = levelNames[i]; //Holder for string
            levelNames[i] = currentName.Replace(' ', '_'); //We dont want whitespaces on codelevel - replace
            newMenuItem.name = levelNames[i]; //Rename object

            InitializeScripts(i, newMenuItem);
            SetPosition(i, newMenuItem, true);
            activeMenuItems.Add(newMenuItem.GetComponent<Objective>()); //Add to list of active objs
        }
        inActiveMenuItems = new List<Objective>(activeMenuItems.Count); //Add to list of inactive objs
        ModifySize();
        SetLevels();
    }

    /// <summary>
    /// Set up a gameobject with script and proper name
    /// </summary>
    /// <param name="index">The index in the levels collection</param>
    /// <param name="newMenuItem">The item</param>
    private void InitializeScripts(int index, GameObject newMenuItem)
    {
        var newObjective = newMenuItem.gameObject.AddComponent<Objective>(); //Add objective script
        newObjective.ObjectiveName = levelNames[index]; //Set the objective name
    }

    /// <summary>
    /// Fixes the position corresponding with index
    /// </summary>
    /// <param name="i">The index</param>
    /// <param name="newMenuItem">The item</param>
    /// <param name="vertical">Vertical or horizontal alignment</param>
    private void SetPosition(int i, GameObject newMenuItem, bool vertical)
    {
        var menuitemTrans = newMenuItem.transform; //get the transform

        if (vertical)
        {
            menuitemTrans.localPosition = new Vector3(menuitemTrans.localPosition.x, menuitemTrans.localPosition.y - (menuItemSize.y + padding) * i); //Set the local position
        }
        else
        {
            menuitemTrans.localPosition = new Vector3(menuitemTrans.localPosition.x - menuItemSize.x * i, menuitemTrans.localPosition.y); //Set the local pos
        }
    }

    /// <summary>
    /// Used to modify the size of the scrollrectangle our menuitems live in
    /// </summary>
    private void ModifySize()
    {
        var sizeOfScrollRect = GetComponent<RectTransform>().sizeDelta;
        GetComponent<RectTransform>().sizeDelta = new Vector2(sizeOfScrollRect.x, (menuItemSize.y + padding) * levelNames.Length);
    }

    /// <summary>
    /// Find the chapterparent and passed all generated levels into the objectiveUtility script
    /// </summary>
    private void SetLevels()
    {
        foreach (ObjectiveUtility.Difficulty difficulty in Enum.GetValues(typeof(ObjectiveUtility.Difficulty))) // Go through each difficulty
        {
            if (difficulty.ToString() != tag)
                continue;
            ObjectiveUtility.Chapter ChapterTag = FindChapterParent(); //Get the chapter for those structures
            ObjectiveUtility.InitializeChapter(difficulty, levelNames, ChapterTag); // initialize the chapter
        }
    }

    /// <summary>
    /// Finds the wrapping parent that is the "chapter"
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    private ObjectiveUtility.Chapter FindChapterParent()
    {
        string chapterParent = transform.parent.parent.tag; //We have to go through two layers with current menus
        ObjectiveUtility.Chapter chapter = ObjectiveUtility.Chapter.None; //The value to return

        foreach (ObjectiveUtility.Chapter value in Enum.GetValues(typeof(ObjectiveUtility.Chapter))) //Go through all chapter enums
        {
            if (chapterParent == value.ToString())
                return value;
        }
        return chapter;
    }
}
