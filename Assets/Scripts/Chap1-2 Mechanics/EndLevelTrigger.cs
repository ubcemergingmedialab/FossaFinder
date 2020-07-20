using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Created by Niccolo Pucci.
/// A trigger that fires a Level End Event to the Level Manager, once it comes in contact with the player.
/// </summary>
public class EndLevelTrigger : MonoBehaviour
{
    public static UnityEvent endLevelEvent = new UnityEvent();

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
            endLevelEvent.Invoke();
    }
}
