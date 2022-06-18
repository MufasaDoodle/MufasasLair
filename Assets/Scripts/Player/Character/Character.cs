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
}
