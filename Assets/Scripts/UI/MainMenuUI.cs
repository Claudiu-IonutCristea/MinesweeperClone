using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

public class MainMenuUI : UIBase
{
	private Button _gridHexButton;
	private Button _gridSqrButton;
	private Button _quitButton;

	public override void Awake()
	{
		base.Awake();

		_gridHexButton = _root.Q<Button>("HexagonalGridButton");
		_gridSqrButton = _root.Q<Button>("SquareGridButton");
		_quitButton = _root.Q<Button>("QuitButton");
	}

	public override void Show()
	{
		_gridHexButton.clicked += OnGridHexButton;
		_gridSqrButton.clicked += OnGridSqrButton;
		_quitButton.clicked += OnQuitButton;

		base.Show();
	}

	public override void Hide()
	{
		_gridHexButton.clicked -= OnGridHexButton;
		_gridSqrButton.clicked -= OnGridSqrButton;
		_quitButton.clicked -= OnQuitButton;

		base.Hide();
	}

	private void OnGridHexButton()
	{
		UIManager.Instance.ChangeUserInterface(UIManager.Instance.gridSettingsHexUI);
	}

	private void OnGridSqrButton()
	{
		UIManager.Instance.ChangeUserInterface(UIManager.Instance.gridSettingsSqrUI);
	}

	private void OnQuitButton()
	{

#if UNITY_EDITOR
		Debug.Log("Quitting App!");
#endif

		Application.Quit();
	}
}
