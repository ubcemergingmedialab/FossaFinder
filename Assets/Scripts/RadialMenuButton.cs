using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonState
{
    Default,
    Selected,
    Disabled
}

public class RadialMenuButton : MonoBehaviour {
    public ButtonState CurrentState { get; set; }
    public Sprite defaultSprite, selectedSprite, disabledSprite;

    public void SwitchToDefaultSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }

    public void SwitchToSelectedSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = selectedSprite;

    }

    public void SwitchToDisabledSprite()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = disabledSprite;
    }
}
