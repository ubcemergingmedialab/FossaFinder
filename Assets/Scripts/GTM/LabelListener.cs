using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelListener : MonoBehaviour {

    private GuidedTourManager gtm;

    private static LabelListener _instance;
    public static LabelListener Instance
    {
        get { return _instance; }
    }

    private boxSizeAdjuster boxSize;
    private textSizeAdjuster textSize;
    private TextMesh text;
    private Color defaultColor;
    private MeshRenderer boxRenderer;

    public GameObject textbox;

    private bool changed;

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
        if(text == null)
        {
            text = GetComponent<TextMesh>();
        }
        boxRenderer = textbox.GetComponent<MeshRenderer>();

        if (defaultColor == null)
        {
            defaultColor = boxRenderer.material.color;
        }

        changed = false;
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

    public void ChangeColor(Color newColor)
    {
        if (changed == false)
        {
            defaultColor = boxRenderer.material.color;
            newColor = defaultColor;
            newColor.a = 0.25f;
            changed = true;
        }
        else
        {
            newColor.a = 0.5f;
        }
        boxRenderer.material.color = newColor;
    }

    public void ToDefault()
    {
        ChangeColor(defaultColor);
        ChangeText("");
   }
}
