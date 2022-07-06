using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : NetworkBehaviour
{
	public static RaceManager Instance { get; private set; }

	public override void OnStartNetwork()
	{
		base.OnStartNetwork();

		Instance = this;
	}

	public List<Contestant> GetContestantPlacements()
	{
		Debug.LogError("TODO");
		return new List<Contestant>();
	}
}
