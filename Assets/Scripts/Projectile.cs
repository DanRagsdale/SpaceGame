using System;
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

	Vector3 posInitial;

	void Start()
	{
		GameObject refObject = GameObject.Find("ReferenceFrame");
		
		rotSpeed = (float) (refObject.GetComponent<ReferenceSpin>().RotationSpeed / 180 * Math.PI);

		refTransform = refObject.transform;
		startTime = Time.time;
		velRelative = refTransform.InverseTransformVector(transform.TransformVector(Vector3.forward).normalized);
		velInitial = refTransform.InverseTransformVector(new Vector3(-rotSpeed * transform.position.z, 0, rotSpeed * transform.position.x));

		posInitial = refTransform.InverseTransformPoint(transform.position);
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

		Vector3 totalMovement = Time.deltaTime * (velMovement + initMovement + rotMovement);

		CheckCollisions(totalMovement);

		transform.Translate(totalMovement, Space.World);
    }

	void OnDrawGizmos()
	{
		//Vector3 rotMovement = new Vector3(rotSpeed * transform.position.z, 0, -rotSpeed * transform.position.x);
		//Vector3 velMovement = refTransform.TransformVector(Vector3.right);
		Gizmos.color = Color.green;
		Vector3 rotMovement = new Vector3(rotSpeed * transform.position.z, 0, -rotSpeed * transform.position.x);
		Gizmos.DrawRay(transform.position, rotMovement);
		
		Gizmos.color = Color.red;
		Vector3 initMovement = refTransform.TransformVector(velInitial);
		Vector3 velMovement = refTransform.TransformVector(velRelative).normalized * 10;
		Gizmos.DrawRay(transform.position, initMovement + velMovement);

		Gizmos.color = Color.black;
		Gizmos.DrawRay(transform.position, refTransform.TransformPoint(posInitial) - transform.position);
	}

	void CheckCollisions(Vector3 nextStep)
	{
		Ray ray = new Ray(transform.position, nextStep);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, nextStep.magnitude, collisionMask, QueryTriggerInteraction.Collide))
		{
			//IDamageable hitDamageable = hit.collider.GetComponent<IDamageable>();
			//if(hitDamageable != null)
			//{
			//	hitDamageable.TakeHit(damage, hit);
			//}
			GameObject.Destroy (gameObject);	
			Debug.Log("Collision Detected");
		}
	}
}
