using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingUIManager : MonoBehaviour
{
    public static RacingUIManager Instance { get; private set; }

	public RaceUI raceUI;
	public BettingUI bettingUI;
	public BetResultsUI resultsUI;
	public RaceStatusUI raceStatusUI;

	private void Awake()
	{
		Instance = this;
	}
}
