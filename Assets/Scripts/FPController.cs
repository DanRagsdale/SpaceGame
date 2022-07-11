using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FPController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 80.0f;


	public LayerMask groundMask;


    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

	Transform refTransform;
	float rotSpeed;

	Vector3 velInitial;

	void Start()
    {
        characterController = GetComponent<CharacterController>();

		
		GameObject refObject = GameObject.Find("ReferenceFrame");
		rotSpeed = (float) (refObject.GetComponent<ReferenceSpin>().RotationSpeed / 180 * Mathf.PI);
		refTransform = refObject.transform;


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

		velInitial = Vector3.zero;
    }




    void Update()
    {

		Ray ray = new Ray(transform.position, -transform.up);
		RaycastHit hit;
		bool isGrounded = Physics.Raycast(ray, out hit, 1.05f, groundMask, QueryTriggerInteraction.Collide);

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float curSpeedX = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical");
        float curSpeedY = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal");


        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Simple test gravity. Need to update to properly account for rotation effects
		if(isGrounded && Input.GetButton("Jump")){
			velInitial = refTransform.InverseTransformVector(jumpSpeed * transform.up + new Vector3(-rotSpeed * transform.position.z, 0, rotSpeed * transform.position.x));
		} else if(isGrounded) {
			velInitial = refTransform.InverseTransformVector(new Vector3(-rotSpeed * transform.position.z, 0, rotSpeed * transform.position.x));
		}

		Vector3 velMovement = refTransform.TransformVector(velInitial);	
		Vector3 rotMovement = new Vector3(rotSpeed * transform.position.z, 0, -rotSpeed * transform.position.x);
		moveDirection += velMovement + rotMovement;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
        //characterController.Move(velMovement * Time.deltaTime);



        // Player and Camera rotation
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);


		// Rotate player so that their head is pointed toward the center of the space station
		float angle = Vector3.Angle(new Vector3(transform.position.x, 0, transform.position.z), -transform.up);
		Vector3 xp = Vector3.Cross((new Vector3(transform.position.x,0,transform.position.z)), -transform.up); 

		if(xp.y > 0)
		{
			transform.Rotate(0,-angle*0.5f,0, Space.World);
		} else {

			transform.Rotate(0,angle*0.5f,0, Space.World);
		}
    }

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, -1.2f * transform.up);
	}
}