using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Kimberly Burke.
/// Draws the reticle UI onto brain slices
/// </summary>
public class PlayerAimReticle : MonoBehaviour
{
    [SerializeField] GameObject reticle;
    private GameObject player;
    [SerializeField] private int startRadius;
    private float rayDist = 1000;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Draws the reticle in the game space when approaching a brain slice
    /// </summary>
    private void FixedUpdate()
    {
        Ray ray = new Ray(player.transform.position, transform.forward);
        if (!ObjectiveUtility.IsMotor)
            ray.origin = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 15);
        else
            ray.origin = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 15);

        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Slices");
        if (Physics.Raycast(ray, out hit, rayDist, mask))
        {
            // 13 index is Slices layer
            if (!ObjectiveUtility.IsMotor)
                reticle.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z - 20); // 20 units in front of brain slice
            else
                reticle.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z + 20); // 20 units in front of brain slice
            float percent = hit.distance / 1000f;
            if (percent < 0.45f) { percent = 0.45f;  }
            reticle.transform.localScale = new Vector3(startRadius * percent, startRadius * percent, 1);
            // Resize
        }
    }
}
