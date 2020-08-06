using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// Created by Kimberly Burke.
/// Tracks the events for the tutorial mode - affects UI and Player controls
/// </summary>
public class TutorialEvents : MonoBehaviour
{
    [SerializeField] Transform spawnPoints;
    [SerializeField] GameObject textPopup;
    [SerializeField] Text tutorialText;

    [SerializeField]
    TextAsset tutorialFile;

    [Header("Game Objects")]
    [SerializeField] GameObject slice;
    [SerializeField] GameObject target;
    [SerializeField] GameObject hintTarget;
    [SerializeField] GameObject brain;

    private string[] textPrompts;
    private TutorialUI uiManager;

    private Rigidbody rb;
    private GameObject player;
    private TutorialMode mode;

    private int textIndex = 0;
    private bool targetHit;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponentInChildren<Rigidbody>();
        DisableMovement();
        mode = TutorialMode.Intro;
        slice.SetActive(false);
        target.SetActive(false);
        hintTarget.SetActive(false);
        uiManager = GameObject.Find("Tutorial Gameplay Canvas").GetComponent<TutorialUI>();
    }

    /// <summary>
    /// Reads text file of tutorial prompts into a string List
    /// </summary>
    private void Awake()
    {
        string tutorialTextFile = tutorialFile.text;
        textPrompts = Regex.Split(tutorialTextFile, "\n|\r\n");
        GameSaveUtility.SetFirstTimePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (textIndex < textPrompts.Length) 
            tutorialText.text = textPrompts[textIndex].Replace("Robert", GameSaveUtility.GetPlayerName());

        switch (mode)
        {
            case TutorialMode.None:
                break;
            case TutorialMode.Intro:
                Introduction();
                if (textIndex >= 3)
                    TransitionToSteer();
                break;
            case TutorialMode.Steering:
                SteeringCheck();
                // transition called by BrainEnterEvent
                break;
            case TutorialMode.Target:
                TargetCheck();
                // transition called by TargetHitEvent in NextStep
                break;
            case TutorialMode.Boost:
                BoostCheck();
                // transition called by TargetHitEvent in NextStep
                break;
            case TutorialMode.Hidden:
                HiddenCheck();
                // transition called by TargetHitEvent in NextStep
                break;
            case TutorialMode.Skip:
                SkipCheck();
                // transition called by SliceSkipEvent in NextStep
                break;
            case TutorialMode.Unskip:
                UnskipCheck();
                // transition called by TargetHitEvent in NextStep
                break;
            case TutorialMode.Hint:
                HintCheck();
                // transition called by TargetHitEvent in NextStep
                break;
            case TutorialMode.Finish:
                Finish();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Called from external events to trigger the transition events to the next modes
    /// </summary>
    public void NextStep()
    {
        switch (mode)
        {
            case TutorialMode.None:
                // Do nothing
                break;
            case TutorialMode.Intro:
                // Do nothing
                break;
            case TutorialMode.Steering:
                // Do nothing
                break;
            case TutorialMode.Target:
                TransitionToBoost();
                break;
            case TutorialMode.Boost:
                TransitionToHidden();
                break;
            case TutorialMode.Hidden:
                TransitionToSkip();
                break;
            case TutorialMode.Skip:
                TransitionToUnskip();
                break;
            case TutorialMode.Unskip:
                TransitionToHint();
                break;
            case TutorialMode.Hint:
                DisableMovement();
                mode = TutorialMode.Finish; // transitions to Finish
                break;
            case TutorialMode.Finish:
                // Do nothing
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Progresses the text box through the introductions
    /// </summary>
    private void Introduction()
    {
        textPopup.SetActive(true);
        if (Input.GetMouseButtonDown(0))
            textIndex++;
    }

    /// <summary>
    /// Event to transition to steering. Turns on the joystick UI and allows player to move.
    /// </summary>
    private void TransitionToSteer()
    {
        uiManager.ActivateUI(0);
        EnableMovement();
        mode = TutorialMode.Steering;
    }

    /// <summary>
    /// Progresses the text box through the steering instructions and also starts the coroutine for Fly time
    /// </summary>
    private void SteeringCheck()
    {
        if (Input.GetMouseButtonDown(0) && textPopup.activeInHierarchy)
            textPopup.SetActive(false);
    }

    /// <summary>
    /// Called by the BrainEnterEvent to set up the target step of the tutorial
    /// </summary>
    public void TransitionToTarget()
    {
        // show UI
        textIndex++;
        textPopup.SetActive(true);
        
        // switch game objects
        brain.SetActive(false);
        slice.SetActive(true);

        // start player at respawn point
        RespawnPlayer();
        DisableMovement();

        mode = TutorialMode.Target;
    }
    
    /// <summary>
    /// Progresses through the textboxes in the target step of the tutorial
    /// </summary>
    private void TargetCheck()
    {
        if (Input.GetMouseButtonDown(0) && textPopup.activeInHierarchy)
        {
            textIndex++;
            if (textIndex == 6) {
                target.SetActive(true);
            } else if (textIndex == 7)
            {
                EnableMovement();
                textPopup.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Called by NextStep from the TargetHitEvent to set up the boost step of the tutorial
    /// </summary>
    private void TransitionToBoost()
    {
        // show UI
        textIndex++;
        textPopup.SetActive(true);
        uiManager.ActivateUI(1);

        // pause player movement
        DisableMovement();

        mode = TutorialMode.Boost;
    }

    /// <summary>
    ///  Progresses through the textboxes in the boost step of the tutorial
    /// </summary>
    private void BoostCheck()
    {
        if (Input.GetMouseButtonDown(0) && textPopup.activeInHierarchy)
        {
            textIndex++;
            if (textIndex == 10)
            {
                EnableMovement();
                textPopup.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Called by NextStep from the TargetHitEvent to set up the hiding highlight step of the tutorial
    /// </summary>
    private void TransitionToHidden()
    {
        // show UI
        textIndex++;
        textPopup.SetActive(true);

        // pause player movement
        DisableMovement();

        mode = TutorialMode.Hidden;
    }

    /// <summary>
    ///  Progresses through the textboxes in the boost step of the tutorial
    /// </summary>
    private void HiddenCheck()
    {
        if (Input.GetMouseButtonDown(0) && textPopup.activeInHierarchy)
        {
            textIndex++;
            if (textIndex == 12)
            {
                target.GetComponent<Renderer>().enabled = false;
            }
            else if (textIndex == 13)
            {
                EnableMovement();
                textPopup.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Called by NextStep from the TargetHitEvent to set up the skipping slices step of the tutorial
    /// </summary>
    private void TransitionToSkip()
    {
        // show UI
        textIndex++;
        textPopup.SetActive(true);
        uiManager.ActivateUI(2);

        // pause player movement
        DisableMovement();

        mode = TutorialMode.Skip;
    }

    /// <summary>
    ///  Progresses through the textboxes in the skip step of the tutorial
    /// </summary>
    private void SkipCheck ()
    {
        if (Input.GetMouseButtonDown(0) && textPopup.activeInHierarchy)
        {
            textIndex++;
            if (textIndex == 16)
            {
                EnableMovement();
                textPopup.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Called by NextStep from the SliceSkipEvent to set up the unskipping slices step of the tutorial
    /// </summary>
    private void TransitionToUnskip()
    {
        // show UI
        textIndex++;
        textPopup.SetActive(true);

        // setup game scene so slice is transparent and target visible
        target.GetComponent<Renderer>().enabled = true;
        Color matColor = slice.GetComponent<Renderer>().material.color;
        matColor.a = 0f;
        slice.GetComponent<Renderer>().material.color = matColor;
        slice.GetComponent<BrainHitEvent>().SetTransparent(true);

        // pause player movement
        DisableMovement();

        mode = TutorialMode.Unskip;
    }

    /// <summary>
    ///  Progresses through the textboxes in the unskip step of the tutorial
    /// </summary>
    private void UnskipCheck()
    {
        if (Input.GetMouseButtonDown(0) && textPopup.activeInHierarchy)
        {
            textIndex++;
            if (textIndex == 19)
            {
                EnableMovement();
                textPopup.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Called by NextStep from the TargetHitEvent to set up the hint step of the tutorial
    /// </summary>
    private void TransitionToHint()
    {
        // show UI
        textIndex++;
        textPopup.SetActive(true);
        uiManager.ActivateUI(3);

        // setup game objects
        target.SetActive(false);
        hintTarget.SetActive(true);
        hintTarget.GetComponent<Renderer>().enabled = false;

        // pause player movement
        DisableMovement();

        mode = TutorialMode.Hint;
    }

    /// <summary>
    /// Progresses through the textboxes in the hint step of the tutorial
    /// </summary>
    private void HintCheck()
    {
        if (Input.GetMouseButtonDown(0) && textPopup.activeInHierarchy)
        {
            Debug.Log(textIndex);
            textIndex++;
            if (textIndex == 22)
            {
                EnableMovement();
                textPopup.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Progresses through the textboxes in the finish step of the tutorial and exiting tutorial
    /// </summary>
    private void Finish()
    { 
        if (Input.GetMouseButtonDown(0) && textPopup.activeInHierarchy)
        {
            textIndex++;
            if (textIndex == 24)
            {
                uiManager.EndTutorialWrapper();
            }
        } else if (!textPopup.activeInHierarchy)
        {
            textPopup.SetActive(true);
        }
    }

    /// <summary>
    /// Allows player to move vertically and horizontally but not forward
    /// </summary>
    private void EnableMovement()
    {
        rb.constraints = RigidbodyConstraints.None;
        // rb.constraints = RigidbodyConstraints.FreezePositionZ;
    }

    /// <summary>
    /// Completely freezes player in place
    /// </summary>
    private void DisableMovement()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    /// <summary>
    /// Respawns the player at either the beginning of next steps or when a target is missed
    /// </summary>
    public void RespawnPlayer()
    {
        player.transform.position = spawnPoints.position;
        rb.gameObject.transform.position = spawnPoints.position;
    }

    /// <summary>
    /// Allows other classes to access the current state of the tutorial
    /// </summary>
    /// <returns></returns>
    public TutorialMode GetMode()
    {
        return mode;
    }
}
