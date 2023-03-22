using System;

public class GridTile
{
	private int _value;
	private int _flagCycle;
	private bool _isValueVisible;

	public GridTile()
	{
		_value = 0;
		_flagCycle = 0;
		_isValueVisible = false;
	}

	public Action ShowValue;

	public int Value
	{
		get => _value;
		set => _value = value;
	}
	public bool HasBomb => _value == -1;
	public bool HasFlag => _flagCycle % 3 == 1;
	public bool HasQuestion => _flagCycle % 3 == 2;
	public bool IsValueVisible
	{
		get => _isValueVisible;
		set => _isValueVisible = value;
	}

	public void PlantBomb()
	{
		_value = -1;
	}

	public void CycleFlag()
	{
		_flagCycle++;
	}
}
