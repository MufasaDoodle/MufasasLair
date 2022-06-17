using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
	public static GameManager Instance { get; private set; }

	[SyncObject]
	[SerializeField]
	public readonly SyncList<Player> players = new();

	[SyncVar]
	public bool canStart;

	public LobbyManagerUI lobbyManager;

	[SyncVar(Channel = FishNet.Transporting.Channel.Reliable, OnChange = nameof(OnSelectedMapChange))]
	public int currentMap;

	[SyncObject]
	public readonly SyncList<ShooterMap> maps = new();

	public override void OnStartNetwork()
	{
		base.OnStartNetwork();

		Instance = this;
		maps.Add(new ShooterMap(0, "Jim's Dirty Butthole", 8, "A 1:1 recreation of the place nobody wants to visit", new Color(139f / 255f, 69f / 255f, 19f / 255f)));
		maps.Add(new ShooterMap(1, "Cherry's Hairy Bush", 8, "Basically Chewbacca wtf", Color.red));
		maps.Add(new ShooterMap(2, "Lunar's Smelly Armpits", 8, "I can smell them across the globe good god", Color.cyan));
		maps.Add(new ShooterMap(3, "Ego's Suculent Muscles", 8, "Makes me question my heterosexuality", Color.blue));
	}

	[Server]
	public void StartGame()
	{
		if (!canStart) return;

		for (int i = 0; i < players.Count; i++)
		{
			players[i].StartGame();
		}
	}

	[Server]
	public void StopGame()
	{
		for (int i = 0; i < players.Count; i++)
		{
			players[i].StopGame();
		}
	}

	[ServerRpc(RequireOwnership = false)]
	public void AddPlayer(Player player)
	{
		players.Add(player);
		lobbyManager.RefreshClientRpc(players.ToArray());
	}

	[ServerRpc(RequireOwnership = false)]
	public void RemovePlayer(Player player)
	{
		players.Remove(player);
		lobbyManager.RefreshClientRpc(players.ToArray());
	}

	[ServerRpc(RequireOwnership = false)]
	public void ReadyStateChangedServerRpc()
	{
		lobbyManager.RefreshClientRpc(players.ToArray());
		canStart = players.All(player => player.isReady);
	}

	private void OnSelectedMapChange(int prev, int next, bool asServer)
	{
		BroadcastMapChangeClientRpc(maps[next]);
	}

	[ServerRpc(RequireOwnership = false)]
	public void SelectMapServerRpc(int id)
	{
		currentMap = id;
	}

	[ObserversRpc(BufferLast = true)]
	public void BroadcastMapChangeClientRpc(ShooterMap map)
	{
		View viewRaw = ViewManager.Instance.GetView<ShooterLobbyView>();

		if (viewRaw == null)
		{
			Debug.LogError("Could not find Lobby View");
			return;
		}

		ShooterLobbyView view = (ShooterLobbyView)viewRaw;
		view.SetMapDescription(map);
	}
}

public struct ShooterMap
{
	public int id;
	public string mapName;
	public int playerNum;
	public string description;
	public Color mapColor;

	public ShooterMap(int id, string mapName, int playerNum, string description, Color mapColor)
	{
		this.id = id;
		this.mapName = mapName;
		this.playerNum = playerNum;
		this.description = description;
		this.mapColor = mapColor;
	}
}