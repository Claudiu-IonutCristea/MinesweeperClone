using System;
using System.Collections;
using System.Collections.Generic;

public class HexGrid : BaseGrid<AxialVector>
{
	private readonly int _gridRadius;
	
	public HexGrid(int gridRadius) : base()
	{
		_gridRadius = gridRadius;
	}

	public float GridRadius => _gridRadius;
	public override int NeighbourCount => 6;

	public override void GenerateGrid()
	{
		var vectors = GenerateSpiral(_gridRadius);
		foreach(var vec in vectors)
		{
			_grid.Add(vec, new GridTile());
		}
	}

	private List<AxialVector> GenerateSpiral(int radius)
	{
		var result = new List<AxialVector>() { AxialVector.Zero };
		for(int i = 1; i <= radius; i++)
		{
			result.AddRange(GenerateRing(i));
		}

		return result;
	}
	private List<AxialVector> GenerateRing(int radius)
	{
		var result = new List<AxialVector>();

		var vector = AxialVector.Zero.GetNeighbour(4).Scale(radius); //AxialVector.Direction(4) * radius;

		for(int i = 0; i < 6; i++)
		{
			for(int j = 0; j < radius; j++)
			{
				result.Add(vector);
				vector = vector.GetNeighbour(i);
			}
		}

		return result;
	}
}
