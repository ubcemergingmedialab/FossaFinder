using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written by Oliver Vennike Riisager
/// </summary>
public class SharedObjective : Objective
{
    [SerializeField]
    List<ObjectiveAttribute> objectiveAttributes;

    [SerializeField]
    public int chapterEnum; //INdexing for editor

    private void Awake()
    {
        ObjectiveName = GetObjectiveName();
    }

    /// <summary>
    /// Gets the objective name
    /// </summary>
    /// <returns>The objective name if one matches the current level, else returns null</returns>
    public override string GetObjectiveName()
    {
        foreach (var objective in objectiveAttributes)
        {
            string underScoredObjective = objective.objectiveName.Replace(' ', '_');
            objective.objectiveName = underScoredObjective;
            if (objective.objectiveName == ObjectiveUtility.CurrentObjective)
                return objective.objectiveName;
        }
        return null;
    }
}
