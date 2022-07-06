using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetResultsUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI betResultsText;

	private void Start()
	{
		betResultsText.text = string.Empty;
	}

	public void SetBettingResultsText(string text)
	{
		betResultsText.text = text;
	}
}
