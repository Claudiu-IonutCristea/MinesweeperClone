using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI : UIBase
{
	private BaseGridManager _gridManager;

	private Label _mineCountText;
	private Label _timerText;

	private Button _backButton;
	private Button _newGameButton;

	private Timer _timer;

	public override void Awake()
	{
		base.Awake();

		_mineCountText = _root.Q<Label>("MineCountText");
		_timerText = _root.Q<Label>("TimerDisplay");

		_backButton = _root.Q<Button>("BackToMenuButton");
		_newGameButton = _root.Q<Button>("NewGameButton");

		_timer = new();
		_timer.Stop();
	}

	public override void Show()
	{
		_mineCountText.text = _gridManager.bombCount.ToString();
		_newGameButton.text = "New Game";

		_gridManager.OnGridValueChanged += UpdateMineCount;
		_gridManager.OnGameOver += GameOver;

		_backButton.clicked += BackToMenu;
		_newGameButton.clicked += NewGame;

		_timer = new();

		base.Show();
	}

	public override void Hide()
	{
		_gridManager.OnGridValueChanged -= UpdateMineCount;
		_gridManager.OnGameOver -= GameOver;

		_backButton.clicked -= BackToMenu;
		_newGameButton.clicked -= NewGame;

		_timer.Stop();

		base.Hide();
	}

	public void FindGridManager()
	{
		_gridManager = FindObjectOfType<BaseGridManager>();
	}

	private void UpdateMineCount(object sender, GridValueChangedEventArgs e)
	{
		int flags = _gridManager.FlagCount;
		int bombs = _gridManager.bombCount;

		_mineCountText.text = (bombs - flags).ToString();
	}

	private void GameOver(object sender, bool hasWon)
	{
		_newGameButton.text = hasWon ? "You WON!" : "You LOST!";
		_timer.Stop();
	}

	private void BackToMenu()
	{
		GameManager.Instance.ToMainMenu();
		_timer.Stop();
	}

	private void NewGame()
	{
		GameManager.Instance.PlayGame();
		_timer = new();
	}

	private void LateUpdate()
	{
		if(!_timer.IsStopped)
			_timerText.text = _timer.Elapsed.ToString(@"hh\:mm\:ss");
	}
}
