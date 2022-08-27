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
		SetUsernameServerRpc(PlayerPrefs.GetString("username"));
		RacingGameManager.Instance.AddPlayer(this);
	}

	public override void OnStopClient()
	{
		base.OnStopClient();

		RacingGameManager.Instance.RemovePlayer(this);
	}

	public override void OnStartServer()
	{
		base.OnStartServer();

		//StartCoroutine(WaitForNetworkStart());
		//RacingGameManager.Instance.AddPlayer(this);
	}

	public override void OnStopServer()
	{
		base.OnStopServer();

		//RacingGameManager.Instance.RemovePlayer(this);
	}

	[ServerRpc(RequireOwnership = false)]
	public void SetUsernameServerRpc(string username)
	{
		Debug.Log("Setting username to " + username);
		PlayerName = username;
	}

	public void RoundStart()
	{
		RacingUIManager.Instance.bettingUI.DeactivateBetting();
		if(IsServer || RacingPlayer.Instance.PlayerName.Contains("Host")) RacingUIManager.Instance.bettingUI.DeactivateHostControls();
	}

	public void RoundStop()
	{
		RacingUIManager.Instance.bettingUI.ActivateBetting();
		if (IsServer || RacingPlayer.Instance.PlayerName.Contains("Host")) RacingUIManager.Instance.bettingUI.ActivateHostControls();
	}
}
