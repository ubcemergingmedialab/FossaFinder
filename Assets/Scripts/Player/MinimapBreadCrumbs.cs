using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written by Oliver Vennike Riisager
/// Used to draw the line behind the player.
/// </summary>
public class MinimapBreadCrumbs : MonoBehaviour
{
    [SerializeField]
    bool isTractLevel;

    [SerializeField]
    float crumbSpeed = 0.2f;
    LineRenderer tractRenderer;

    [SerializeField]
    Transform playerActualPosition;
    private int currentAmount;

    private const int LINESTEPS = 10;

    // Start is called before the first frame update
    void Start()
    {
        CrumbUtility.Start(crumbSpeed); //Start the timer
        tractRenderer = GetComponent<LineRenderer>(); //Get the lineRenderer
    }

    // Update is called once per frame
    void Update()
    {
        AddCrumbs();
        DrawCrumbsList();
    }

    private void AddCrumbs()
    {
        if (CrumbUtility.DecreaseTimer(Time.deltaTime) && GetComponentInChildren<Rigidbody>().constraints != RigidbodyConstraints.FreezeAll) //If the timer is lower than 0
        {
            CrumbUtility.Crumbs.Add(new Vector3(playerActualPosition.position.x, playerActualPosition.position.y, playerActualPosition.position.z)); //Adding a new Crumb
            tractRenderer.positionCount = CrumbUtility.Crumbs.Count; //Set the LineRenderers amount of points to be the amount of crumbs, plus the added midpoints.
        }
    }

    private void DrawCrumbsList()
    {
        if (CrumbUtility.Crumbs.Count > 2) //If the count is larger than 2, draw
        {
            tractRenderer.enabled = true; //Enable the renderer
            int i = 0;
            foreach (Vector3 crumb in CrumbUtility.Crumbs)
            {
                tractRenderer.SetPosition(i, crumb);
                i++;
            }
        }
        else
        {
            tractRenderer.enabled = false;
        }
    }

    private void BezierPoints(Vector3 from, Vector3 to)
    {
        float midX;
        float midY;
        if (from.x < 0 && to.x < 0)
        {
            if (from.x < to.x)
                midX = from.x;
            else
                midX = to.x;
        }
    }
}
