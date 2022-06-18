using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Player : NetworkBehaviour
{
	public static Player LocalInstance { get; private set; }

	[SyncVar]
	public string playerName = "default";

	[SyncVar]
	public bool isReady;

	[SyncVar]
	public Character controlledPlayer;

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

	public void StartGame(Vector3 spawnPos)
	{
		GameObject characterPrefab = Addressables.LoadAssetAsync<GameObject>("ShooterCharacter").WaitForCompletion();

		GameObject charInstance = Instantiate(characterPrefab, spawnPos, Quaternion.identity);
		Spawn(charInstance, Owner);

		controlledPlayer = charInstance.GetComponent<Character>();
	}

	public void StopGame()
	{
		if(controlledPlayer != null && controlledPlayer.IsSpawned)
		{
			controlledPlayer.Despawn();
		}
	}
}
