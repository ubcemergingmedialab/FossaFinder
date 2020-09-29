using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If player hits the target when flying through the brain slice
/// </summary>
public class TargetHitEvent : MonoBehaviour
{
    [SerializeField] TutorialEvents tutorial;
    [SerializeField] TutorialUI uiManager;
    [SerializeField] BrainHitEvent brain;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !brain.GetTransparent())
        {  
            uiManager.PlayPositiveFeedback();
            tutorial.RespawnPlayer();
            tutorial.NextStep();
        }
    }

    /// <summary>
    /// Called by the hint button
    /// </summary>
    public void AcitvateHint()
    {
        Debug.Log("Hint Activate");
        transform.GetComponent<Renderer>().enabled = true;
    }
}
