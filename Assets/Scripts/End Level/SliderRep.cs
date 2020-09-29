using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderRep : MonoBehaviour
{
    //move slider
    private Vector3 end;
    private Vector3 start;
    private float distance;
    private Vector3 currentPos;
    private Vector3 wantedPosition;

    //Zoom
    private Transform zoomSlider;
    private float maxZoom;
    private float zoomPercent;

    //Damping for movement
    private float damping = 10;

    // Start is called before the first frame update
    void Start()
    {
        Prep(ObjectiveUtility.IsMotor);
    }

    private void Prep(bool isMotor)
    {
        //Set up movement slider
        end = transform.position;
        start = Vector3.zero;
        distance = Vector3.Distance(start, end);

        //Set up zoomSlider and maxZoom
        zoomSlider = transform.GetChild(0);
        maxZoom = Vector3.Distance(zoomSlider.position, transform.position);


        if (!isMotor)
        {
            transform.position = start;
            zoomSlider.position = start;
        }
        else
        {
            transform.position = end;
            zoomSlider.position = end;
            wantedPosition = end;
        }
    }

    public void ChangePos(float percent)
    {
        if (!ObjectiveUtility.IsMotor)
            wantedPosition = Vector3.Lerp(start, end, percent); //Find the new position we want to move to
        else
        {
            float invertedPerc = 1f - percent;
            wantedPosition = Vector3.Lerp(start, end, invertedPerc); //Find the new position we want to move to
        }
    }

    public void ChangeZoom(float percent)
    {
        zoomPercent = percent; //Save the zoom percent
    }

    private void Update()
    {
        ChangeSlide();
        ChangeZoom();
    }

    /// <summary>
    /// Change move the slider representation in game
    /// </summary>
    private void ChangeSlide()
    {
        currentPos = transform.position; //Get our current position
        transform.position = Vector3.Lerp(currentPos, wantedPosition, Time.deltaTime * damping); //Lerp between current and wanted, using delta time and damping as t
    }

    /// <summary>
    /// Change the zoom depending on what the last saved percent was
    /// </summary>
    private void ChangeZoom()
    {
        Vector3 wantedZoom = transform.position + transform.forward * maxZoom * zoomPercent;

        if (ObjectiveUtility.IsMotor) //Override if motorlevel
            wantedZoom = transform.position - transform.forward * maxZoom * zoomPercent;
        zoomSlider.position = Vector3.Lerp(zoomSlider.position, wantedZoom, Time.deltaTime * damping); //Fix the position of the zoom representation by lerping thats position and the wanted
    }
}
