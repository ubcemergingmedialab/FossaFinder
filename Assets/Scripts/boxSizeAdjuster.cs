using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxSizeAdjuster : MonoBehaviour {

	public GameObject textObj;
	public textSizeAdjuster tsa;
	float prevScale;

	// Use this for initialization
	void Start () {
		tsa = textObj.GetComponent<textSizeAdjuster>();
		prevScale = 1;
	}

    public void ChangeText(float newSize)
    {
        if (prevScale != newSize)
        {
            print("adjusting scale");
            print(prevScale);
            transform.localScale = new Vector3(
                (float)transform.localScale.x * (float)newSize / (float)prevScale,
                transform.localScale.y, transform.localScale.z);
            prevScale = newSize;
            print(prevScale);
        }
    }

}
