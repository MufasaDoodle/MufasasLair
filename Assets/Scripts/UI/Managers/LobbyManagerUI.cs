using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManagerUI : MonoBehaviour
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

	public void RefreshClientRpc(Player[] players)
	{
		int index = 0;
		for (int i = 0; i < players.Length; i++)
		{
			playerSlots[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = players[i].playerName;
			playerSlots[i].GetComponentInChildren<Toggle>().isOn = players[i].isReady;
			index = i;

			//TODO figure out a way to enable the kicking of clients
			/*
			if (Player.LocalInstance.IsHost)
			{
				if (i == 0) continue; //don't put a kick button on the host
				PlayerCardsPanel.transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
				PlayerCardsPanel.transform.GetChild(i).GetChild(2).GetComponent<Button>().onClick.AddListener(() => KickClient(i));
			}
			*/
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

	/*
	public void KickClient(int id)
	{
		if (!IsHost) return;
		GameManager.Instance.KickClient(id);
	}
	*/
}
