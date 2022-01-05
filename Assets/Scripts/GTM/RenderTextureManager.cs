﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureManager : MonoBehaviour {
    public List<GameObject> availableRenderTextures;

    public void OnEnable()
    {
        GuidedTourManager.SetRenderTexture += EnableRenderTexture;
        GuidedTourManager.DisableRenderTexture += DisableRenderTexture;
        GuidedTourManager.InitializeEvent += DisableRenderTexture;
    }

    public void OnDisable()
    {
        GuidedTourManager.SetRenderTexture -= EnableRenderTexture;
        GuidedTourManager.DisableRenderTexture -= DisableRenderTexture;
        GuidedTourManager.InitializeEvent -= DisableRenderTexture;
    }

    private void EnableRenderTexture(string name)
    {
        foreach (GameObject availableRenderTexture in availableRenderTextures)
        {
            availableRenderTexture.SetActive(false);
        }

        foreach (GameObject availableRenderTexture in availableRenderTextures)
        {
            if (availableRenderTexture.name == name)
            {
                availableRenderTexture.SetActive(true);
                //Debug.Log("LIGHT: enabling " + availableRenderTexture.name);
            }
        }
    }

    private void DisableRenderTexture()
    {
        foreach (GameObject availableRenderTexture in availableRenderTextures)
        {
            availableRenderTexture.SetActive(false);
        }
    }
}
