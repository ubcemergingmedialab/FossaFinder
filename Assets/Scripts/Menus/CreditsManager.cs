using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Created by Kimberly Burke.
/// Controls the scrolling of the credits, the touch inputs to scroll through the credits and where to send the player at the end of the credits.
/// </summary>
public class CreditsManager : MonoBehaviour
{
    [SerializeField] RectTransform scrollPanel;
    [SerializeField] float speed;
    private Vector2 newPos;
    private Vector2 pointerInit;

    private void OnEnable()
    {
        //newPos = new Vector2(scrollPanel.position.x, 0);
        newPos = Vector2.zero;
        scrollPanel.localPosition = newPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            newPos.y = newPos.y + speed * Time.deltaTime;
            scrollPanel.localPosition = newPos;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            pointerInit = Input.mousePosition;
        }
        // control scrolling through credits
        if (Input.GetMouseButton(0))
        {
            Vector2 pointerNew = Input.mousePosition;
            float diff = pointerNew.y - pointerInit.y;
            var diffVec = new Vector2(0, diff);
            if (Mathf.Abs(diff) < 5)
                return;
            else
                newPos += diffVec.normalized * speed* 1.5f * Time.deltaTime;
            //newPos.y = newPos.y + (7 * Mathf.Sign(diff)); // TODO - improve scrolling
            scrollPanel.localPosition = newPos;
        }
    }
}
