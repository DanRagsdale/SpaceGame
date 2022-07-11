using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public Transform muzzle;
	public Projectile bullet;
	public float firingPeriod = 0.1f;

	float nextShotTime;

	public void Shoot()
	{
		if(Time.time > nextShotTime)
		{
			nextShotTime = Time.time + firingPeriod;
			Projectile newProjectile = Instantiate(bullet, muzzle.position, muzzle.rotation) as Projectile;
		}
	}
}
