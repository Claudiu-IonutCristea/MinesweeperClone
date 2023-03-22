using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqrGrid : BaseGrid<SquareVector>
{

	private readonly int _width;
	private readonly int _height;

	public SqrGrid(int width, int height) : base()
	{
		_width = width;
		_height = height;
	}

	public override int NeighbourCount => 8;

	public override void GenerateGrid()
	{
		for(int y = 0; y < _height; y++)
		{
			for(int x = 0; x < _width; x++)
			{
				_grid.Add(new SquareVector(x, y), new GridTile());
			}
		}
	}
}
