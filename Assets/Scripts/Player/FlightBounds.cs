using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Determines wether or not the we are out of bounds. Adds a force towards center if we are.
/// </summary>
public class FlightBounds : MonoBehaviour
{
    [SerializeField]
    float centerPull;

    Vector2 center;
    private Rigidbody playerBody;
    Vector3 dirToCenter;
    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponentInChildren<Rigidbody>();
        center = new Vector3(playerBody.position.x, playerBody.position.y);
    }

    private void FixedUpdate()
    {
        Vector2 currentPos = new Vector2(playerBody.position.x, playerBody.position.y); //Save the current position as Vector2, we only care about x and y
        if (Vector3.Distance(currentPos, center) > LevelManager.maxRange) //Are we out of bounds ?
        {
            dirToCenter = (center - currentPos).normalized; //Find dir to center

            playerBody.AddForce(dirToCenter * centerPull * playerBody.velocity.magnitude, ForceMode.Impulse); //Add corresponding force
        }
    }
}
