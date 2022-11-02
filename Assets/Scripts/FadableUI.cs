using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadableUI : MonoBehaviour
{

    public CanvasGroup cg;
    public float fadeSpeed = 1.25f;
    private float step = 0;
    private bool isFadeIn = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeIn)
        {
            if (step < 1)
            {
                step += Time.deltaTime * fadeSpeed;
            }
        }
        else
        {
            if (step > 0)
            {
                step -= Time.deltaTime * fadeSpeed;
            }
        }
        cg.alpha = Mathf.Lerp(0, 1, step);

    }

    public void FadeIn()
    {
        isFadeIn = true;
    }

    public void FadeOut()
    {
        isFadeIn = false;
    }
}
