using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RacingGameManager : NetworkBehaviour
{
	public static RacingGameManager Instance { get; private set; }

	[SyncObject]
	[SerializeField]
	public readonly SyncList<RacingPlayer> players = new();

	[SyncObject]
	public readonly SyncList<Contestant> contestants = new();

	private bool isRoundStarted = false;

	public override void OnStartNetwork()
	{
		base.OnStartNetwork();
		Instance = this;

		if (IsServer)
		{
			contestants.Add(new Contestant(0, "Car", "Racing/Contestants/car"));
			contestants.Add(new Contestant(1, "Morgan", "Racing/Contestants/morgan"));
			contestants.Add(new Contestant(2, "Old Guy", "Racing/Contestants/oldman"));
			contestants.Add(new Contestant(3, "Stroller", "Racing/Contestants/stroller"));
			contestants.Add(new Contestant(4, "Truck", "Racing/Contestants/truck3"));
			contestants.Add(new Contestant(5, "Dino", "Racing/Contestants/dino"));
			RacingUIManager.Instance.bettingUI.ActivateHostControls();
			isRoundStarted = false;
			//maybe revise this to allow for remote hosting
			StartBetting();
		}
		else
		{
			//StartCoroutine(CheckIfHost());
		}

		LoadContestantImages();
	}

	[ServerRpc(RequireOwnership = false)]
	public void StartRound()
	{
		Debug.Log("Game started...");
		isRoundStarted = true;

		StartRoundClientRpc();

		RacingUIManager.Instance.raceUI.UpdateContestantPositions(new ContestantPositionData(new float[6], RacingUIManager.Instance.raceUI.startingYPositions)); //reset position
		RaceManager.Instance.StartGame();
	}

	[Server]
	public void StopRound()
	{
		Debug.Log("Game ended...");
		isRoundStarted = false;
		List<Contestant> contestantPlacements = RaceManager.Instance.GetContestantPlacements();
		var bettingResults = BetManager.Instance.GetBetResults(contestantPlacements);
		SendBettingResultsClientRpc(bettingResults);

		BetManager.Instance.ResetBets();
		StopRoundClientRpc();
	}

	[ServerRpc(RequireOwnership = false)]
	public void AddPlayerServerRpc(RacingPlayer player)
	{
		players.Add(player);
		CheckIfPlayerIsHost(player);
		SetContestantPositions(player.Owner, RacingUIManager.Instance.raceUI.GetContestantPositionData());

		if (!isRoundStarted)
			player.ActivateBetting();
	}

	[ServerRpc(RequireOwnership = false)]
	public void RemovePlayerServerRpc(RacingPlayer player)
	{
		players.Remove(player);

		if (players.Count == 0)
		{
			BetManager.Instance.ResetBets();
			RacingUIManager.Instance.raceUI.UpdateContestantPositions(new ContestantPositionData(new float[6], RacingUIManager.Instance.raceUI.startingYPositions)); //reset position
		}
	}

	[ObserversRpc(BufferLast = true)]
	public void StartRoundClientRpc()
	{
		RacingPlayer.Instance.RoundStart();
	}

	[ObserversRpc(BufferLast = true)]
	public void StopRoundClientRpc()
	{
		RacingPlayer.Instance.RoundStop();
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
				returnString += $"{PlayerIDToName(betResult.originalBet.playerID)} can give out <color=green>{betResult.drinkAmount}</color> sips!\n";
			}
			else
			{
				returnString += $"{PlayerIDToName(betResult.originalBet.playerID)} must drink <color=red>{betResult.drinkAmount}</color> sips!\n";
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

	[ObserversRpc(BufferLast = true)]
	public void StartBetting()
	{
		RacingUIManager.Instance.bettingUI.ActivateBetting();
	}

	private void LoadContestantImages()
	{
		var ui = RacingUIManager.Instance.raceUI;

		for (int i = 0; i < contestants.Count; i++)
		{
			ui.contestants[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(contestants[i].imagePath);
		}
	}

	[Server]
	private void CheckIfPlayerIsHost(RacingPlayer player)
	{
		if (player.PlayerName.Contains("Host"))
		{
			ActivateHostControlsTargetRpc(player.Owner);
			//player.ActivateHostControls();
		}
	}

	[TargetRpc]
	private void ActivateHostControlsTargetRpc(NetworkConnection conn)
	{
		RacingPlayer.Instance.ActivateHostControls();
		RacingUIManager.Instance.bettingUI.ActivateHostControls();
	}

	[TargetRpc]
	private void SetContestantPositions(NetworkConnection conn, ContestantPositionData posData)
	{
		RacingUIManager.Instance.raceUI.UpdateContestantPositions(posData);
	}

	IEnumerator CheckIfHost()
	{
		Debug.Log("Waiting to check for host status");
		yield return new WaitForSeconds(0.3f);
		Debug.Log("Checking for host...");

		if (RacingPlayer.Instance.PlayerName.Contains("Host"))
		{
			Debug.Log("We are host!");
			RacingUIManager.Instance.bettingUI.ActivateHostControls();
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
