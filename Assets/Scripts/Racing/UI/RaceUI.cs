using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceUI : MonoBehaviour
{
    public GameObject[] contestants;


	public ContestantPositionData GetContestantPositionData()
	{
		float[] contestantsXPos = new float[contestants.Length];

		for (int i = 0; i < contestants.Length; i++)
		{
			Vector3 currPos = contestants[i].transform.position;
			contestantsXPos[i] = currPos.x;
		}

		return new ContestantPositionData(contestantsXPos);
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
			Vector3 newPos = new Vector3(posData.contestantXPos[i], currPos.y, currPos.z);
			contestants[i].transform.position = newPos;
		}
	}
}
