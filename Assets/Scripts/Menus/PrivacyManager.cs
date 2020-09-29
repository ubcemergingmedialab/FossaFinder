using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke
/// Navigation between privacy policy and the previously open page
/// </summary>
public class PrivacyManager : MonoBehaviour
{
    public string prevPage { get; set; }


    [SerializeField] CanvasManager canvasManager;
    [SerializeField] GameObject contentPrivacy;
    [SerializeField] GameObject contentService;
    [SerializeField] GameObject container;

    public void OpenPrivacy()
    {
        contentPrivacy.SetActive(true);
        contentService.SetActive(false);
        container.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 2000);
    }

    public void OpenService()
    {
        contentPrivacy.SetActive(false);
        contentService.SetActive(true);
        container.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 5000);
    }

    public void BackPage()
    {
        canvasManager.SwitchCanvas(prevPage);
    }

    private void OnEnable()
    {
        // reset content to start at top
        contentPrivacy.GetComponent<RectTransform>().Translate(Vector3.zero);
        contentService.GetComponent<RectTransform>().Translate(Vector3.zero);
    }
}
