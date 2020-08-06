using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Created by Wayland.
/// Transforms character location to specified target over time
/// </summary>
public class Teleport : MonoBehaviour
{
    public Transform[] target;
    public GameObject Player;
    private float t;
    public float timeToReachTarget;
    bool teleOn;
    Vector3 startPosition;


    private void Update()
    {
        //If the boolean of teleOn is on, then continue transforming the player to the next location in target[] until the last position.
        //You will notice for this to work with our system we have to disable and enable NavMeshAgent, otherwise it will not work
        if (teleOn)
        {
            for (int i = 0; i < target.Length; i++)
            {
                t += Time.deltaTime / timeToReachTarget;
                if (Player.transform.position != target[i].position)
                {
                    Player.transform.position = Vector3.Lerp(Player.transform.position, target[i].position, t);
                    Debug.Log(target[i]);
                }
                if (Player.transform.position == target[target.Length - 1].position)
                {
                    teleOn = false;
                    Player.GetComponent<NavMeshAgent>().enabled = true;
                }
            }
        }
    }
    //the trigger once the user collides into a teleporter area
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            teleOn = true;
            Player.GetComponent<NavMeshAgent>().enabled = false;
        }
    }



}