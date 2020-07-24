using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObjectOnZoom : MonoBehaviour {

    private GuidedTourManager tour;
    public GameObject transparent, opaque;

    private void Start()
    {
        if (transparent == null || opaque == null)
        {
            Debug.Log("[SwitchObjectOnZoom]: Skull references not set.");
            return;
        }

        SwitchToOpaque();
    }

    private void OnEnable()
    {
        GuidedTourManager.ZoomedOut += SwitchToTransparent;
        GuidedTourManager.DefaultState += SwitchToOpaque;
    }

    private void OnDisable()
    {
        GuidedTourManager.ZoomedOut -= SwitchToTransparent;
        GuidedTourManager.DefaultState -= SwitchToOpaque;
    }

    private void SwitchToTransparent()
    {
        if(transparent == null || opaque == null)
        {
            Debug.Log("[SwitchObjectOnZoom]: Skull references not set.");
            return;
        }

        transparent.SetActive(true);
        opaque.SetActive(false);
    }

    private void SwitchToOpaque()
    {
        if (transparent == null || opaque == null)
        {
            Debug.Log("[SwitchObjectOnZoom]: Skull references not set.");
            return;
        }

        transparent.SetActive(false);
        opaque.SetActive(true);
    }

}
