using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textSizeAdjuster : MonoBehaviour {

	public float verticalScaleSize;
	public string gameobjName;
	public float prevScale;

	// Use this for initialization
	void Start () {
		verticalScaleSize = 1;
		prevScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		gameobjName = gameObject.GetComponent<TextMesh>().text;
		verticalScaleSize = (float) gameobjName.Length / (float) 11;

		if (prevScale != verticalScaleSize)
		{
			transform.localScale = new Vector3(
				transform.localScale.x / verticalScaleSize * prevScale,
				transform.localScale.y,
				transform.localScale.z);
			prevScale = verticalScaleSize;
		}
	}
}