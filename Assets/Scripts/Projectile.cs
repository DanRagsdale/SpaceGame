﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public LayerMask collisionMask;

	float rotSpeed;

	float speed = 10.0f;
	float damage = 1.0f;

	float startTime;



	Transform refTransform;

	Vector3 velRelative;
	Vector3 velInitial;


	void Start()
	{
		GameObject refObject = GameObject.Find("ReferenceFrame");
		
		rotSpeed = (float) (refObject.GetComponent<ReferenceSpin>().RotationSpeed / 180 * Math.PI);

		refTransform = refObject.transform;
		startTime = Time.time;
		velRelative = refTransform.InverseTransformVector(transform.TransformVector(Vector3.forward).normalized);
		velInitial = refTransform.InverseTransformVector(new Vector3(-rotSpeed * transform.position.z, 0, rotSpeed * transform.position.x));
	}

    void Update()
    {
		if(Time.time - startTime > 5.0f)
		{
			GameObject.Destroy(gameObject);
		}

		float moveStep = speed * Time.deltaTime;

		Vector3 velMovement = refTransform.TransformVector(velRelative)* 15;
		Vector3 initMovement = refTransform.TransformVector(velInitial);
		Vector3 rotMovement = new Vector3(rotSpeed * transform.position.z, 0, -rotSpeed * transform.position.x);
		
		transform.Translate(Time.deltaTime * (rotMovement  + velMovement + initMovement), Space.World);
    }

	void OnDrawGizmos()
	{
		//Vector3 rotMovement = new Vector3(rotSpeed * transform.position.z, 0, -rotSpeed * transform.position.x);
		//Vector3 velMovement = refTransform.TransformVector(Vector3.right);
		Gizmos.color = Color.red;
		Vector3 initMovement = refTransform.TransformVector(velInitial);
		Gizmos.DrawRay(transform.position, initMovement);
		
		Gizmos.color = Color.green;
		Vector3 rotMovement = new Vector3(rotSpeed * transform.position.z, 0, -rotSpeed * transform.position.x);
		Gizmos.DrawRay(transform.position, rotMovement);
		
		Gizmos.color = Color.blue;
		Vector3 velMovement = refTransform.TransformVector(velRelative).normalized * 10;
		Gizmos.DrawRay(transform.position, velMovement);
	}

	//void CheckCollisions(float moveStep)
	//{
	//	Ray ray = new Ray(transform.position, transform.forward);
	//	RaycastHit hit;

	//	if (Physics.Raycast(ray, out hit, moveStep, collisionMask, QueryTriggerInteraction.Collide))
	//	{
	//		IDamageable hitDamageable = hit.collider.GetComponent<IDamageable>();
	//		if(hitDamageable != null)
	//		{
	//			hitDamageable.TakeHit(damage, hit);
	//		}
	//		GameObject.Destroy (gameObject);	
	//	}
	//}
}