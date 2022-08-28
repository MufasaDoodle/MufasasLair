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

		placeBetButton.interactable = true;
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
		Debug.Log("Activating betting");
		if (autobetToggle.isOn)
		{
			if(prevBet.contestantID == 0 && prevBet.betAmount == 0) //fsr the network does not recognize autobet should be off when first joining, so we have this check here
			{
				Debug.Log("Making bet button available");
				autobetToggle.isOn = false;
				placeBetButton.interactable = true;
				return;
			}

			AutoPlaceBet();
			return;
		}

		placeBetButton.interactable = true;
	}

	public void DeactivateBetting()
	{
		Debug.Log("Deactivating betting");
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

		Debug.Log("Placing bet");

		Bet bet = new Bet(RacingPlayer.Instance.OwnerId, contestantIndex, betAmount);

		prevBet = bet;

		Debug.Log("Placing bet!");
		BetManager.Instance.PlaceBetServerRpc(bet);

		DeactivateBetting();
	}

	private void AutoPlaceBet()
	{
		Debug.Log("Auto placing bet!");
		BetManager.Instance.PlaceBetServerRpc(prevBet);
		DeactivateBetting();
	}

	public void StartGame()
	{
		RacingGameManager.Instance.StartRound();
	}
}
