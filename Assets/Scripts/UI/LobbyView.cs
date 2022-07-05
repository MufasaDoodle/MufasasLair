using FishNet;
using FishNet.Transporting.Tugboat;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : View
{
	[SerializeField]
	private Button hostButton;

	[SerializeField]
	private Button connectButton;

	[SerializeField]
	private Button backButton;

	[SerializeField]
	private TMP_InputField usernameText;

	[SerializeField]
	private TMP_InputField ipText;

	[SerializeField]
	private TMP_InputField portText;

	public override void Initialize()
	{
		hostButton.onClick.AddListener(() =>
		{
			SaveUsername();
			InstanceFinder.ServerManager.StartConnection();
			InstanceFinder.ClientManager.StartConnection();
		});

		connectButton.onClick.AddListener(() =>
		{
			SaveUsername();
			SetIP();
			SetPort();
			InstanceFinder.ClientManager.StartConnection();
		});

		backButton.onClick.AddListener(() =>
		{
			SaveUsername();
			ViewManager.Instance.Show<ShooterMainMenu>();
		});
		usernameText.text = PlayerPrefs.GetString("username");

		//ipText.onValueChanged.AddListener(delegate { SetIP(); });


		base.Initialize();
	}

	private void SaveUsername()
	{
		PlayerPrefs.SetString("username", usernameText.text);
		PlayerPrefs.Save();
	}

	private void SetIP()
	{
		if(String.IsNullOrEmpty(ipText.text))
		{
			ipText.text = "localhost";
		}
		InstanceFinder.NetworkManager.GetComponent<Tugboat>().SetClientAddress(ipText.text);
	}

	private void SetPort()
	{
		InstanceFinder.NetworkManager.GetComponent<Tugboat>().SetPort(Convert.ToUInt16(portText.text));
	}
}
