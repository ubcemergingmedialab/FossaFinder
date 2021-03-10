using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelListener : MonoBehaviour {

    private static LabelListener _instance;
    public static LabelListener Instance
    {
        get { return _instance; }
    }

    private boxSizeAdjuster boxSize;
    private textSizeAdjuster textSize;
    private TextMesh text;

    // Use this for initialization
    void Start () {
		if (boxSize == null)
        {
            boxSize = GetComponentInParent<boxSizeAdjuster>();
        }
        if(textSize == null)
        {
            textSize = GetComponent<textSizeAdjuster>();
        }
        if( text == null)
        {
            text = GetComponent<TextMesh>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeText(string newText)
    {
        text.text = newText;
        float newSize = textSize.ChangeText(newText);
        boxSize.ChangeText(newSize);
    }
}
