using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Niccolo Pucci.
/// A timer that counts upwards.
/// </summary>
public class ActivityTimer : MonoBehaviour
{
    public Text timerText;

    private int minutes = 0;
    private int seconds = 0;

    private IEnumerator runningTimer;
    private IEnumerator flashingTimer;

    private const float CROSSFADE_IMMEDIATELY = 0f;
    private const float CROSSFADE_DURATION_SEC = 1.5f;

    private const float VISIBLE_ALPHA = 1f;
    private const float TRANSPARENT_ALPHA = 0f;

    private const bool CROSSFADE_IGNORE_TIMESCALE = false;

    private bool isFlashing = false;

    void Start () {
        runningTimer = RunningTimer ();
        flashingTimer = FlashingTimer ();

        timerText.CrossFadeAlpha (
            TRANSPARENT_ALPHA,
            CROSSFADE_IMMEDIATELY,
            CROSSFADE_IGNORE_TIMESCALE
        );

        StartTimer ();
    }

    public void ShowTimer () {
        TimerVisibility (
            VISIBLE_ALPHA,
            CROSSFADE_DURATION_SEC
        );
    }

    public void HideTimer ()
    {
        //TimerVisibility (
        //    TRANSPARENT_ALPHA,
        //    CROSSFADE_DURATION_SEC
        //);
    }

    public void FlashTimer ()
    {
        //if ( !isFlashing )
        //{
        //    StartCoroutine ( flashingTimer );
        //}
    }

    private void TimerVisibility ( 
        float targetAlpha,
        float crossFadeDuractionSec
    ) {
        if ( timerText != null )
        {
            timerText.CrossFadeAlpha (
                targetAlpha,
                crossFadeDuractionSec,
                CROSSFADE_IGNORE_TIMESCALE
            );
        }
    }

    public void StartTimer ()
    {
        ShowTimer ();
        StartCoroutine ( runningTimer );
    }

    public void StopTimer()
    {
        StopCoroutine ( runningTimer );
        FlashTimer ();
    }

    private void ResetTime () {
        minutes = 0;
        seconds = 0;
    }

    public string GetTimeScore ()
    {
        string minutes = GetMinutes ();
        if ( minutes.Length == 1 )
        {
            minutes = "0" + minutes;
        }

        string seconds = GetSeconds ();
        string time = minutes + ":" + seconds;
        return time;
    }

    public string GetUIFormatTime ()
    {
        string minutes = GetMinutes ();
        string seconds = GetSeconds ();
        string time = minutes + ":" + seconds;
        return time;
    }

    private string GetMinutes ()
    {
        string min = "" + minutes;
        
        return min;
    }

    private string GetSeconds ()
    {
        string sec = "" + seconds;
        if ( sec.Length == 1 )
        {
            sec = "0" + sec;
        }
        return sec;
    }

    private void UpdateTimerUI ()
    {
        if ( timerText != null )
        {
            string time = GetUIFormatTime ();
            timerText.text = time;
        }
    }

    private IEnumerator FlashingTimer ()
    {
        isFlashing = true;

        float currentTargetAlpha = TRANSPARENT_ALPHA;
        float waitTime = 1.5f * CROSSFADE_DURATION_SEC;

        while ( isFlashing )
        {
            TimerVisibility (
                currentTargetAlpha,
                CROSSFADE_DURATION_SEC
            );

            bool isTransparentAlpa = Mathf.Approximately (
                currentTargetAlpha, 
                TRANSPARENT_ALPHA
            );

            if ( isTransparentAlpa )
            {
                currentTargetAlpha = VISIBLE_ALPHA;
            }
            else
            {
                currentTargetAlpha = TRANSPARENT_ALPHA;
            }

            yield return new WaitForSeconds ( waitTime );
        }
    }

    private IEnumerator RunningTimer ()
    {
        ResetTime ();

        while ( true )
        {
            yield return new WaitForSeconds ( 1f );

            ModTimeSec ( 1 );
            UpdateTimerUI ();
        }
    }

    public void ModTimeSec ( int modSec )
    {
        seconds += modSec;

        while ( 60 <= seconds )
        {
            seconds -= 60;
            minutes += 1;
        }

        while ( seconds < 0f )
        {
            seconds += 60;
            minutes -= 1;
        }

        if ( minutes < 0 )
        {
            seconds = 0;
            minutes = 0;
        }

        UpdateTimerUI ();
    }
}
