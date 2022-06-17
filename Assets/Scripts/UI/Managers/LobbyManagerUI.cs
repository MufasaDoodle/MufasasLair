using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManagerUI : NetworkBehaviour
{
	public GameObject PlayerCardsPanel;

	GameObject[] playerSlots;

	private void Awake()
	{
		playerSlots = new GameObject[12];
		for (int i = 0; i < 12; i++)
		{
			playerSlots[i] = PlayerCardsPanel.transform.GetChild(i).gameObject;
		}
	}

	[ObserversRpc]
	public void RefreshClientRpc(Player[] players)
	{
		int index = 0;
		for (int i = 0; i < players.Length; i++)
		{
			playerSlots[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = players[i].playerName;
			playerSlots[i].GetComponentInChildren<Toggle>().isOn = players[i].isReady;
			index = i;
		}
		index++;

		if(index < playerSlots.Length)
		{
			for (int i = index; i < playerSlots.Length; i++)
			{
				playerSlots[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Empty slot...";
				playerSlots[i].GetComponentInChildren<Toggle>().isOn = false;
			}
		}
	}
}
