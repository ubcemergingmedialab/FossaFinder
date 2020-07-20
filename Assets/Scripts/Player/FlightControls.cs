using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the player in relation to the joystick.
/// </summary>
public class FlightControls : MonoBehaviour
{
    private JoystickControl joystick;

    [SerializeField] private float forwardSpeed = 170f;
    [SerializeField] private float sideSpeed = 150;
    [SerializeField] private float boosterPower = 400;
    Rigidbody playerRigidBody;
    private Vector3 moveVector;
    private bool boost;

    // Start is called before the first frame update
    void Start()
    {
        joystick = GameObject.FindGameObjectWithTag("JoyStick").GetComponent<JoystickControl>();
        playerRigidBody = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = Vector3.zero;

        moveVector.x = joystick.Horizontal() * sideSpeed;
        moveVector.z = forwardSpeed;
        if (ObjectiveUtility.IsMotor)
        {
            moveVector *= -1;
        }
        moveVector.y = joystick.Vertical() * sideSpeed;

    }

    /// <summary>
    /// Used by the boost button
    /// </summary>
    public void Boost(bool value)
    {
        boost = value;
    }
    private void FixedUpdate()
    {
        playerRigidBody.velocity = moveVector;
        if (boost)
            playerRigidBody.AddForce(transform.forward * boosterPower, ForceMode.VelocityChange);
    }
}
