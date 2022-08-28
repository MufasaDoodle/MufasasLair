using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BetManager : NetworkBehaviour
{
	public static BetManager Instance { get; private set; }

	[SyncObject]
	[SerializeField]
	public readonly SyncList<Bet> betList = new();

	public override void OnStartNetwork()
	{
		base.OnStartNetwork();
		Instance = this;
	}

	[ServerRpc(RequireOwnership = false)]
	public void PlaceBetServerRpc(Bet bet)
	{
		if (betList.Any(b => b.playerID == bet.playerID))
		{
			Debug.LogError("Player " + bet.playerID + " is trying to add more than one bet");
			return;
		}
		betList.Add(bet);
		UpdateBettingUIClientRpc(betList.ToList()); //update all player clients with updated betlist
		Debug.Log($"Player {bet.playerID} has placed a bet on {bet.contestantID} worth {bet.betAmount} sips");
	}

	[ObserversRpc(BufferLast = true, IncludeOwner = true)]
	public void UpdateBettingUIClientRpc(List<Bet> bets)
	{
		string returnString = "";
		Debug.Log("Number of bets: " + bets.Count);

		foreach (var bet in bets)
		{
			string playerName = RacingGameManager.Instance.PlayerIDToName(bet.playerID);
			string contestantName = RacingGameManager.Instance.contestants[bet.contestantID].contestantName;
			returnString += $"{playerName} has bet {bet.betAmount} on {contestantName}\n";
		}

		RacingUIManager.Instance.bettingUI.SetPlacedBetsText(returnString);
	}

	[Server]
	public void ResetBets()
	{
		betList.Clear();
		UpdateBettingUIClientRpc(betList.ToList());
		Debug.Log("Bets cleared");
	}

	[Server]
	public List<BetResult> GetBetResults(List<Contestant> contestantRanking)
	{
		List<BetResult> results = new();

		//iterate over all the bets and get the results of each one
		for (int i = 0; i < betList.Count; i++)
		{
			Bet bet = betList[i];

			int drinkAmount = bet.betAmount;
			bool wonBet = false;
			if (bet.contestantID == contestantRanking[0].id)
			{
				wonBet = true;
			}
			else
			{
				//finding the index of the bet's contestant placement and adding it onto their bet amount
				int contestantPlacement = contestantRanking.FindIndex(c => c.id == bet.contestantID);
				drinkAmount += contestantPlacement;
			}

			results.Add(new BetResult(bet, wonBet, drinkAmount));
		}

		betList.Clear();
		return results;
	}

	[ServerRpc(RequireOwnership = false)]
	public void RemoveBetFromPlayerServerRpc(int playerID)
	{
		foreach (var bet in betList)
		{
			if(bet.playerID == playerID)
			{
				betList.Remove(bet);
			}
		}
		UpdateBettingUIClientRpc(betList.ToList());
	}
}

public struct Bet
{
	public int playerID;
	public int contestantID;
	public int betAmount;

	public Bet(int playerID, int contestantID, int betAmount)
	{
		this.playerID = playerID;
		this.contestantID = contestantID;
		this.betAmount = betAmount;
	}
}

public struct BetResult
{
	public Bet originalBet;
	public bool hasWon;
	public int drinkAmount;

	public BetResult(Bet originalBet, bool hasWon, int drinkAmount)
	{
		this.originalBet = originalBet;
		this.hasWon = hasWon;
		this.drinkAmount = drinkAmount;
	}
}
