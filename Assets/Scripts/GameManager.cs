using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Niccolo Pucci.
/// Implementation responsible for managing gameplay and gameflow actions.
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    private Rigidbody rb;
    private bool started;

    void Start ()
    {
        // Start sequence where the player cannot move
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        started = false;
    }

    /// <summary>
    /// Call UI manager to hide all hint text and re-enables player movement
    /// </summary>
    public virtual void StartGame()
    {
        uiManager.ShowActivityStartScreen();
        rb.constraints = RigidbodyConstraints.None;
        started = true;
    }
}
