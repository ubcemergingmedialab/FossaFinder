using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written by Oliver Vennike Riisager
/// Used to sent objectives to the objectiveUtility
/// </summary>
public class ObjectiveMessenger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objectives = GameObject.FindGameObjectsWithTag("Objective"); //Find all Objectives in scene
        List<GameObject> wantedObjectives = new List<GameObject>(); //Prepare collection of wanted objectives. Ie objectives that match the selected level
        for (int i = 0; i < objectives.Length; i++)
        {
            Objective currentObjective = objectives[i].GetComponent<Objective>(); //Get  the objective component
            if (currentObjective.GetObjectiveName() == ObjectiveUtility.CurrentObjective) //Does it match ?
                wantedObjectives.Add(objectives[i]);//Add
        }
        ObjectiveUtility.SetObjectives(wantedObjectives.ToArray()); //Set the objective Utilities collection of objectives to the current levels
        ObjectiveUtility.SetSlices(); //Set the slices.
    }
}
