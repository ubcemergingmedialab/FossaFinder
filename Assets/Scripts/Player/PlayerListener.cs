using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SliceControllerEvent : UnityEvent<SliceController> { }//SliceController Events
public class ObjectiveEvent : UnityEvent<Objective> { }//Objective Events

/// <summary>
/// Written by Oliver Vennike Riisager
/// Lets the player "see" what he has hit and react accordingly.
/// </summary>
public class PlayerListener : MonoBehaviour
{
    public static SliceControllerEvent sliceHit = new SliceControllerEvent();
    private bool isSliceSkipped = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Slice")
        {
            CheckForSlice(other);
            return;
        }

        if (other.transform.parent != null && other.transform.parent.tag == "Objective") //TODO : Change for mesh colliders
            CheckForObjective(other);
    }

    /// <summary>
    /// Checks if we hit a slice.
    /// </summary>
    /// <param name="other">The hit</param>
    /// <param name="correct">Did we hit the correct objective on this slice ?=</param>
    private void CheckForSlice(Collider other)
    {
        var hitSlice = other.GetComponent<SliceController>();
        if (hitSlice.Skipped)
        {
            if (hitSlice.ActiveObjective != null)
            {
                SliceController.negativeFeedback.Invoke();
                ObjectiveUtility.Score--;
                if (ObjectiveUtility.Score < 0)
                    ObjectiveUtility.Score = 0;
            }
            else
            {
                if (ObjectiveUtility.CurrentGameMode != ObjectiveUtility.GameMode.Training && !hitSlice.HintActive)
                    ObjectiveUtility.Score++;
                SliceController.positiveFeedback.Invoke();
            }
        }
        else
        {
            ObjectiveUtility.Score--;
            if (ObjectiveUtility.Score < 0)
                ObjectiveUtility.Score = 0;
            SliceController.negativeFeedback.Invoke();
        }
    }

    /// <summary>
    /// Checks wether or not the hit object is the currentobjective
    /// </summary>
    private void CheckForObjective(Collider other)
    {
        Objective objective = other.transform.parent.GetComponent<Objective>();

        if (LevelManager.currentSlice.Skipped)
            return;


        if (ObjectiveUtility.CurrentObjective == objective.GetObjectiveName() && !objective.HintActive) //If we hit the correct objective
        {
            ObjectiveUtility.Score++;
            SliceController.positiveFeedback.Invoke();
        }
        else if (ObjectiveUtility.CurrentObjective == objective.GetObjectiveName() && objective.HintActive)
        {
            SliceController.positiveFeedback.Invoke();
        }
        else
        {
            ObjectiveUtility.Score--;
            if (ObjectiveUtility.Score < 0)
                ObjectiveUtility.Score = 0;
            SliceController.negativeFeedback.Invoke();
        }
    }
}
