using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelListener : MonoBehaviour
{
    public string defaultLabelValue;

    private static LabelListener _instance;
    public static LabelListener Instance
    {
        get { return _instance; }
    }

    public Text textBox;
    private Color defaultColor;
    private Image bgImage;

    public GameObject textbox;

    private bool changed;

    // Use this for initialization
    void Start()
    {
        //background panel image
        bgImage = GetComponentInParent<Image>();
        if (defaultColor == null)
        {
            defaultColor = bgImage.color;
        }

        changed = false;
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
        textBox.text = newText;
    }

    public void ChangeColor(Color newColor)
    {
        if (changed == false)
        {
            defaultColor = bgImage.color;
            newColor = defaultColor;
            newColor.a = 0.25f;
            changed = true;
        }
        else
        {
            newColor.a = 0.5f;
        }

        bgImage.color = newColor;
    }

    public void ToDefault()
    {
        ChangeColor(defaultColor);
        ChangeText(defaultLabelValue);
    }
}
