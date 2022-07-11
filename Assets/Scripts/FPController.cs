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

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    
	void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float curSpeedX = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical");
        float curSpeedY = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal");


        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Simple test gravity. Need to update to properly account for rotation effects
		moveDirection += 20*(new Vector3(transform.position.x, 0, transform.position.z)).normalized;


        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

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
}