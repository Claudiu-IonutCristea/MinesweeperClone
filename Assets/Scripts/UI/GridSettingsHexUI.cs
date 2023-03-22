using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GridSettingsHexUI : GridSettingsBase
{
	private SliderInt _gridRadiusSlider;
	private IntegerField _gridRadiusValue;

	public override void Awake()
	{
		base.Awake();

		_gridRadiusSlider = _root.Q<SliderInt>("GridRadiusSlider");
		_gridRadiusValue = _root.Q<IntegerField>("GridRadiusValue");
		_gridRadiusValue.value = _gridRadiusSlider.value;
		_bombCountValue.value = Mathf.RoundToInt(_bombCountSlider.value * MaxBombCount);
	}

	public override void Show()
	{
		base.Show();

		GameManager.Instance.gridRadius = _gridRadiusValue.value;


		_gridRadiusSlider.RegisterValueChangedCallback(evt => OnGridRadiusSliderChanged(evt));
		_gridRadiusValue.RegisterValueChangedCallback(evt => OnGridRadiusValueChanged(evt));
	}

	public override void Hide()
	{
		base.Hide();


		_gridRadiusSlider.UnregisterValueChangedCallback(evt => OnGridRadiusSliderChanged(evt));
		_gridRadiusValue.UnregisterValueChangedCallback(evt => OnGridRadiusValueChanged(evt));

	}

	private protected override int MaxBombCount
	{
		get
		{
			var radius = _gridRadiusValue.value;
			return 1 + 3 * radius * ( radius + 1 );
		}
	}
	private protected override GridType GridTileType => GridType.Hexagonal;

	private protected override void GameDifficultyChanged(GameDifficulty difficulty)
	{
		switch(difficulty)
		{
			case GameDifficulty.Easy:
				SetGameDifficultyWithoutNotify(5, 15);
				break;
			case GameDifficulty.Medium:
				SetGameDifficultyWithoutNotify(10, 75);
				break;
			case GameDifficulty.Hard:
				SetGameDifficultyWithoutNotify(15, 150);
				break;
		}
	}
	
	private void SetGameDifficultyWithoutNotify(int radius, int bombCount)
	{
		_bombCountSlider.SetValueWithoutNotify((float)bombCount / MaxBombCount);
		_gridRadiusSlider.SetValueWithoutNotify(radius);

		_bombCountValue.SetValueWithoutNotify(bombCount);
		_gridRadiusValue.SetValueWithoutNotify(radius);

		_bombCoverageText.text = ( (float)bombCount / MaxBombCount ).ToString("P1");

		GameManager.Instance.gridRadius = radius;
		GameManager.Instance.minesCount = bombCount;
	}

	//Grid Radius
	private void OnGridRadiusSliderChanged(ChangeEvent<int> evt)
	{
		GameManager.Instance.gridRadius = evt.newValue;
		_gridRadiusValue.SetValueWithoutNotify(evt.newValue);

		_bombCountValue.value = Mathf.RoundToInt(MaxBombCount * _bombCountSlider.value);
	}
	private void OnGridRadiusValueChanged(ChangeEvent<int> evt)
	{
		GameManager.Instance.gridRadius = evt.newValue;
		_gridRadiusSlider.SetValueWithoutNotify(evt.newValue);

		_bombCountValue.value = Mathf.RoundToInt(MaxBombCount * _bombCountSlider.value);
	}

}
