using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceUI : MonoBehaviour
{
    public GameObject[] contestants;

	public GameObject finishLine;

	public float[] startingYPositions;

	private void Start()
	{
		startingYPositions = new float[6];
		for (int i = 0; i < contestants.Length; i++)
		{
			startingYPositions[i] = contestants[i].transform.position.y;
		}
	}

	public ContestantPositionData GetContestantPositionData()
	{
		float[] contestantsXPos = new float[contestants.Length];
		float[] contestantsYPos = new float[contestants.Length];

		for (int i = 0; i < contestants.Length; i++)
		{
			Vector3 currPos = contestants[i].transform.position;
			contestantsXPos[i] = currPos.x;
			contestantsYPos[i] = currPos.y;
		}

		return new ContestantPositionData(contestantsXPos, contestantsYPos);
	}

    public void UpdateContestantPositions(ContestantPositionData posData)
	{
		if(posData.contestantXPos.Length != contestants.Length)
		{
			Debug.LogError("Contestant-list lengths do not match");
			return;
		}

		for (int i = 0; i < contestants.Length; i++)
		{
			Vector3 currPos = contestants[i].transform.position;
			Vector3 newPos = new Vector3(posData.contestantXPos[i], posData.contestantYPos[i], currPos.z);
			contestants[i].transform.position = newPos;
		}
	}
}
