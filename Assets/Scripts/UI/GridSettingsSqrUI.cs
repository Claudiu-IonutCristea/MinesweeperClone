using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public enum GridSizeType
{
	Width,
	Height,
}

public class GridSettingsSqrUI : GridSettingsBase
{
	private SliderInt _gridWidthSlider;
	private SliderInt _gridHeightSlider;
	private IntegerField _gridWidthValue;
	private IntegerField _gridHeightValue;

	public override void Awake()
	{
		base.Awake();

		_gridWidthSlider = _root.Q<SliderInt>("GridSizeWidthSlider");
		_gridHeightSlider = _root.Q<SliderInt>("GridSizeHeightSlider");
		_gridWidthValue = _root.Q<IntegerField>("GridSizeWidthValue");
		_gridHeightValue = _root.Q<IntegerField>("GridSizeHeightValue");
	}

	public override void Show()
	{
		base.Show();

		GameManager.Instance.gridWidth = _gridWidthValue.value;
		GameManager.Instance.gridHeight = _gridHeightValue.value;

		_gridWidthSlider.RegisterValueChangedCallback(evt => OnGridSizeSliderChanged(evt, GridSizeType.Width));
		_gridHeightSlider.RegisterValueChangedCallback(evt => OnGridSizeSliderChanged(evt, GridSizeType.Height));

		_gridWidthValue.RegisterValueChangedCallback(evt => OnGridSizeValueChanged(evt, GridSizeType.Width));
		_gridHeightValue.RegisterValueChangedCallback(evt => OnGridSizeValueChanged(evt, GridSizeType.Height));

		_gridWidthSlider.RegisterValueChangedCallback(evt => OnGameDifficultyCustom());
		_gridHeightSlider.RegisterValueChangedCallback(evt => OnGameDifficultyCustom());

		_gridWidthValue.RegisterValueChangedCallback(evt => OnGameDifficultyCustom());
		_gridHeightValue.RegisterValueChangedCallback(evt => OnGameDifficultyCustom());
	}

	public override void Hide()
	{
		base.Hide();

		_gridWidthSlider.UnregisterValueChangedCallback(evt => OnGridSizeSliderChanged(evt, GridSizeType.Width));
		_gridHeightSlider.UnregisterValueChangedCallback(evt => OnGridSizeSliderChanged(evt, GridSizeType.Height));

		_gridWidthValue.UnregisterValueChangedCallback(evt => OnGridSizeValueChanged(evt, GridSizeType.Width));
		_gridHeightValue.UnregisterValueChangedCallback(evt => OnGridSizeValueChanged(evt, GridSizeType.Height));


		_gridWidthSlider.UnregisterValueChangedCallback(evt => OnGameDifficultyCustom());
		_gridHeightSlider.UnregisterValueChangedCallback(evt => OnGameDifficultyCustom());

		_gridWidthValue.UnregisterValueChangedCallback(evt => OnGameDifficultyCustom());
		_gridHeightValue.UnregisterValueChangedCallback(evt => OnGameDifficultyCustom());
	}

	private protected override int MaxBombCount
	{
		get
		{
			var width = _gridWidthValue.value;
			var height = _gridHeightValue.value;
			return width * height - 1;
		}
	}

	private protected override GridType GridTileType => GridType.Square;

	private protected override void GameDifficultyChanged(GameDifficulty difficulty)
	{
		switch(difficulty)
		{
			case GameDifficulty.Easy:
				SetGameDifficultyWithoutNotify(10, 10, 10);
				break;
			case GameDifficulty.Medium:
				SetGameDifficultyWithoutNotify(13, 15, 40);
				break;
			case GameDifficulty.Hard:
				SetGameDifficultyWithoutNotify(30, 16, 99);
				break;
			default:
				break;
		}
	}

	private void SetGameDifficultyWithoutNotify(int width, int height, int bombCount)
	{
		_bombCountSlider.SetValueWithoutNotify((float)bombCount / MaxBombCount);
		_gridWidthSlider.SetValueWithoutNotify(width);
		_gridHeightSlider.SetValueWithoutNotify(height);

		_bombCountValue.SetValueWithoutNotify(bombCount);
		_gridWidthValue.SetValueWithoutNotify(width);
		_gridHeightValue.SetValueWithoutNotify(height);

		_bombCoverageText.text = ((float)bombCount / MaxBombCount).ToString("P1");

		GameManager.Instance.gridWidth = width;
		GameManager.Instance.gridHeight = height;
		GameManager.Instance.minesCount = bombCount;
	}

	//Grid Dimensions
	private void OnGridSizeSliderChanged(ChangeEvent<int> evt, GridSizeType gridSizeType)
	{
		switch(gridSizeType)
		{
			case GridSizeType.Width:
				GameManager.Instance.gridWidth = evt.newValue;
				_gridWidthValue.SetValueWithoutNotify(evt.newValue);
				break;
			case GridSizeType.Height:
				GameManager.Instance.gridHeight = evt.newValue;
				_gridHeightValue.SetValueWithoutNotify(evt.newValue);
				break;
			default:
				throw new ArgumentException("Invalid grid size type!", nameof(gridSizeType));
		}

		_bombCountValue.value = Mathf.RoundToInt(MaxBombCount * _bombCountSlider.value);
	}

	private void OnGridSizeValueChanged(ChangeEvent<int> evt, GridSizeType gridSizeType)
	{
		switch(gridSizeType)
		{
			case GridSizeType.Width:
				GameManager.Instance.gridWidth = evt.newValue;
				_gridWidthSlider.SetValueWithoutNotify(evt.newValue);
				break;
			case GridSizeType.Height:
				GameManager.Instance.gridHeight = evt.newValue;
				_gridHeightSlider.SetValueWithoutNotify(evt.newValue);
				break;
			default:
				throw new ArgumentException("Invalid grid size type!", nameof(gridSizeType));
		}

		_bombCountValue.value = Mathf.RoundToInt(MaxBombCount * _bombCountSlider.value);
	}
}
