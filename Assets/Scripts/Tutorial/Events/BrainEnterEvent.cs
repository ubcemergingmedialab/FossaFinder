using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Triggers the next step in the tutorial - Targeting on a brain slice
/// </summary>
public class BrainEnterEvent : MonoBehaviour
{
    [SerializeField] TutorialEvents tutorial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorial.TransitionToTarget();
        }
    }
}
