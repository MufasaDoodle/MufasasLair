using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShooterMainMenu : View
{
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button optionsButton;

    [SerializeField]
    private Button backButton;

	public override void Initialize()
	{
        playButton.onClick.AddListener(() => ViewManager.Instance.Show<LobbyView>());

        optionsButton.onClick.AddListener(() => ViewManager.Instance.Show<ShooterOptionsView>());

        backButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));

		base.Initialize();
	}
}
