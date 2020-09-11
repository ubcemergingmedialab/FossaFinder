using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Kimberly Burke.
/// Determines what content to display on chapter 2 loading screen for tract origination narrative.
/// </summary>
public class ChapterTwoLoading : MonoBehaviour
{
    // [SerializeField] Text entryBlurb;
    // [SerializeField] Image entryImage;
    // [SerializeField] Sprite[] tractImages;

    [SerializeField] Text loadingText;
    private float loadDone;

    // Update is called once per frame
    void Update()
    {
        loadDone = LevelLoaderUtility.Async.progress;
        if (loadDone >= 0.9f)
        {
            loadingText.text = "Tap to Continue";
        }
    }

    public void ContinuetoScene()
    {
        LevelLoaderUtility.Async.allowSceneActivation = true;
    }

    /* public void SelectedTract(string tractName)
    {
        switch (tractName) {
            case "PCML_System_L":
                entryBlurb.text = "Robert enters the spinal cord from the spinal ganglion and pass diredtion to ipsilateral dorsal column. He follows the caudal fibers that enter fasciculus gracilis (medial) and the rostral fibers enter fasciulus cuneatus to ascend.";
                entryImage.GetComponent<Image>().sprite = tractImages[0];
                entryImage.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 500);
                break;
            case "PCML_System_R":
                entryBlurb.text = "Robert enters the spinal cord from the spinal ganglion and pass diredtion to ipsilateral dorsal column. He follows the caudal fibers that enter fasciculus gracilis (medial) and the rostral fibers enter fasciulus cuneatus to ascend.";
                entryImage.GetComponent<Image>().sprite = tractImages[1];
                entryImage.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 500);
                break;
            case "Spinothalamic_Tract_L":
                entryBlurb.text = "Robert enters the spinal cord from the spinal ganglion, travel up or down 1-2 segments inLissauer's tract and then synapse in the posterious horn. He also crosses the midline in the anterior white commissure and ascends as the anterolateral tract in the spinal cord.";
                entryImage.GetComponent<Image>().sprite = tractImages[2];
                entryImage.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 500);
                break;
            case "Spinothalamic_Tract_R":
                entryBlurb.text = "Robert enters the spinal cord from the spinal ganglion, travel up or down 1-2 segments inLissauer's tract and then synapse in the posterious horn. He also crosses the midline in the anterior white commissure and ascends as the anterolateral tract in the spinal cord.";
                entryImage.GetComponent<Image>().sprite = tractImages[3];
                entryImage.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 500);
                break;
            case "Lateral_Corticospinal_Tract_L":
                entryBlurb.text = "Robert starts in the motor cortex and he follows descending fibers that form the corona radiata, and converge to pass through the posterior limb of the internal capsule.";
                entryImage.GetComponent<Image>().sprite = tractImages[4];
                entryImage.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 800);
                break;
            case "Lateral_Corticospinal_Tract_R":
                entryBlurb.text = "Robert starts in the motor cortex and he follows descending fibers that form the corona radiata, and converge to pass through the posterior limb of the internal capsule.";
                entryImage.GetComponent<Image>().sprite = tractImages[5];
                entryImage.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 800);
                break;
            default:
                Debug.LogError("No tract selected");
                break;
        }
    } */
    
}
