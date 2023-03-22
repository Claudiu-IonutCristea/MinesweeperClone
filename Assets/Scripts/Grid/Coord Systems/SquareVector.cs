using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SquareVector : ICoordSystem<SquareVector>
{
	public SquareVector(int x, int y)
	{
		X = x;
		Y = y;
	}

	public int X { get; set; }
	public int Y { get; set; }

	private static readonly SquareVector[] _neighbours =
	{
		new(-1,  1), new(0,  1), new(1,  1),
		new(-1,  0),             new(1,  0),
		new(-1, -1), new(0, -1), new(1, -1),
	};
	private static readonly SquareVector _zero = new(0, 0);

	public static SquareVector Zero { get => _zero; }

	public Vector2 CoordSystemToPoint() => new(X, Y);

	public SquareVector GetNeighbour(int dir)
		=> new(X + _neighbours[dir].X, Y + _neighbours[dir].Y);

	public SquareVector Scale(int scaleFactor)
		=> new(X * scaleFactor, Y * scaleFactor);

	public override int GetHashCode() => System.HashCode.Combine(X, Y);
	public override string ToString() => $"({X}, {Y})";
}
