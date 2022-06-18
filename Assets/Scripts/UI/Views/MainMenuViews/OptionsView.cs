using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsView : View
{
    [SerializeField]
    private Button saveExitButton;

    [SerializeField]
    private Slider fpsSlider;

    [SerializeField]
    private TextMeshProUGUI fpsText;

	public override void Initialize()
	{
        saveExitButton.onClick.AddListener(() => SaveAndExit());
        fpsSlider.onValueChanged.AddListener(FPSSliderChanged);

        var maxFps = PlayerPrefs.GetInt("maxFPS");

        if(maxFps == 0)
		{
            maxFps = 144;
            PlayerPrefs.SetInt("maxFPS", maxFps);
        }

        fpsSlider.value = maxFps;
        FPSSliderChanged(maxFps);

		base.Initialize();
	}

    private void FPSSliderChanged(float value)
	{
        fpsText.text = value.ToString();
	}

    private void SaveAndExit()
	{
        //TODO save settings to playerprefs
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = (int)fpsSlider.value;
        PlayerPrefs.SetInt("maxFPS", (int)fpsSlider.value);

        ViewManager.Instance.Show<MainMenu>();
    }
}
