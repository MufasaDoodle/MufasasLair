using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeapon : NetworkBehaviour
{
    private Character character;

    private CharacterInput input;

    [SerializeField]
    private float damage;

    [SerializeField]
    private float shotDelay;

    private float shootCooldown;

	[SerializeField]
	private Transform firePoint;

	public override void OnStartNetwork()
	{
		base.OnStartNetwork();

        character = GetComponent<Character>();
        input = GetComponent<CharacterInput>();
	}

	private void Update()
	{
        if (!IsOwner) return;

		Debug.DrawRay(firePoint.position, firePoint.forward * 10);

		if (shootCooldown <= 0.0f)
		{
			if (input.fire)
			{
				FireServerRpc(firePoint.position, firePoint.forward);

				shootCooldown = shotDelay;
			}
		}
		else
		{
			shootCooldown -= Time.deltaTime;
		}
	}

	[ServerRpc]
    private void FireServerRpc(Vector3 firePointPos, Vector3 firePointDirection)
	{
		if(Physics.Raycast(firePointPos, firePointDirection, out RaycastHit hit))
		{
			var charHit = hit.transform.GetComponent<Character>();

			if (charHit == null) return;

			charHit.ReceiveDamage(damage);
		}
	}
}
