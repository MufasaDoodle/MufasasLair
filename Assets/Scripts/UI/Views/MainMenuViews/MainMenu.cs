using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : View
{
	[SerializeField]
	private Button selectGameButton;

	[SerializeField]
	private Button optionsButton;

	[SerializeField]
	private Button quitButton;


	private void OpenView<T>() where T : View
	{
		ViewManager.Instance.Show<T>();
	}

	public override void Initialize()
	{
		selectGameButton.onClick.AddListener(() => OpenView<SelectGameView>());
		optionsButton.onClick.AddListener(() => OpenView<OptionsView>());
		quitButton.onClick.AddListener(() => Application.Quit());

		var maxFps = PlayerPrefs.GetInt("maxFPS");

		if (maxFps <= 0)
		{
			maxFps = 144;
			PlayerPrefs.SetInt("maxFPS", maxFps);
		}

		Application.targetFrameRate = maxFps;

		base.Initialize();
	}
}
