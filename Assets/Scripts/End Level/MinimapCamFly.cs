using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written by Oliver Vennike Riisager
/// Determines how the minimapcamera should move.
/// </summary>
public class MinimapCamFly : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    public static bool endGame = false;
    private float distance;
    private float prevPercent;

    [SerializeField]
    private Vector3 looktargetOrigPos;
    private Vector3 camOrigPos;
    private float percent;
    private Vector3 targetPos;
    private float damping = 10;

    // Start is called before the first frame update
    void Start()
    {
        transform.DetachChildren();
        Prep(ObjectiveUtility.IsMotor);
    }

    /// <summary>
    /// Sets up the camera
    /// </summary>
    private void Prep(bool isMotor)
    {
        distance = Vector3.Distance(transform.position, target.position); //Find the distance between us and the target
        MinimapCamUtility.Target = target; //Update the utility class
        MinimapCamUtility.LookTarget = target.GetChild(0); //Update the utility class
        looktargetOrigPos = MinimapCamUtility.LookTarget.position; //Save original position of the looktarget
        camOrigPos = transform.position; //Save our original position

        if (isMotor)
        {
            Vector3 flipLength = (target.position - transform.position).normalized * distance * 2; //Find distance to our target and multiply by 2
            camOrigPos = transform.position + flipLength; //Add to our current position
            transform.position = camOrigPos; //Set starting pos
        }

        targetPos = Vector3.Lerp(camOrigPos, target.position, percent); //Set initial target position
    }

    /// <summary>
    /// Moves the camera towards the target
    /// </summary>
    /// <param name="percent"></param>
    public void Zoom(float percent)
    {
        targetPos = Vector3.Lerp(camOrigPos, target.position, percent); //Where do we want to be ?
        this.percent = percent; //Saves the current zoom percent
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * damping); //Set our position in relation to the wanted target
        transform.position = new Vector3(transform.position.x, transform.position.y, MinimapCamUtility.Target.position.z); //Fix our z position
        transform.LookAt(MinimapCamUtility.LookTarget, transform.up); //Look at the correct target
    }
}
