using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceSpin : MonoBehaviour
{
	public float RotationSpeed = 30;

    void Start()
    {
        
    }

    void Update()
    {
		transform.RotateAround(transform.position, Vector3.up, RotationSpeed * Time.deltaTime);
    }
}
