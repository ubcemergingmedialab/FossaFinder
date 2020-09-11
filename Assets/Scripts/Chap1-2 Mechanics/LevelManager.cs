using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Written by Oliver Vennike Riisager. Modified by Kimberly Burke
/// Manages all things related to a level. Slices, spawnpoints, score.
/// </summary>
public class LevelManager : MonoBehaviour
{
    //UI
    [SerializeField]
    UIManager uIManager;

    [SerializeField]
    Canvas EndGame;

    // Audio
    [SerializeField]
    AudioManager audioManager;

    //Minimap
    RawImage minimapImage;

    // CH 1 skip
    [SerializeField] GameObject animateSkip;
    [SerializeField] GameObject regularSkip;

    //Player
    private GameObject player;
    private Rigidbody playerRigidBody;
    public static float maxRange;

    //Level elements
    private Queue<SliceController> slices;
    [SerializeField]
    private Animator positive, negative, emptyIndicator;
    public static SliceController currentSlice;
    private SliceController firstSlice;
    public static bool endGame;
    private static bool perfectGame = false;

    [SerializeField]
    Text findObjective;
    private float distToStart = 1000;
    private bool flipped;
    private GameObject[] HLControllers;
    private bool colFlipped;

    public GameObject[] BGOutlines { get; private set; }
    private static bool scoreStored;


    // Start is called before the first frame update
    void Start()
    {
        endGame = false;
        scoreStored = false;

        if (ObjectiveUtility.CurrentGameMode != ObjectiveUtility.GameMode.Training)
            perfectGame = true;
        //Listeners
        EndLevelTrigger.endLevelEvent.AddListener(EndLevel);
        SliceController.negativeFeedback.AddListener(Negative);
        SliceController.positiveFeedback.AddListener(Positive);
        SliceController.emptyHintIndication.AddListener(EmptyHint);
        PlayerListener.sliceHit.AddListener(ChangeCurrentSlice);

        if (ObjectiveUtility.CurrentChapter == ObjectiveUtility.Chapter.Chapter1)
            minimapImage = GameObject.FindGameObjectWithTag("Minimap").GetComponent<RawImage>();

        maxRange = GameObject.FindGameObjectWithTag("Slice").GetComponent<Collider>().bounds.size.x / 2;

        PrepSlices();
        PrepEndTrigger();
        if (BGOutlines == null)
        {
            BGOutlines = GameObject.FindGameObjectsWithTag("BGOutline");
        }
            

        if (!ObjectiveUtility.IsMotor)
        {
            PrepPlayer();
            if (colFlipped)
                FlipColliders();
        }
        else
        {
            FlipColliders();
            PrepPlayerMotor();
            FlipAndMoveBGOutlines();
        }

        if (ObjectiveUtility.CurrentGameMode == ObjectiveUtility.GameMode.Master)
            RemoveOutlines();
        if (ObjectiveUtility.CurrentGameMode == ObjectiveUtility.GameMode.Training)
            EnableTraining();

        findObjective.text = ObjectiveUtility.CurrentObjective.Replace('_', ' ');

        if (currentSlice.ActiveObjective == null && ObjectiveUtility.CurrentChapter == ObjectiveUtility.Chapter.Chapter1 && ObjectiveUtility.CurrentGameMode == ObjectiveUtility.GameMode.Training)
        {
            animateSkip.SetActive(true);
            regularSkip.SetActive(false);
            animateSkip.GetComponent<Animator>().Play("SkipButton");
        }

        DBManager.Instance.ConnectionTest();
    }

    private void RemoveOutlines()
    {
        for (int i = 0; i < BGOutlines.Length; i++)
        {
            var currentColor = BGOutlines[i].layer = 9;//The minimap layer, used for aftergame rot cam
        }
    }

    /// <summary>
    /// prep the endtrigger
    /// </summary>
    private void PrepEndTrigger()
    {
        var endTrigger = GameObject.FindGameObjectWithTag("EndTrigger");
        endTrigger.transform.position = GetEndTriggerSpawnPoint();
    }

