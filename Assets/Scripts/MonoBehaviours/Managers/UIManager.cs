using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance { get; private set; }

	public MainMenuUI mainMenuUI;
	public GridSettingsHexUI gridSettingsHexUI;
	public GridSettingsSqrUI gridSettingsSqrUI;
	public InGameUI inGameUI;

	private UIBase _activeScreen;

	private void Awake()
	{
		
		if(Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}

		_activeScreen = mainMenuUI;
		_activeScreen.Show();

	}

	public void ChangeUserInterface(UIBase newScreen)
	{
		_activeScreen.Hide();
		newScreen.Show();
		_activeScreen = newScreen;
	}
}
