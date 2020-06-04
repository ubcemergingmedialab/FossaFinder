using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Written by Oliver Vennike Riisager.
/// Controls the up and down application of forces.
/// </summary>
public class FlightController : MonoBehaviour
{
    private Rigidbody ufoRigidbody;
    private float speedIncrement;

    //DeadZones and rigidbody
    float thrustDeadZone = 0.55f;
    float thrustApplyDeadZone = 0.1f;
    private float tiltDifferenceBackwards = 0.0001f;
    private float tiltDifferenceForwards = 0.0001f;

    //Forcefactors
    float ufoThrust = 0;
    [SerializeField]
    float speed;
    [SerializeField]
    private float forwardSpeed;
    private float prevZAcc;

    void Start()
    {
        ufoRigidbody = GetComponent<Rigidbody>();
        GetTiltDeadZone();
    }


    private void GetTiltDeadZone()
    {
        thrustDeadZone = Mathf.Abs(Input.acceleration.z);
    }

    void Update()
    {
        ThrustMobileTilt();
    }

    private void ThrustMobileTilt()
    {
        float zAcc = Input.acceleration.z;
        speedIncrement = 0; //Reset the increment
        ufoThrust = 0; //Reset the thrust
        if (zAcc > -thrustDeadZone + thrustApplyDeadZone*2) // Is the phone rotated in negative or pos dir ?
        {
            if (prevZAcc - zAcc < -thrustApplyDeadZone) //If the difference is less than 0.1
            {
                speedIncrement = 1;
            }
            else 
            {
                speedIncrement = -1;
            }
        }
        else if (zAcc < -thrustDeadZone - thrustApplyDeadZone)
        {
            if (Mathf.Abs(prevZAcc) - Mathf.Abs(zAcc) < -thrustApplyDeadZone) //Same as above, but inverted to compensate for accelorometer
            {
                speedIncrement = -1;
            }
            else
            {
                speedIncrement = 1;
            }
        }
        ufoThrust = speed * speedIncrement;
        prevZAcc = zAcc;
    }

    private void FixedUpdate()
    {
        ufoRigidbody.AddForce(transform.forward * forwardSpeed);
        ApplyVerticalSpeed();
    }

    /// <summary>
    /// Applies a force in either up or down depending on the phones tilt.
    /// Only if there is any thrust to be applied.
    /// </summary>
    private void ApplyVerticalSpeed()
    {
        if (ufoThrust != 0)
        {
            ufoRigidbody.AddForce(-transform.up * ufoThrust);
        }
    }

    /// <summary>
    /// Determines the tilt on the Z axis and saves that forcefactor.
    /// Should be read in update, applied in fixedupdate.
    /// </summary>
    private void ThrustMobileTiltOld()
    {
        float zAcc = Input.acceleration.z;
        speedIncrement = 0; //Reset speedInc.
        if (zAcc < thrustDeadZone - tiltDifferenceForwards)
        {
            speedIncrement = 1;
            ufoThrust = speedIncrement * speed;
            return;
        }
        if (zAcc > thrustDeadZone + tiltDifferenceBackwards)
        {
            speedIncrement = -1;
            ufoThrust = speedIncrement * speed;
            return;
        }
    }
}
