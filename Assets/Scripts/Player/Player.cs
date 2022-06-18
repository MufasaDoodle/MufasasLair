using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
	public static Player LocalInstance { get; private set; }

	[SyncVar]
	public string playerName = "default";

	[SyncVar]
	public bool isReady;

	//[SyncVar]
	//public PlayerCharacter character;

	public override void OnStartClient()
	{
		base.OnStartClient();

		SetIsReadyServerRpc(false);

		if (!IsOwner) return;

		LocalInstance = this;
		SetUsernameServerRpc();

		ViewManager.Instance.Initialize();
	}

	public override void OnStartServer()
	{
		base.OnStartServer();

		GameManager.Instance.AddPlayer(this);
	}

	public override void OnStopServer()
	{
		base.OnStopServer();

		GameManager.Instance.RemovePlayer(this);
	}

	private void Update()
	{
		if (!IsOwner) return;
	}

	[ServerRpc(RequireOwnership = false)]
	public void SetUsernameServerRpc()
	{
		playerName = PlayerPrefs.GetString("username");
	}

	[ServerRpc(RequireOwnership = false)]
	public void SetIsReadyServerRpc(bool value)
	{
		isReady = value;
		GameManager.Instance.ReadyStateChangedServerRpc();
	}

	public void StartGame()
	{
		//switch to proper scene?
		//video 1:25:44
	}

	public void StopGame()
	{
		//TODO
	}
}
