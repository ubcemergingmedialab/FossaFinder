using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

/// <summary>
/// Written By Oliver Vennike Riisager. Draws and check for input on joystick
/// </summary>
public class JoystickControl : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image backgroundImage;
    public Vector3 BackgroundSizeDelta { get { return backgroundImage.rectTransform.sizeDelta; }}
    private Image joystickImage;
    private Vector3 inputVector;

    private void Start()
    {
        backgroundImage = GetComponent<Image>(); //Get background of joystick
        joystickImage = transform.GetChild(1).GetComponent<Image>(); //Get joystick thumb
    }

    public virtual void OnDrag(PointerEventData pd)
    {
        //Where do we click in relation to the background image?
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(backgroundImage.rectTransform, pd.position, pd.pressEventCamera, out position))
        {
            inputVector = CalculateInputVector(position);
            UpdateJoystickPosition(inputVector);
        }
    }

    /// <summary>
    /// Calculates the inputvector in relation to its backgroundimage
    /// </summary>
    /// <param name="position"></param>
    /// <returns>The inputvector</returns>
    private Vector3 CalculateInputVector(Vector2 position)
    {
        position.x = (position.x / backgroundImage.rectTransform.sizeDelta.x); //Get a 0-1 value of our clicks position
        position.y = (position.y / backgroundImage.rectTransform.sizeDelta.y);

        inputVector = new Vector3(position.x * 2 + 1, 0, position.y * 2 - 1); //Make sure we get a zero.zero position when we click the center of the joystick 
        return (inputVector.magnitude > 1) ? inputVector.normalized : inputVector; //Normalize if bigger than one, else stay the same
    }

    /// <summary>
    /// Updates the joysticks position;
    /// </summary>
    /// <param name="inputVector"></param>
    private void UpdateJoystickPosition(Vector3 inputVector)
    {
        joystickImage.rectTransform.anchoredPosition = new Vector3(inputVector.x * (BackgroundSizeDelta.x / 3), inputVector.z * (BackgroundSizeDelta.y / 3));
    }

    /// <summary>
    /// Used for mouseinput
    /// </summary>
    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped); //Ondrag is called more often and will do the same
    }

    /// <summary>
    /// Reset values
    /// </summary>
    /// <param name="peu"></param>
    public virtual void OnPointerUp(PointerEventData peu)
    {
        inputVector = Vector3.zero;
        joystickImage.rectTransform.anchoredPosition = Vector3.zero; //Reset to 0,0
    }

    /// <summary>
    /// Used to get the movement in horizontally
    /// </summary>
    /// <returns> The horizontal direction</returns>
    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;//Return it if it isnt zero
        else
        {
            return Input.GetAxis("Horizontal"); //Else return the horizontal axis
        }
    }

    /// <summary>
    /// Used to get the movement in vertically
    /// </summary>
    /// <returns> The vertical direction</returns>
    public float Vertical()
    {
        if (inputVector.z != 0)
            return inputVector.z;//Return it if it isnt zero
        else
        {
            return Input.GetAxis("Vertical"); //Else return the horizontal axis
        }
    }
}
