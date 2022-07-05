using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardManager : NetworkBehaviour
{
	public static ScoreboardManager Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	[ServerRpc]
	public void PlayerKilled(Player killed, Player killedBy)
	{
		//do some UI stuff to show who killed who
		Debug.Log($"{killedBy.playerName} killed {killed.playerName}");
	}
}
