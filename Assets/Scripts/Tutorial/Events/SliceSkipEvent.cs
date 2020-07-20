using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class SliceSkipEvent : MonoBehaviour
{
    [SerializeField] TutorialEvents tutorial;
    [SerializeField] TutorialUI uiManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tutorial.GetMode() != TutorialMode.Skip)
            {
                uiManager.PlayNegativeFeedback();
            }
            else
            {
                // When tutorial mode is on the skip step and the slice is transparent, then trigger the next step of the tutorial (positive feedback called by brain hit event)
                tutorial.NextStep();
            }
            tutorial.RespawnPlayer();
        }
    }
}
