using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyWalker : MonoBehaviour
{

    public float moveSpeed = 8.0f;
    public float headingRotSpeed = 60.0f;
    public LayerMask layerMask;
    Vector3 lastSavePosition;
    Quaternion lastSaveRotation;
    Vector3 lastSaveNormal;
    Vector3 v3Forward;

    private JoystickControl joystick;
    private Vector3 moveVector;

    [SerializeField] private float forwardSpeed = 170f;
    [SerializeField] private float sideSpeed = 150;

    // Start is called before the first frame update
    void Start()
    {
        //lastSavePosition = new Vector3(-2.4f, 2.8f, -5);
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<JoystickControl>();

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        Ray rayA = new Ray(transform.position, -transform.up);
        Debug.DrawRay(rayA.origin, -transform.up * 100, Color.blue);
        //Debug.Log(transform.position.x + " " + transform.position.y + " " + transform.position.z);

        if (Physics.Raycast(rayA, out hit, float.PositiveInfinity, layerMask))
        {
            Debug.Log("Hit");

            lastSavePosition = transform.position;
            lastSaveNormal = hit.normal;
            transform.position = hit.point + transform.up * 0.5f + hit.normal * 0.2f;
            // Inpput heading rotation
            float heading = Mathf.Atan2(joystick.Horizontal(), joystick.Vertical());
            transform.rotation = Quaternion.AngleAxis(heading * Mathf.Rad2Deg, transform.up);

            v3Forward = Vector3.Cross(hit.normal, transform.right).normalized;
            //float inputTranslation = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

            float inputTranslation = Mathf.Clamp01(new Vector2(joystick.Horizontal(), joystick.Vertical()).magnitude) * moveSpeed * Time.deltaTime;
            //float inputTranslation = joystick.Vertical() * moveSpeed * Time.deltaTime;
            transform.position += -v3Forward * inputTranslation;
            //Debug.Log(joystick.Horizontal());
            //float inputTranslationH = joystick.Horizontal() * moveSpeed * Time.deltaTime;
            //transform.position += -transform.right * inputTranslationH;

            Quaternion fro = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = fro;

            lastSaveRotation = transform.rotation;
        }
        else
        {
            Debug.Log("Lost contact...");
            transform.position = lastSavePosition;
            //transform.rotation = lastSaveRotation;
            Ray rayB = new Ray(transform.position, -transform.forward);
            Ray rayC = new Ray(transform.position, transform.up);
            Ray rayD = new Ray(transform.position, -transform.right);
            Debug.DrawRay(rayB.origin, -transform.forward * 100, Color.red);
            Debug.DrawRay(rayC.origin, transform.up * 100, Color.yellow);
            Debug.DrawRay(rayD.origin, -transform.right * 100, Color.green);
            if (Physics.Raycast(rayB, out hit, float.PositiveInfinity, layerMask) || Physics.Raycast(rayC, out hit, float.PositiveInfinity, layerMask) || Physics.Raycast(rayD, out hit, float.PositiveInfinity, layerMask))
            {
                transform.position = hit.point + transform.up * 0.5f + hit.normal * 0.2f;
                Quaternion fro = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                transform.rotation = fro;
            }
        }




        //float headingDeltaAngle = joystick.Horizontal() * Time.deltaTime * headingRotSpeed;
        //Quaternion headingDelta = Quaternion.AngleAxis(headingDeltaAngle, transform.up);
        //transform.rotation = headingDelta * transform.rotation;
    }
}
