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

	Bet prevBet;

	private void Start()
	{
		placedBetsText.text = string.Empty;

		placeBetButton.interactable = false;
		startGameButton.interactable = false;
		autobetToggle.isOn = false;

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

	public void DeactivateHostControls()
	{
		startGameButton.interactable = false;
	}

	public void ActivateBetting()
	{
		if (autobetToggle.isOn)
		{
			AutoPlaceBet();
			return;
		}

		placeBetButton.interactable = true;
	}

	public void DeactivateBetting()
	{
		placeBetButton.interactable = false;
	}

	public void SetPlacedBetsText(string text)
	{
		Debug.Log("Recieved placed bets update:\n" + text);
		placedBetsText.text = text;
	}

	public void PlaceBet()
	{
		if (!placeBetButton.interactable)
		{
			return;
		}

		int contestantIndex = contestantDropdown.value;
		if (contestantIndex < 0)
		{
			Debug.LogError("Invalid contestant index: " + contestantDropdown);
			return;
		}

		int betAmount = int.Parse(betAmountField.text);
		if (betAmount < 0 || betAmount > 10)
		{
			Debug.LogError("Invalid bet amount");
			return;
		}

		Bet bet = new Bet(RacingPlayer.Instance.OwnerId, contestantIndex, betAmount);

		prevBet = bet;

		BetManager.Instance.PlaceBetServerRpc(bet);

		DeactivateBetting();
	}

	private void AutoPlaceBet()
	{
		BetManager.Instance.PlaceBetServerRpc(prevBet);
		DeactivateBetting();
	}

	public void StartGame()
	{
		RacingGameManager.Instance.StartRound();
	}
}
