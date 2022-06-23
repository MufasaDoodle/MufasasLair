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

	public void ReceiveDamage(float amount)
	{
		if (!IsSpawned) return;

		if((health -= amount) <= 0.0f)
		{
			controllingPlayer.TargetCharacterKilled(Owner);

			Despawn();
		}
	}
}
