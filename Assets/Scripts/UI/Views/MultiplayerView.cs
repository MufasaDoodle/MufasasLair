using FishNet;
using System.Collections;
using System.Collections.Generic;
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

	public override void Initialize()
	{
		hostButton.onClick.AddListener(() =>
		{
			InstanceFinder.ServerManager.StartConnection();
			InstanceFinder.ClientManager.StartConnection();
		});

		connectButton.onClick.AddListener(() => InstanceFinder.ClientManager.StartConnection());

		backButton.onClick.AddListener(() => ViewManager.Instance.Show<ShooterMainMenu>());


		base.Initialize();
	}
}
