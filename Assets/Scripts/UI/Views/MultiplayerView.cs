using FishNet;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerView : View
{
	[SerializeField]
	private Button hostButton;

	[SerializeField]
	private Button connectButton;

	[SerializeField]
	private Button backButton;

	[SerializeField]
	private TMP_InputField usernameText;

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
			InstanceFinder.ClientManager.StartConnection();
		});

		backButton.onClick.AddListener(() =>
		{
			SaveUsername();
			ViewManager.Instance.Show<ShooterMainMenu>();
		});
		usernameText.text = PlayerPrefs.GetString("username");


		base.Initialize();
	}

	private void SaveUsername()
	{
		PlayerPrefs.SetString("username", usernameText.text);
		PlayerPrefs.Save();
	}
}
