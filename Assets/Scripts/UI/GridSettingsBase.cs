using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class GridSettingsBase : UIBase
{
	private protected RadioButtonGroup _gameDifficultyRadioGroup;

	private protected Slider _bombCountSlider;
	private protected IntegerField _bombCountValue;
	private protected Label _bombCoverageText;

	private protected Button _playButton;
	private protected Button _backButton;

	public override void Awake()
	{
		base.Awake();

		_gameDifficultyRadioGroup = _root.Q<RadioButtonGroup>("DifficultyRadioGroup");

		_bombCountSlider = _root.Q<Slider>("BombCountSlider"); //0.1f - 0.8f
		_bombCountValue = _root.Q<IntegerField>("BombCountValue");
		_bombCoverageText = _root.Q<Label>("BombCoveragePercentageText");
		_bombCoverageText.text = _bombCountSlider.value.ToString("P1");

		_playButton = _root.Q<Button>("PlayButton");
		_backButton = _root.Q<Button>("BackButton");
	}

	public override void Show()
	{

		GameManager.Instance.minesCount = _bombCountValue.value;
		GameManager.Instance.gridType = GridTileType;

		_gameDifficultyRadioGroup.RegisterValueChangedCallback(evt => OnGameDifficultyChanged(evt));

		_bombCountSlider.RegisterValueChangedCallback(evt => OnBombCountSliderChanged(evt));
		_bombCountValue.RegisterValueChangedCallback(evt => OnBombCountValueChanged(evt));

		_bombCountSlider.RegisterValueChangedCallback(evt => OnGameDifficultyCustom());
		_bombCountValue.RegisterValueChangedCallback(evt => OnGameDifficultyCustom());

		_backButton.clicked += OnBackButton;
		_playButton.clicked += OnPlayButton;

		_gameDifficultyRadioGroup.value = 0; //Easy

		base.Show();

	}

	public override void Hide()
	{
		_gameDifficultyRadioGroup.UnregisterValueChangedCallback(evt => OnGameDifficultyChanged(evt));

		_bombCountSlider.UnregisterValueChangedCallback(evt => OnBombCountSliderChanged(evt));
		_bombCountValue.UnregisterValueChangedCallback(evt => OnBombCountValueChanged(evt));

		_bombCountSlider.UnregisterValueChangedCallback(evt => OnGameDifficultyCustom());
		_bombCountValue.UnregisterValueChangedCallback(evt => OnGameDifficultyCustom());

		_backButton.clicked -= OnBackButton;
		_playButton.clicked -= OnPlayButton;

		base.Hide();
	}

	private protected abstract int MaxBombCount { get; }
	private protected abstract GridType GridTileType { get; }

	//Game Difficulty
	private protected abstract void GameDifficultyChanged(GameDifficulty difficulty);
	private void OnGameDifficultyChanged(ChangeEvent<int> evt)
	{
		var difficulty =  evt.newValue switch
		{
			0 => GameDifficulty.Easy,
			1 => GameDifficulty.Medium,
			2 => GameDifficulty.Hard,
			_ => GameDifficulty.Custom,
		};
		GameManager.Instance.gameDifficulty = difficulty;

		GameDifficultyChanged(difficulty);
	}
	private protected void OnGameDifficultyCustom()
	{
		_gameDifficultyRadioGroup.value = -1;
	}

	//Bomb Count
	private void OnBombCountSliderChanged(ChangeEvent<float> evt)
	{
		_bombCountValue.SetValueWithoutNotify(Mathf.RoundToInt(MaxBombCount * evt.newValue));
		_bombCoverageText.text = evt.newValue.ToString("P1");
		GameManager.Instance.minesCount = _bombCountValue.value;
	}
	private void OnBombCountValueChanged(ChangeEvent<int> evt)
	{
		GameManager.Instance.minesCount = evt.newValue;
		_bombCountSlider.SetValueWithoutNotify((float)evt.newValue / MaxBombCount);
		_bombCoverageText.text = _bombCountSlider.value.ToString("P1");
	}

	//Buttons
	private void OnBackButton()
	{
		UIManager.Instance.ChangeUserInterface(UIManager.Instance.mainMenuUI);
	}

	private void OnPlayButton()
	{
		GameManager.Instance.PlayGame();
	}
}
