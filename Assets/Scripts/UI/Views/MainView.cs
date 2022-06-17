using FishNet;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainView : View
{
	[SerializeField]
	private Button disconnectButton;

	[SerializeField]
	private TextMeshProUGUI infoText;

	[SerializeField]
	private TextMeshProUGUI scoreText;

	[SerializeField]
	private TextMeshProUGUI playerCountText;

	private void LateUpdate()
	{
		if (!IsInitialized) return;

		infoText.text = $"Is Server = {InstanceFinder.IsServer}, is client = {InstanceFinder.IsClient}, is host = {InstanceFinder.IsHost}";

		scoreText.text = $"Score: {Player.LocalInstance.Score}";

		if (InstanceFinder.IsHost)
		{
			playerCountText.text = $"Players: {InstanceFinder.ServerManager.Clients.Count}";

			playerCountText.gameObject.SetActive(true);
		}
		else
		{
			playerCountText.gameObject.SetActive(false);
		}
	}

	public override void Initialize()
	{
		disconnectButton.onClick.AddListener(() =>
		{
			if (InstanceFinder.IsServer)
			{
				InstanceFinder.ServerManager.StopConnection(true);
			}
			else if (InstanceFinder.IsClient)
			{
				InstanceFinder.ClientManager.StopConnection();
			}
		});

		base.Initialize();
	}
}
