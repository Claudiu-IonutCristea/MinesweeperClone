using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManagerSqr : BaseGridManager<SqrGrid, SquareVector>
{
	[Range(0, 20)]
	public int width;
	[Range(0, 20)]
	public int height;

	public override int MaxBombCount => width * height;
	public override int FlagCount => grid.Tiles.Where(tile => tile.HasFlag).Count();
	public override Func<Vector3, SquareVector> PointToCoordSystemFunc => vector3 => vector3.PointToSquareVector();
	public override Func<SquareVector, Vector3> CoordSystemToPointFunc => sqrVector => sqrVector.CoordSystemToPoint();
	public override Func<SqrGrid> NewGridFunc => () => new SqrGrid(width, height);

	public override void Awake()
	{
		base.Awake();

		if(GameManager.Instance != null)
		{
			width = GameManager.Instance.gridWidth;
			height = GameManager.Instance.gridHeight;
		}
	}

	public override void Start()
	{
		base.Start();

		_camera.transform.position = new(width / 2, height / 2, _camera.transform.position.z);
	}
}
