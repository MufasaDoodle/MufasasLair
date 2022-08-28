using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct ContestantPositionData
{
	public float[] contestantXPos;
	public float[] contestantYPos;

	public ContestantPositionData(float[] contestantXPos, float[] contestantYPos)
	{
		this.contestantXPos = contestantXPos;
		this.contestantYPos = contestantYPos;
	}
}
