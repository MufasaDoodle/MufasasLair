using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStatusUI : MonoBehaviour
{
	public TMPro.TextMeshProUGUI raceStatusText;

	private void Start()
	{
		raceStatusText.text = string.Empty;
	}

	public void SetRaceStatusText(string text)
	{
		raceStatusText.text = text;
	}
}
