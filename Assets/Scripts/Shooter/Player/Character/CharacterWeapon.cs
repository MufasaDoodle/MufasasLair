using FishNet.Component.Animating;
using FishNet.Connection;
using FishNet.Object;
using System;
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

	private bool isReloading = false;

	[SerializeField]
	private GameObject hitMarker;

	[SerializeField]
	private AudioClip hitmarkerSound;

	[SerializeField]
	private AudioClip shootingSound;

	[SerializeField]
	private AudioSource shootingSource;

	public override void OnStartNetwork()
	{
		base.OnStartNetwork();

        character = GetComponent<Character>();
        input = GetComponent<CharacterInput>();
	}

	public override void OnStartClient()
	{
		base.OnStartClient();

		hitMarker = ViewManager.Instance.GetSpecificView("HitMarker");
	}

	private void Update()
	{
        if (!IsOwner) return;

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

		if (input.reload)
		{
			StartCoroutine(WaitForReload());
		}
	}

	[ServerRpc]
	private void ReloadServerRpc()
	{
		if (shootCooldown > 0.0f) return; //can't reload if on shoot cooldown
		character.Reload();
	}

	[ServerRpc]
    private void FireServerRpc(Vector3 firePointPos, Vector3 firePointDirection)
	{
		if (character.ammo <= 0 || isReloading)
		{
			return;
		}

		PlayShootAnimation();
		PlayShootSoundClientRpc();
		character.SpendAmmo();
		if(Physics.Raycast(firePointPos, firePointDirection, out RaycastHit hit))
		{
			var charHit = hit.transform.GetComponent<Character>();

			if (charHit == null) return;

			StartCoroutine(HitMarker(Owner));

			charHit.ReceiveDamage(damage, GetComponent<Character>().controllingPlayer);
		}
	}

	[ObserversRpc(IncludeOwner = true)]
	private void PlayShootAnimation()
	{
		StartCoroutine(ShootAnimation());
	}

	[ObserversRpc(IncludeOwner = true)]
	public void PlayShootSoundClientRpc()
	{
		shootingSource.PlayOneShot(shootingSound);
	}

	public IEnumerator ShootAnimation()
	{
		GetComponent<NetworkAnimator>().Animator.SetBool("IsShooting", true);
		yield return new WaitForSeconds(0.93f);
		GetComponent<NetworkAnimator>().Animator.SetBool("IsShooting", false);
	}

	public IEnumerator WaitForReload()
	{
		GetComponent<NetworkAnimator>().Animator.SetBool("IsReloading", true);
		isReloading = true;
		yield return new WaitForSeconds(0.97f);
		GetComponent<NetworkAnimator>().Animator.SetBool("IsReloading", false);
		isReloading = false;
		ReloadServerRpc();
	}
		
	private IEnumerator HitMarker(NetworkConnection networkConnection)
	{
		HitMarkerActive(networkConnection);
		yield return new WaitForSeconds(0.3f);
		HitMarkerInactive(networkConnection);
	}

	[TargetRpc]
	public void HitMarkerActive(NetworkConnection networkConnection)
	{
		hitMarker.SetActive(true);
		GetComponent<CharacterCameraLook>().GetCamera().GetComponent<AudioSource>().PlayOneShot(hitmarkerSound);
	}

	[TargetRpc]
	public void HitMarkerInactive(NetworkConnection networkConnection)
	{
		hitMarker.SetActive(false);
	}
}
