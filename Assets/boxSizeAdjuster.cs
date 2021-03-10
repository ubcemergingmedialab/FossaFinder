using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxSizeAdjuster : MonoBehaviour {

	public GameObject textObj;
	public textSizeAdjuster tsa;
	float scaleSize;
	float prevScale;

	// Use this for initialization
	void Start () {
		tsa = textObj.GetComponent<textSizeAdjuster>();
		prevScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		scaleSize = tsa.verticalScaleSize;
		if (prevScale != scaleSize)
        {
			print("adjusting scale");
			print(prevScale);
			transform.localScale = new Vector3(
				(float)transform.localScale.x * (float) scaleSize / (float) prevScale,
				transform.localScale.y, transform.localScale.z);
			prevScale = scaleSize;
			print(prevScale);
		}
	}

}
