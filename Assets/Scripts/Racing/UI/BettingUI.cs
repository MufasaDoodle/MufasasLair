using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BettingUI : MonoBehaviour
{
	public TMP_Dropdown contestantDropdown;
	public TMP_InputField betAmountField;
	public Toggle autobetToggle;
	public Button placeBetButton;
	public Button startGameButton;
	public TextMeshProUGUI placedBetsText;

	private void Start()
	{
		placedBetsText.text = string.Empty;

		placeBetButton.interactable = false;
		startGameButton.interactable = false;

		placeBetButton.onClick.AddListener(() =>
		{
			PlaceBet();
		});

		startGameButton.onClick.AddListener(() =>
		{
			StartGame();
		});
	}

	public void ActivateHostControls()
	{
		startGameButton.interactable = true;
	}

	public void ActivateBetting()
	{
		placeBetButton.interactable = true;
	}

	public void DeactivateBetting()
	{
		placeBetButton.interactable = false;
	}

	public void SetPlacedBetsText(string text)
	{
		placedBetsText.text = text;
	}

	public void PlaceBet()
	{
		if (!placeBetButton.interactable)
		{
			return;
		}

		int contestantIndex = contestantDropdown.value;
		if (contestantIndex >= 0)
		{
			Debug.LogError("Invalid contestant index");
			return;
		}

		int betAmount = int.Parse(betAmountField.text);
		if (betAmount < 0 || betAmount > 10)
		{
			Debug.LogError("Invalid bet amount");
			return;
		}

		if (autobetToggle.isOn)
		{
			//TODO make an rpc call on betManager
			//basically we want to save a list of the players who have autobets on, and just automatically place the same bet for them
			//if they ever turn it off, then we activate their betting again
		}

		BetManager.Instance.PlaceBetServerRpc(new Bet(RacingPlayer.Instance.OwnerId, contestantIndex, betAmount));
	}

	public void StartGame()
	{
		RacingGameManager.Instance.StartRound();
	}
}
