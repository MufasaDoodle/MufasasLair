using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : NetworkBehaviour
{
	[SyncVar]
	public Player controllingPlayer;

	[SyncVar]
	public float health;

	[SyncVar]
	public int ammo;

	public void ReceiveDamage(float amount, Player damagedBy)
	{
		if (!IsSpawned) return;

		if((health -= amount) <= 0.0f)
		{
			ScoreboardManager.Instance.PlayerKilled(controllingPlayer, damagedBy);

			controllingPlayer.TargetCharacterKilled(Owner);

			Despawn();
		}
	}

	public void SpendAmmo()
	{
		ammo -= 1;
		if (ammo <0 ) ammo = 0;
	}

	public void Reload()
	{
		ammo = 5;
	}
}
