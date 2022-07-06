using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RacingGameManager : NetworkBehaviour
{
	public static RacingGameManager Instance { get; private set; }

	[SyncObject]
	[SerializeField]
	public readonly SyncList<RacingPlayer> players = new();

	[SyncObject]
	public readonly SyncList<Contestant> contestants = new();

	[SyncVar]
	public bool hasRoundStarted;

	public override void OnStartNetwork()
	{
		base.OnStartNetwork();
		Instance = this;

		if (IsServer)
		{
			SetGameStartBoolServerRpc(false);
			contestants.Add(new Contestant(0, "Car", "CarImage"));
			contestants.Add(new Contestant(1, "Morgan", "MorganImage"));
			contestants.Add(new Contestant(2, "Old Guy", "OldGuyImage"));
			contestants.Add(new Contestant(3, "Stroller", "StrollerImage"));
			contestants.Add(new Contestant(4, "Truck", "TruckImage"));
			contestants.Add(new Contestant(5, "Dino", "DinoImage"));
		}

		if (IsHost)
		{
			RacingUIManager.Instance.bettingUI.ActivateHostControls();
		}
	}

	[Server]
	public void StartRound()
	{
		Debug.Log("Game started...");
		SetGameStartBoolServerRpc(true);
	}

	[Server]
	public void StopRound()
	{
		Debug.Log("Game ended...");
		SetGameStartBoolServerRpc(false);
		//TODO there is more to do here
		List<Contestant> contestantPlacements = RaceManager.Instance.GetContestantPlacements();
		var bettingResults = BetManager.Instance.GetBetResults(contestantPlacements);
	}

	[ServerRpc(RequireOwnership = false)]
	public void AddPlayer(RacingPlayer player)
	{
		players.Add(player);
	}

	[ServerRpc(RequireOwnership = false)]
	public void RemovePlayer(RacingPlayer player)
	{
		players.Remove(player);
	}

	[ServerRpc(RequireOwnership = false)]
	private void SetGameStartBoolServerRpc(bool value)
	{
		hasRoundStarted = value;
	}

	[ObserversRpc(BufferLast = true)]
	public void SendBettingResultsClientRpc(List<BetResult> results)
	{
		string returnString = "";

		for (int i = 0; i < results.Count; i++)
		{
			BetResult betResult = results[i];
			//TODO add results to database

			if (betResult.hasWon)
			{
				returnString += $"{PlayerIDToName(betResult.originalBet.playerID)} can give out <color=green>{betResult.drinkAmount}</color> sips!";
			}
			else
			{
				returnString += $"{PlayerIDToName(betResult.originalBet.playerID)} must drink <color=red>{betResult.drinkAmount}</color> sips!";
			}
		}

		RacingUIManager.Instance.resultsUI.SetBettingResultsText(returnString);
	}

	public string PlayerIDToName(int id)
	{
		if (players.Any(p => p.OwnerId == id))
		{
			return players.Find(p => p.OwnerId == id).PlayerName;
		}
		else
		{
			Debug.Log($"Could not find player with ID of {id}");
			return null;
		}
	}
}

public struct Contestant
{
	public int id;
	public string contestantName;
	public string imagePath;

	public Contestant(int id, string contestantName, string imagePath)
	{
		this.id = id;
		this.contestantName = contestantName;
		this.imagePath = imagePath;
	}
}
