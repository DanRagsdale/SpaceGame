using UnityEngine;
 
public class FPSimple : MonoBehaviour {
 
    public Rigidbody Rigid;
    public float MouseSensitivity = 1.0f;

    public float MoveSpeed = 1.0f;
	public float MoveAcceleration = 1.0f;

    public float JumpForce = 1.0f;

	public GameObject Camera;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

    void Update ()
    {
		//Rigid.MoveRotation(Rigid.rotation * Quaternion.Euler(new Vector3(0, Input.GetAxis("Mouse X") * MouseSensitivity, 0)));
		transform.Rotate(0, Input.GetAxis("Mouse X") * MouseSensitivity, 0, Space.World);
		Camera.transform.Rotate(-Input.GetAxis("Mouse Y") * MouseSensitivity, 0, 0);
    }

	void FixedUpdate()
	{
		Rigid.AddForce(transform.forward * Input.GetAxis("Vertical") * MoveSpeed + transform.right * Input.GetAxis("Horizontal") * MoveSpeed);
        if (Input.GetKeyDown("space"))
            Rigid.AddForce(transform.up * JumpForce);

	}
}
 