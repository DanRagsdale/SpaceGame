using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
	public Transform weaponHold;	
	public Transform crosshair;

	public Gun[] guns;
	int weaponIndex = 0;

	Gun equippedGun;

	void Start()
	{
		if (guns[0] != null)
		{
			EquipGun(guns[0]);
		}
	}

	public void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Shoot();
		}
		if (Input.GetButtonDown("Change")){
			weaponIndex = (weaponIndex + 1 ) % (guns.Length + 1);
			if(weaponIndex == guns.Length)
			{
				HolsterGun();
			} else {
				EquipGun(guns[weaponIndex]);
			}
		}
	}

    public void EquipGun(Gun gunToEquip)
	{
		if (equippedGun != null)
		{
			Destroy(equippedGun.gameObject);
		}
		equippedGun = Instantiate (gunToEquip, weaponHold.position, weaponHold.rotation * gunToEquip.transform.rotation) as Gun;
		equippedGun.transform.parent = weaponHold;
		equippedGun.SetMuzzle(crosshair);
	}

	public void HolsterGun()
	{
		if (equippedGun != null)
		{
			Destroy(equippedGun.gameObject);
		}
	}

	public void Shoot() 
	{
		if (equippedGun != null)
		{
			equippedGun.Shoot();
		}
	}
}
