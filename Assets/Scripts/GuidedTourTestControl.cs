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
            if (manager.GetIsDuringTransition())
            {
                if (manager.GetCurrentTransitionType() == TransitionType.Forward)
                {
                    manager.SkipTransition();
                }
            }
            else
            {
                manager.VisitNextScene();
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (manager.GetIsDuringTransition())
            {
                if (manager.GetCurrentTransitionType() == TransitionType.Backward)
                {
                    manager.SkipTransition();
                }
            }
            else
            {
                manager.VisitPreviousScene();
            }
        }
        if (Input.GetKeyDown("2"))
        {
            manager.showSceneDataInfo = !manager.showSceneDataInfo;
        }

    }
}