    /// <summary>
    /// Finds and flips and moves all background outline images.
    /// </summary>
    private void FlipAndMoveBGOutlines()
    {
        if (!flipped)
        {
            for (int i = 0; i < BGOutlines.Length; i++)
            {
                var currentScale = BGOutlines[i].transform.localScale;
                BGOutlines[i].transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z * -1); //Flip

                var currentPos = BGOutlines[i].transform.localPosition;
                BGOutlines[i].transform.localPosition = new Vector3(currentPos.x, currentPos.y - (currentPos.y * 2), currentPos.z); //move
            }
            flipped = true;
        }
        else
        {
            for (int i = 0; i < BGOutlines.Length; i++)
            {
                var currentScale = BGOutlines[i].transform.localScale;
                BGOutlines[i].transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z * -1); //Flip back

                var currentPos = BGOutlines[i].transform.localPosition;
                BGOutlines[i].transform.localPosition = new Vector3(currentPos.x, currentPos.y + (currentPos.z * 2), currentPos.z); //moveback to original
            }
            flipped = false;
        }
    }

    private void FlipColliders()
    {
        if (!colFlipped)
        {
            foreach (var slice in slices)
            {
                for (int i = 0; i < slice.SliceObjectiveColliders.Length; i++)
                {
                    var currentCol = slice.SliceObjectiveColliders[i].transform;
                    currentCol.localScale = new Vector3(currentCol.localScale.x, -currentCol.localScale.y, currentCol.localScale.z);
                }
            }
            colFlipped = true;
        }
        else
        {
            foreach (var slice in slices)
            {
                for (int i = 0; i < slice.SliceObjectiveColliders.Length; i++)
                {
                    var currentCol = slice.SliceObjectiveColliders[i].transform;
                    currentCol.localScale = new Vector3(currentCol.localScale.x, -currentCol.localScale.y, currentCol.localScale.z);
                }
            }
            colFlipped = false;
        }
    }

    /// <summary>
    /// Sets the players position corresponding with a motor level, aka swaps sides and rotates him
    /// </summary>
    private void PrepPlayerMotor()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject;
        player.transform.Rotate(player.transform.up, 180);
        player.transform.position = GetPlayerSpawnPoint();
        playerRigidBody = player.GetComponentInChildren<Rigidbody>();
        playerRigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    /// <summary>
    /// Finds the player and freezes his position
    /// </summary>
    private void PrepPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject;
        player.transform.position = GetPlayerSpawnPoint();
        playerRigidBody = player.GetComponentInChildren<Rigidbody>();
        playerRigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    /// <summary>
    /// Prepares all slices depending on gamemode
    /// Also initializes minimap images
    /// </summary>
    private void PrepSlices()
    {
        slices = ObjectiveUtility.Slices;
        currentSlice = slices.Peek();
        if (firstSlice == null)
            firstSlice = currentSlice;
        if (ObjectiveUtility.CurrentChapter == ObjectiveUtility.Chapter.Chapter1)
            minimapImage.texture = currentSlice.MiniMapTexture;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Negative()
    {
        if (!currentSlice.Disabled) //If we hit something already in this "layer" or the slice is skipped
        {
            negative.Play("redBorder");
            audioManager.PlayWrongSound();
            currentSlice.Missed();
            ChangeCurrentSlice(currentSlice);
            if (ObjectiveUtility.CurrentChapter == ObjectiveUtility.Chapter.Chapter1)
                minimapImage.texture = currentSlice.MiniMapTexture; //Change the image to the next slices image
            uIManager.UpdateScoreUI();
            Handheld.Vibrate();
        }
        perfectGame = false; // no perfect game = no star reward
    }

    /// <summary>
    /// 
    /// </summary>
    private void Positive()
    {
        if (!currentSlice.Disabled)
        {
            positive.Play("greenBorder");
            audioManager.PlayCorrectSound();
            currentSlice.Correct();
            ChangeCurrentSlice(currentSlice);
            if (ObjectiveUtility.CurrentChapter == ObjectiveUtility.Chapter.Chapter1)
                minimapImage.texture = currentSlice.MiniMapTexture; //Change the image to the next slices image
            uIManager.UpdateScoreUI();
        }
    }

    /// <summary>
    /// Played from the slice controller when a hint reveals the slice is empty.
    /// </summary>
    private void EmptyHint()
    {
        emptyIndicator.Play("emptyIndicator");
    }

    /// <summary>
    /// Changes the minimap
    /// </summary>
    /// <param name="hitSliceController">The slice we want to change image to</param>
    private void ChangeMinimapImage(SliceController hitSliceController)
    {
        if (ObjectiveUtility.CurrentChapter == ObjectiveUtility.Chapter.Chapter1)
        {
            var sliceInQueue = slices.Peek(); //Peek the next slice
            if (ReferenceEquals(sliceInQueue, hitSliceController)) //If its equal to the one we hit
            {
                var sliceWeHit = slices.Dequeue(); //Get the one we just hit and put it back in the queue
                sliceWeHit.DisableObjectiveColliders();
                sliceWeHit.DisableSlice();
                slices.Enqueue(sliceWeHit);

                currentSlice = slices.Peek(); //Update the current slice
                minimapImage.texture = currentSlice.MiniMapTexture; //Change the image to the next slices image
            }
            else { Debug.LogError("The queue of slices is not ordered - LevelManager. Hit : " + hitSliceController.name + " Next : " + sliceInQueue.name); }
        }
        else if (ObjectiveUtility.CurrentChapter == ObjectiveUtility.Chapter.Chapter2)
        {
            var sliceInQueue = slices.Peek(); //Peek the next slice
            if (ReferenceEquals(sliceInQueue, hitSliceController)) //If its equal to the one we hit
            {
                var sliceWeHit = slices.Dequeue(); //Get the one we just hit and put it back in the queue
                sliceWeHit.DisableObjectiveColliders();
                sliceWeHit.DisableSlice();
                slices.Enqueue(sliceWeHit);

                currentSlice = slices.Peek(); //Update the current slice
            }
        }
    }

    void ChangeCurrentSlice(SliceController hitSliceController)
    {
        var sliceInQueue = slices.Peek(); //Peek the next slice
        if (ReferenceEquals(sliceInQueue, hitSliceController)) //If its equal to the one we hit
        {
            var sliceWeHit = slices.Dequeue(); //Get the one we just hit and put it back in the queue
            sliceWeHit.DisableObjectiveColliders();
            sliceWeHit.DisableSlice();
            slices.Enqueue(sliceWeHit);

            currentSlice = slices.Peek(); //Update the current slice
        }
#if UNITY_EDITOR
        else { Debug.LogError("The queue of slices is not ordered - LevelManager. Hit : " + hitSliceController.name + " Next : " + sliceInQueue.name); }
#endif
        Debug.Log(hitSliceController.ActiveObjective);
        if (currentSlice.ActiveObjective == null && ObjectiveUtility.CurrentChapter == ObjectiveUtility.Chapter.Chapter1 && ObjectiveUtility.CurrentGameMode == ObjectiveUtility.GameMode.Training)
        {
            animateSkip.SetActive(true);
            regularSkip.SetActive(false);
            animateSkip.GetComponent<Animator>().Play("SkipButton");
        }
    }

    /// <summary>
    /// Activates hint on the next slice when not in training mode
    /// </summary>
    public void HintSlice()
    {
        if (ObjectiveUtility.CurrentGameMode != ObjectiveUtility.GameMode.Training)
        {
            perfectGame = false;
            currentSlice.EnableHint();
        }
    }

    /// <summary>
    /// Skips a slice
    /// </summary>
    public void SkipSlice()
    {
        currentSlice.ToggleSkip();
    }

    /// <summary>
    /// Enables hint highlights
    /// </summary>
    private void EnableTraining()
    {
        for (int i = 0; i < slices.Count; i++)
        {
            SliceController currentSlice = slices.Dequeue();
            currentSlice.EnableHint();
            slices.Enqueue(currentSlice);
        }
    }

    /// <summary>
    /// Saves the score of the player.
    /// </summary>
    public static void SaveScore()
    {
        // TODO - account for star value being greater than 1
        int star = 0;
        int score = ObjectiveUtility.GetScore();
        if (GameSaveUtility.OfflineMode == 0 && GameSaveUtility.GetID() != -2) 
        {
            // progress will not be stored if application is offline or if player is on as an offline account (ID = -2)
            if (!scoreStored)
            {
                if (ObjectiveUtility.CurrentGameMode == ObjectiveUtility.GameMode.Training) { perfectGame = false; }
                GameSaveUtility.SaveLevelScoreInt(ObjectiveUtility.GetScore(), perfectGame);
                if (perfectGame)
                {
                    DBManager.Instance.UpdateLeaderboard();
                }
                scoreStored = true;
            }
            // Only store to database if the score is higher than what is in the database
            if (ObjectiveUtility.CurrentGameMode == ObjectiveUtility.GameMode.Master)
            {
                if (PlayerPrefs.GetInt("Master" + ObjectiveUtility.CurrentObjective, -1) > score)
                    score = PlayerPrefs.GetInt("Master" + ObjectiveUtility.CurrentObjective, -1);
                if (perfectGame || GameSaveUtility.GetStarBoolean(ObjectiveUtility.CurrentObjective, "Perfect Master"))
                    star = 1;
            }
            else
            {
                if (PlayerPrefs.GetInt(ObjectiveUtility.CurrentObjective, -1) > score)
                    score = PlayerPrefs.GetInt(ObjectiveUtility.CurrentObjective, -1);
                if (perfectGame || GameSaveUtility.GetStarBoolean(ObjectiveUtility.CurrentObjective, "Perfect"))
                    star = 1;
            }
            DBManager.Instance.UpdateLevelScore(ObjectiveUtility.CurrentObjective, score, star);
        }
    }

    /// <summary>
    /// Starts a level by removing movement constraints on the player.
    /// </summary>
    public void StartLevel()
    {
        uIManager.ShowActivityStartScreen();
        playerRigidBody.constraints = RigidbodyConstraints.None;
    }

    /// <summary>
    /// Pauses a level by constraining his movement
    /// </summary>
    public void PauseLevel()
    {
        uIManager.PauseGame();
        playerRigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    /// <summary>
    /// Resumes a level by removing movement constraints on the player.
    /// </summary>
    public void ResumeLevel()
    {
        uIManager.ResumeGame();
        playerRigidBody.constraints = RigidbodyConstraints.None;
    }

    /// <summary>
    /// Ends a level and constrains the players movements
    /// </summary>
    public void EndLevel()
    {
        ObjectiveUtility.ResetSlices();
        SaveScore();

        EndGame.gameObject.SetActive(false);
        uIManager.ShowActivityEndScreen(true, perfectGame);
        playerRigidBody.constraints = RigidbodyConstraints.FreezeAll;
        endGame = true;
    }

    /// <summary>
    /// Restarts a level
    /// </summary>
    public void Restart()
    {
        ObjectiveUtility.Restart(); //Tell the objectiveUtility to do the same

        player.transform.position = GetPlayerSpawnPoint(); //Resets the players position
        endGame = false;
    }

    /// <summary>
    /// Gets the spawnpoint of the player, depending on the chapter
    /// </summary>
    /// <returns></returns>
    private Vector3 GetPlayerSpawnPoint()
    {
        switch (ObjectiveUtility.CurrentChapter)
        {
            case ObjectiveUtility.Chapter.Chapter1:
                return new Vector3(firstSlice.transform.position.x, firstSlice.transform.position.y, firstSlice.transform.position.z - distToStart);
            case ObjectiveUtility.Chapter.Chapter2:
                if (!ObjectiveUtility.IsMotor)
                    return new Vector3(firstSlice.transform.position.x, firstSlice.transform.position.y, firstSlice.transform.position.z - distToStart);
                else
                    return new Vector3(firstSlice.transform.position.x, firstSlice.transform.position.y, firstSlice.transform.position.z + distToStart);
        }
        return Vector3.zero;
    }
    /// <summary>
    /// Gets the spawnpoint of the endtrigger, depending on the chapter
    /// </summary>
    /// <returns></returns>
    private Vector3 GetEndTriggerSpawnPoint()
    {
        var pos = FindFurthestSlice().transform.position;
        switch (ObjectiveUtility.CurrentChapter)
        {
            case ObjectiveUtility.Chapter.Chapter1:
                return new Vector3(pos.x, pos.y, pos.z + distToStart / 5);
            case ObjectiveUtility.Chapter.Chapter2:
                if (ObjectiveUtility.IsMotor)
                    return new Vector3(pos.x, pos.y, pos.z - distToStart / 5);
                else
                    return new Vector3(pos.x, pos.y, pos.z + distToStart / 5);

        }
        return Vector3.zero;
    }

    /// <summary>
    /// Finds the slice that is furthest away, takes motor levels into account
    /// </summary>
    /// <returns></returns>
    private SliceController FindFurthestSlice()
    {
        SliceController current = null;
        foreach (var slice in ObjectiveUtility.Slices)
        {
            if (current == null)
            {
                current = slice;
                continue;
            }
            if (!ObjectiveUtility.IsMotor)
            {
                if (slice.transform.position.z > current.transform.position.z)
                    current = slice;
            }
            else
            {
                if (slice.transform.position.z < current.transform.position.z)
                    current = slice;
            }
        }
        return current;
    }
}
