using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedTourTestControl : MonoBehaviour
{
    public GuidedTourManager manager;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            manager.VisitOrSkipNextScene();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            manager.VisitOrSkipPreviousScene();
        }
        if (Input.GetKeyDown("2"))
        {
            manager.ToggleSceneDataInfo();
        }
    }
}
