using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLToolTipButton : MonoBehaviour
{
    [Header("Tooltip Game Object")]
    public GameObject toolTipDisplayGo;

    // Use this for initialization
    void Start()
    {
        //Disable the tool tip on start
        toolTipDisplayGo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClick()
    {
        toolTipDisplayGo.SetActive(!toolTipDisplayGo.activeSelf);
    }
}
