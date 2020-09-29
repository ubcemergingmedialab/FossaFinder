using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Written by Oliver Vennike Riisager
/// Holder for all editor related values, and objectivenames.
/// </summary>
[Serializable]
public class ObjectiveAttribute
{
    [SerializeField]
    public string objectiveName;

    [SerializeField]
    public int typeIndex;

    [SerializeField]
    public int difficultyIndex;

    public ObjectiveAttribute(string objectiveName)
    {
        this.objectiveName = objectiveName;
    }
}
/// <summary>
/// Created by Oliver Vennike Riisager.
/// Used to change colour of hit objects and what was hit
/// Used to highlight objectives
/// </summary> 
public class Objective : MonoBehaviour
{
    // CORRECT => GREEN
    Color correctColor = new Color(0, 255, 0, 135);
    Color hintColor = new Color(0, 255, 0, 135);

    // MISSED => RED
    Color missedColor = new Color(255, 0, 0, 135);
    [SerializeField]
    public ObjectiveAttribute objectiveAttribute;
    public string ObjectiveName
    {
        get
        {
            return objectiveAttribute.objectiveName;
        }
        set
        {
            objectiveAttribute = new ObjectiveAttribute(value);
        }
    }


    public static GameObject[] HightLightControllers { get; private set; }
    public bool HintActive { get; private set; }
    private Color origColor;

    private void Start()
    {
        origColor = new Color(0,0,0,0);
    }

    public void Highlight()
    {
        HintActive = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().material.color = correctColor;
        }
    }

    public virtual void Correct()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().material.color = correctColor;
        }
    }

    public virtual void Missed()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().material.color = missedColor;
        }
    }

    public virtual void RemoveHighlight()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().material.color = origColor;
        }
    }

    public virtual string GetObjectiveName()
    {
        string toReturn = ObjectiveName.Replace(' ', '_');
        return toReturn;
    }
}
