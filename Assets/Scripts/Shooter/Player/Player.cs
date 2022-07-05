using FishNet;
using FishNet.Component.Spawning;
using FishNet.Connection;
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

		if (GameManager.Instance.isGameStarted)
			StartGame(new Vector3(0, 0, 0));
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
		ViewManager.Instance.Show<RespawnView>();
	}

	[ServerRpc(RequireOwnership = false)]
	public void SpawnCharacterServerRpc()
	{
		Transform[] spawns = InstanceFinder.NetworkManager.GetComponent<PlayerSpawner>().Spawns;
		CharacterSpawn(spawns[Random.Range(1, spawns.Length)].position);
	}

	public void CharacterSpawn(Vector3 spawnPos)
	{
		GameObject characterPrefab = Addressables.LoadAssetAsync<GameObject>("ShooterCharacter").WaitForCompletion();

		GameObject charInstance = Instantiate(characterPrefab, spawnPos, Quaternion.identity);
		Spawn(charInstance, Owner);

		controlledPlayer = charInstance.GetComponent<Character>();

		controlledPlayer.controllingPlayer = this;

		if (Camera.main != null)
			Destroy(Camera.main.gameObject);

		TargetCharacterSpawned(Owner);
	}

	public void StopGame()
	{
		if (controlledPlayer != null && controlledPlayer.IsSpawned)
		{
			controlledPlayer.Despawn();
		}
	}

	[TargetRpc]
	private void TargetCharacterSpawned(NetworkConnection networkConnection)
	{
		ViewManager.Instance.Show<ShooterView>();
	}

	[TargetRpc]
	public void TargetCharacterKilled(NetworkConnection networkConnection)
	{
		ViewManager.Instance.Show<RespawnView>();
	}
}
