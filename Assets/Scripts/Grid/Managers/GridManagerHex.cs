using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManagerHex : BaseGridManager<HexGrid, AxialVector>
{
	[Range(0, 10)]
	public int gridRadius;

	public override int MaxBombCount => 1 + 3 * gridRadius * ( gridRadius + 1 );
	public override int FlagCount => grid.Tiles.Where(tile => tile.HasFlag).Count();
	public override Func<Vector3, AxialVector> PointToCoordSystemFunc => vector3 => vector3.PointToAxialVector();
	public override Func<AxialVector, Vector3> CoordSystemToPointFunc => axialVector => axialVector.CoordSystemToPoint();
	public override Func<HexGrid> NewGridFunc => () => new HexGrid(gridRadius);

	public override void Awake()
	{
		base.Awake();

		if(GameManager.Instance != null)
		{
			gridRadius = GameManager.Instance.gridRadius;
		}
	}
}
