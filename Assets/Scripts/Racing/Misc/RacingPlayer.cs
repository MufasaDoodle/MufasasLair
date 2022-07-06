using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingPlayer : NetworkBehaviour
{
	public static RacingPlayer Instance;

	[SyncVar]
	public string PlayerName = "default";

	public override void OnStartClient()
	{
		base.OnStartClient();

		if (!IsOwner) return;

		Instance = this;
		SetUsernameServerRpc();
	}

	public override void OnStartServer()
	{
		base.OnStartServer();

		RacingGameManager.Instance.AddPlayer(this);
	}

	public override void OnStopServer()
	{
		base.OnStopServer();

		RacingGameManager.Instance.RemovePlayer(this);
	}

	[ServerRpc(RequireOwnership = false)]
	public void SetUsernameServerRpc()
	{
		PlayerName = PlayerPrefs.GetString("username");
	}

	public void RoundStart()
	{
		RacingUIManager.Instance.bettingUI.DeactivateBetting();
	}

	public void RoundStop()
	{
		RacingUIManager.Instance.bettingUI.ActivateBetting();
	}
}
