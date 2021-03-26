using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textSizeAdjuster : MonoBehaviour {

	public float horizontalScaleSize;
	public string gameobjName;
	public float prevScale;

	// Use this for initialization
	void Start () {
		horizontalScaleSize = 1;
		prevScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public float ChangeText(string newText)
    {
        if (newText == "")
        {
            horizontalScaleSize = 1;
        } else
        {
            horizontalScaleSize = (float)newText.Length / (float)11;
        }

        if (prevScale != horizontalScaleSize)
        {
            transform.localScale = new Vector3(
                transform.localScale.x / horizontalScaleSize * prevScale,
                transform.localScale.y,
                transform.localScale.z);
            prevScale = horizontalScaleSize;
        }

        return horizontalScaleSize;
    }
}