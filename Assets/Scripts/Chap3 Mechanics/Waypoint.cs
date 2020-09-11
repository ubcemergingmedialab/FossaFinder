using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Cindy Shi.
/// buggy's patrol point
/// </summary>
/// 
public class Waypoint : MonoBehaviour
{

    [SerializeField] protected float debugDrawRadius = 1.0f;

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);
    }
    
}
