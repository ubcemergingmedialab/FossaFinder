using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke.
/// Controls the touch input to progress through cutscene and where to send player after ending the cutscene.
/// </summary>
public class CutsceneManager : MonoBehaviour
{
    [SerializeField] Sprite[] cutscenes;
    [SerializeField] Image display;
    [SerializeField] CanvasManager canvasManager;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (index >= cutscenes.Length)
            {
                if (GameSaveUtility.GetID() <= 0 || GameSaveUtility.GetFirstTimePlayer())
                    canvasManager.SwitchCanvas("input");
                else
                    canvasManager.SwitchCanvas("menus");
            } else
            {
                index++;
                if (index != cutscenes.Length)
                    display.sprite = cutscenes[index];
            }
        }
    }
}
