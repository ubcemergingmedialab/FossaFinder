using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (CharacterController))]

public class NormalWalker : MonoBehaviour 
{
	//this game object's Transform
	private Transform goTransform;
	
	//the speed to move the game object
	private float speed = 6.0f;
	//the gravity
	private float gravity = 50.0f;
	
	//the direction to move the character
	private Vector3 moveDirection = Vector3.zero;
	//the attached character controller
	private CharacterController cController;
	
	//a ray to be cast 
	private Ray ray;
    private Ray leftRay;
    private Ray rightRay;

    private float distanceFromCentre= 0.26f;
    public float maxRotationDegrees;
    //A class that stores ray collision info
    private RaycastHit hit;
    private RaycastHit leftHitInfo, rightHitInfo;



    //a class to store the previous normal value
    private Vector3 oldNormal;
	//the threshold, to discard some of the normal value variations
	public float threshold = 0.009f;


    public float sphereRadius;
    public float maxDistance;
    public LayerMask layerMask;

    private Vector3 origin;
    private Vector3 direction;

    private float currentHitDistance;

    private JoystickControl joystick;

    Rigidbody playerRigidBody;
    private Vector3 moveVector;

    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float sideSpeed = 150;


    // Use this for initialization
    void Start () 
	{
		//get this game object's Transform
		goTransform = this.GetComponent<Transform>();
		//get the attached CharacterController component
		cController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        leftRay = new Ray(goTransform.position + new Vector3(0, 0, distanceFromCentre), -goTransform.up);
        rightRay = new Ray(goTransform.position - new Vector3(0, 0, distanceFromCentre), -goTransform.up);

        Debug.DrawRay(goTransform.position + new Vector3(0, 0, distanceFromCentre), -goTransform.up * 100, Color.blue);
        Debug.DrawRay(goTransform.position - new Vector3(0, 0, distanceFromCentre), -goTransform.up * 100, Color.blue);


        if (Physics.Raycast(goTransform.position + new Vector3(0, 0, distanceFromCentre), -goTransform.up, out leftHitInfo, 5, layerMask) 
           ||  Physics.Raycast(goTransform.position - new Vector3(0, 0, distanceFromCentre), -goTransform.up, out rightHitInfo, 5, layerMask))
        {
            Debug.Log("HIT");

            Vector3 averageNormal = (leftHitInfo.normal + rightHitInfo.normal) / 2;
            Vector3 averagePoint = (leftHitInfo.point + rightHitInfo.point) / 2;
            
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, averageNormal);
            Quaternion finalRotation = Quaternion.RotateTowards(goTransform.rotation, targetRotation, maxRotationDegrees);

            goTransform.up = averageNormal;


            //goTransform.position = averagePoint + transform.up * threshold;
        }
            

        //move the game object based on keyboard input
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //moveDirection.y -= gravity * Time.deltaTime * goTransform.up.y;

        //apply the movement relative to the attached game object orientation
        moveDirection = goTransform.TransformDirection(moveDirection);
        //apply the speed to move the game object
        moveDirection *= speed;
        cController.Move(moveDirection * Time.deltaTime);


        //Debug.DrawRay(transform.position, -goTransform.up *100, Color.blue);
        /*
        RaycastHit[] sphereCastHits = Physics.SphereCastAll(goTransform.position - new Vector3(0, 0, maxRadius), maxRadius, moveDirection, maxDistance);
        List<RaycastHit> coneCastHitList = new List<RaycastHit>();

        if (sphereCastHits.Length > 0)
        {
            for (int i = 0; i < sphereCastHits.Length; i++)
            {
                //sphereCastHits[i].collider.gameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
                Vector3 hitPoint = sphereCastHits[i].point;
                Vector3 directionToHit = hitPoint - goTransform.position;
                float angleToHit = Vector3.Angle(moveDirection, directionToHit);

                if (angleToHit < coneAngle)
                {
                    sphereCastHits[i].collider.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 1f);

                }
            }
        }
        

        

        //cast a ray from the current game object position downward, relative to the current game object orientation
        ray = new Ray(goTransform.position, -goTransform.up);  
		
		//if the ray has hit something
		if(Physics.Raycast(ray.origin,ray.direction, out hit, 5))//cast the ray 5 units at the specified direction  
		{  
			//if the current goTransform.up.y value has passed the threshold test
			if(oldNormal.y >= goTransform.up.y + threshold || oldNormal.y <= goTransform.up.y - threshold)
			{
				//set the up vector to match the normal of the ray's collision
				goTransform.up = hit.normal;
			}
			//store the current hit.normal inside the oldNormal
			oldNormal =  hit.normal;
		}
       
        
        origin = goTransform.position;
        direction = -goTransform.up;
        RaycastHit hit;
        if (Physics.SphereCast(origin, sphereRadius,direction,out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            currentHitDistance = hit.distance;
            //Debug.DrawLine(origin, origin + direction * currentHitDistance, Color.blue);

            goTransform.up = hit.normal;

        }

        Debug.DrawLine(origin, origin + direction * currentHitDistance, Color.blue);


        //move the game object based on keyboard input
        //moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));

        //apply the movement relative to the attached game object orientation
        moveDirection = goTransform.TransformDirection(moveDirection);
		//apply the speed to move the game object
		moveDirection *= speed;

        // Apply gravity downward, relative to the containing game object orientation
        moveDirection.y -= gravity * Time.deltaTime * goTransform.up.y;
        //moveDirection.y -= gravity * Time.deltaTime;

        // Move the game object
        cController.Move(moveDirection * Time.deltaTime);
        goTransform.Rotate(0, Input.GetAxis("Horizontal"), 0);
 */
    }

    /*
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }
    */

}





