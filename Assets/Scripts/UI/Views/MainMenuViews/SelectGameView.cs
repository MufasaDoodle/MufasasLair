using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectGameView : View
{
	[SerializeField]
	private Button shooterButton;

	[SerializeField]
	private Button backButton;

	public override void Initialize()
	{
		shooterButton.onClick.AddListener(() => SceneManager.LoadScene("ShooterMenu"));

		backButton.onClick.AddListener(() => ViewManager.Instance.Show<MainMenu>());

		base.Initialize();
	}
}
