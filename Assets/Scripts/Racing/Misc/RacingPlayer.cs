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

		Debug.Log("Starting racing player client.");

		Instance = this;
		SetUsernameServerRpc(PlayerPrefs.GetString("username"));
		RacingGameManager.Instance.AddPlayerServerRpc(this);
	}

	public void Reinit()
	{
		//for whatever reason, sometimes the instance does not get set. that is why this method exists
		Instance = this;
	}

	public override void OnStopClient()
	{
		base.OnStopClient();

		BetManager.Instance.RemoveBetFromPlayerServerRpc(OwnerId);
		RacingGameManager.Instance.RemovePlayerServerRpc(this);
	}

	public override void OnStartServer()
	{
		base.OnStartServer();
	}

	public override void OnStopServer()
	{
		base.OnStopServer();
	}

	[ServerRpc(RequireOwnership = false)]
	public void SetUsernameServerRpc(string username)
	{
		Debug.Log("Setting username to " + username);
		PlayerName = username;
	}

	public void ActivateHostControls()
	{
		RacingUIManager.Instance.bettingUI.ActivateHostControls();
	}

	public void DeactivateHostControls()
	{
		RacingUIManager.Instance.bettingUI.DeactivateHostControls();
	}

	public void ActivateBetting()
	{
		RacingUIManager.Instance.bettingUI.ActivateBetting();
	}

	public void DeactivateBetting()
	{
		RacingUIManager.Instance.bettingUI.DeactivateBetting();
	}

	public void RoundStart()
	{
		RacingUIManager.Instance.bettingUI.DeactivateBetting();
		if(IsServer || PlayerName.Contains("Host")) RacingUIManager.Instance.bettingUI.DeactivateHostControls();
	}

	public void RoundStop()
	{
		RacingUIManager.Instance.bettingUI.ActivateBetting();
		if (IsServer || PlayerName.Contains("Host")) RacingUIManager.Instance.bettingUI.ActivateHostControls();
	}
}
