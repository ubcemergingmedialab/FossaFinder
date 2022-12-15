using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLNavigationButton : MonoBehaviour
{
    private GuidedTourManager manager;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.FindObjectOfType<GuidedTourManager>();
        if (manager == null)
        {
            Debug.LogError ("We cannot find the GuidedTourManager!");
        }
    }

    public void VisitOrSkipNextScene()
    {
        manager.VisitOrSkipNextScene();
    }

    public void VisitOrSkipPreviousScene()
    {
        manager.VisitOrSkipPreviousScene();
    }
}
