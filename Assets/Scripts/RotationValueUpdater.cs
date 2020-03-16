using UnityEngine;
using UnityEngine.UI;

public class RotationValueUpdater : MonoBehaviour {
    Text rotationValue;
    public GameObject rotationSlider;

    // Use this for initialization
    void Start () {
		rotationValue = gameObject.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
