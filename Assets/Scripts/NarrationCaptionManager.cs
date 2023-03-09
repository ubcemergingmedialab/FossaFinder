using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NarrationCaptionManager : MonoBehaviour
{
    //Engineering Notes for Captioning
    //Update SceneData scriptable object to take in an array of new scriptable objects called NarrationCaptionData
    //NarrationCaptionData will have two properties:
    //1 float - Timestamp of when to show the caption
    //2 String - the caption 

    //In GuidedTourManager, when we 'play' the scene we will trigger an event (located here)
    //this event will start a coroutine which will play the narration caption data, according
    //to the order and timing set out in the scriptable object

    //things to consider + test:  How to pause and resume coroutine properly
    //skipping scenes by going forward and backward

    public Text captionTextField;
    public Image backgroundImage;
    private IEnumerator displayCaptionCoroutine;

    //Timers
    private float currentTime = 0;

    public void OnEnable()
    {
        //Important: Audio narration only shows during 'visit next event' so it make sense
        //to show caption during this event.
        //GuidedTourManager.VisitPreviousEvent -= DisplayCaption;
        GuidedTourManager.VisitNextEvent += DisplayCaption;
        GuidedTourManager.NarrationEndEvent += DisplayEmptyCaption;
    }

    public void OnDisable()
    {
        //GuidedTourManager.VisitPreviousEvent -= DisplayCaption;
        GuidedTourManager.VisitNextEvent -= DisplayCaption;
        GuidedTourManager.NarrationEndEvent -= DisplayEmptyCaption;
    }

    private IEnumerator DisplayCaptionCoroutine(SceneData data)
    {
        int numOfCaptions = data.captionDataArray.Length;
        if (numOfCaptions != 0)
        {
            //short pause before the first speech
            yield return new WaitForSeconds(0.25f);
            backgroundImage.enabled = true;
            captionTextField.text = data.captionDataArray[0].captionString;
            for (int i = 1; i < data.captionDataArray.Length; i++)
            {
                yield return new WaitForSeconds(data.captionDataArray[i].timing);
                captionTextField.text = data.captionDataArray[i].captionString;
            }
        }
        yield return null;
    }

    private void DisplayCaption(SceneData data)
    {
        NarrationCaptionData[] array = data.captionDataArray;
        int size = array.Length;

        //stop the existing co-routine
        if (displayCaptionCoroutine != null)
        {
            StopCoroutine(displayCaptionCoroutine);
        }

        //start another
        displayCaptionCoroutine = DisplayCaptionCoroutine(data);
        StartCoroutine(displayCaptionCoroutine);
    }

    void Start()
    {
        backgroundImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DisplayEmptyCaption()
    {
        backgroundImage.enabled = false;
        SetString("");
    }

    private void SetString(string inputText)
    {
        captionTextField.text = inputText;
    }

    private void Pause()
    {
        //TODO
    }
}
