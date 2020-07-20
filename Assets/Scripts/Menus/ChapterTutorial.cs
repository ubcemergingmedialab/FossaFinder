using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterTutorial : MonoBehaviour
{
    [SerializeField] Sprite[] pages;
    [SerializeField] Image display;
    [SerializeField] GameObject[] gameplayObjects;
    [SerializeField] AudioManager audio;
    [SerializeField] string[] text;
    [SerializeField] Text textDisplay;

    [SerializeField] int chapter;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;

        if (GameSaveUtility.GetChapProgress(chapter))
        {
            // if chapter progression already completed, end tutorial
            EndTutorial();
        }
        else
        {
            foreach (GameObject obj in gameplayObjects)
            {
                obj.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (index >= pages.Length)
            {
                GameSaveUtility.SetChapProgress(chapter); // set chapter 1 progression as complete/true
                EndTutorial();
            }
            else
            {
                index++;
                if (index != pages.Length)
                {
                    display.sprite = pages[index];
                    textDisplay.text = text[index];
                }
            }
        }
    }

    private void EndTutorial()
    {
        foreach (GameObject obj in gameplayObjects)
        {
            obj.SetActive(true);
        }
        this.gameObject.SetActive(false);
        audio.StartPlayerSounds();
    }
}
