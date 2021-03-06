using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using FishNet;

public class ShooterLobbyView : View
{
	[SerializeField]
	private Button disconnectButton;

	[SerializeField]
	private Button startGameButton;

	[SerializeField]
	private Button readyButton;

	[SerializeField]
	private TMP_Dropdown mapSelect;

	[SerializeField]
	private Image mapImage;

	[SerializeField]
	private TextMeshProUGUI playerCountPrefText;

	[SerializeField]
	private TextMeshProUGUI mapDescriptionText;

	[SerializeField]
	private Toggle ffaToggle;

	[SerializeField]
	private TMP_InputField timeLimitInput;


	public override void Initialize()
	{
		disconnectButton.onClick.AddListener(() => SceneManager.LoadScene("ShooterMenu"));
		startGameButton.onClick.AddListener(() => StartGameClicked());
		readyButton.onClick.AddListener(() => ReadyButtonClicked());
		mapSelect.onValueChanged.AddListener(OnMapSelectionChanged);
		ffaToggle.onValueChanged.AddListener(OnFFAToggleChanged);
		timeLimitInput.onEndEdit.AddListener(OnTimeLimitChanged);

		List<string> mapOptions = new List<string>();
		foreach (var map in GameManager.Instance.maps)
		{
			mapOptions.Add(map.mapName);
		}

		mapSelect.AddOptions(mapOptions);

		SetMapDescription(GameManager.Instance.maps[GameManager.Instance.currentMap]);

		if (InstanceFinder.IsServer)
		{
			mapSelect.interactable = true;
			ffaToggle.interactable = true;
			timeLimitInput.interactable = true;
		}

		base.Initialize();
	}

	private void Update()
	{
		if (!IsInitialized || !InstanceFinder.IsServer) return;

		startGameButton.interactable = GameManager.Instance.canStart;
	}

	private void StartGameClicked()
	{
		GameManager.Instance.StartGame();
	}

	private void ReadyButtonClicked()
	{
		Player.LocalInstance.SetIsReadyServerRpc(!Player.LocalInstance.isReady);

		readyButton.GetComponent<Image>().color = Player.LocalInstance.isReady ? Color.red : Color.green;
	}

	private void OnMapSelectionChanged(int value)
	{
		if (!InstanceFinder.IsServer) return;

		GameManager.Instance.SelectMapServerRpc(value);
	}

	public void SetMapDescription(ShooterMap map)
	{
		playerCountPrefText.text = $"Players: {map.playerNum}";
		mapDescriptionText.text = map.description;
		mapImage.color = map.mapColor; //TODO Change from color to an image
		mapSelect.value = map.id;
	}

	private void OnFFAToggleChanged(bool value)
	{
		GameManager.Instance.GameSettingsChangedServerRpc(value, int.Parse(timeLimitInput.text));
	}

	private void OnTimeLimitChanged(string value)
	{
		GameManager.Instance.GameSettingsChangedServerRpc(ffaToggle.isOn, int.Parse(value));
	}

	public void NewGameSettings(bool ffa, int timeLimit)
	{
		ffaToggle.isOn = ffa;
		timeLimitInput.text = timeLimit.ToString();
	}
}
