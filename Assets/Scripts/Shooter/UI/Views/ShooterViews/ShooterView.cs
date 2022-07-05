using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShooterView : View
{
	[SerializeField]
	private TextMeshProUGUI healthText;

	[SerializeField]
	private TextMeshProUGUI ammoText;

	private void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void OnDisable()
	{
		Cursor.lockState = CursorLockMode.None;
	}

	private void Update()
	{
		if (!IsInitialized) return;

		Player player = Player.LocalInstance;

		if(player == null || player.controlledPlayer == null) return;

		healthText.text = $"{player.controlledPlayer.health} HP";
		ammoText.text = $"{player.controlledPlayer.ammo} / 5";
	}
}
