using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceManager : NetworkBehaviour
{
	public static RaceManager Instance { get; private set; }

	private RaceUI raceUI;

	private float finishLineXPos;

	public override void OnStartNetwork()
	{
		base.OnStartNetwork();

		Instance = this;
		raceUI = RacingUIManager.Instance.raceUI;
	}

	[Server]
	public void StartGame()
	{
		finishLineXPos = raceUI.finishLine.transform.position.x;

		StartCoroutine(RunGame());
	}

	[Server]
	private IEnumerator RunGame()
	{
		while (!CheckIfRaceOver()) //some condition
		{
			yield return new WaitForSeconds(0.07f);
			GameTick();
		}

		RacingGameManager.Instance.StopRound(); //end game
	}

	[Server]
	private void GameTick()
	{
		var posData = raceUI.GetContestantPositionData();

		float[] newContestantXPos = new float[posData.contestantXPos.Length];
		float[] newContestantYPos = new float[posData.contestantXPos.Length];

		for (int i = 0; i < posData.contestantXPos.Length; i++)
		{
			float positionIncrease = Random.Range(0, 8);
			newContestantXPos[i] = posData.contestantXPos[i] + positionIncrease;
			newContestantYPos[i] = posData.contestantYPos[i];
		}

		//UpdateContestantPositionsClientRpc(new ContestantPositionData(newContestantPosData));
		raceUI.UpdateContestantPositions(new ContestantPositionData(newContestantXPos, newContestantYPos)); //update the contestant positions
		UpdateContestantPlacementClientRpc(GetContestantPlacements());
	}

	[ObserversRpc(BufferLast = true)]
	public void UpdateContestantPositionsClientRpc(ContestantPositionData pos)
	{
		raceUI.UpdateContestantPositions(pos); //update the contestant positions
	}

	[ObserversRpc(BufferLast = true)]
	public void UpdateContestantPlacementClientRpc(List<Contestant> contestants)
	{
		string placements = "";

		for (int i = 0; i < contestants.Count; i++)
		{
			placements += $"{i+1}) {contestants[i].contestantName}\n";
		}

		RacingUIManager.Instance.raceStatusUI.SetRaceStatusText(placements);
	}

	[Server]
	private bool CheckIfRaceOver()
	{
		var posData = raceUI.GetContestantPositionData();

		foreach (var contestantXPos in posData.contestantXPos)
		{
			if (contestantXPos >= finishLineXPos) return true;
		}

		return false;
	}

	public List<Contestant> GetContestantPlacements()
	{
		var posData = raceUI.GetContestantPositionData();
		var contestants = RacingGameManager.Instance.contestants.ToList();

		List<ContestantAndPosition> contestantAndPositions = new List<ContestantAndPosition>();

		List<Contestant> contestantsOrdered = new();

		for (int i = 0; i < contestants.Count; i++)
		{
			contestantAndPositions.Add(new ContestantAndPosition(posData.contestantXPos[i], i));
		}

		var result = from contestant in contestantAndPositions
					 orderby contestant.offset
					 select contestant;

		var list = result.ToList();
		list.Reverse();

		for (int i = 0; i < list.Count; i++)
		{
			contestantsOrdered.Add(contestants.Find(c => c.id == list[i].id));
		}

		//RETURN
		return contestantsOrdered;
	}
}

public struct ContestantAndPosition
{
	public float offset;
	public int id;

	public ContestantAndPosition(float offset, int id)
	{
		this.offset = offset;
		this.id = id;
	}
}
