using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulsingAlphaEffect : MonoBehaviour
{
    [Header("Button Script")]
    public Button button;

    [Header("Target Color")]
    public Color targetColor;

    [Header("Speed of the pulsing color")]
    public float pulseSpeed = 1.0f;

    private Color originalNormalColor;
    private bool isPulsing = true;

    // Use this for initialization
    void Start()
    {
        //save the default color
        originalNormalColor = button.colors.normalColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPulsing)
        {
            PulseColor();
        }
    }

    public void SetPulsingEffect(bool isPulsing)
    {
        this.isPulsing = isPulsing;
        if (!this.isPulsing)
        {
            //revert colors back to normal
            ColorBlock colorVars = button.colors;
            colorVars.normalColor = originalNormalColor;
            button.colors = colorVars;
        }
    }

    private void PulseColor()
    {
        //oscillates between the two colors, using Cos as the lerp value 
        Color newColor = Color.Lerp(originalNormalColor, targetColor, Mathf.Cos(Time.time * pulseSpeed));

        //assign new color
        ColorBlock colorVars = button.colors;
        colorVars.normalColor = newColor;
        button.colors = colorVars;
    }
}
