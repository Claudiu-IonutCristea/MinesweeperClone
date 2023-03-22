using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct AxialVector : ICoordSystem<AxialVector>
{
	public AxialVector(int q, int r)
	{
		Q = q;
		R = r;
	}

	public int Q { get; set; }
	public int R { get; set; }


	private static readonly float _sqrt3 = 1.73205080757f;
	private static readonly float _sqrt3div2 = 0.86602540378f;

	private static readonly AxialVector[] _neighbours =
	{
		new(1, 0), new(1, -1), new(0, -1),
		new(-1, 0), new(-1, 1), new(0, 1),
	};


	private static readonly AxialVector _zero = new(0, 0);
	public static AxialVector Zero { get => _zero; }


	public AxialVector GetNeighbour(int dir)
		=> new(Q + _neighbours[dir].Q, R + _neighbours[dir].R);

	public Vector2 CoordSystemToPoint()
	{
		var x = _sqrt3 * this.Q + _sqrt3div2 * this.R;
		var y = 						1.5f * this.R;
		return new(x, y);
	}

	public AxialVector Scale(int scaleFactor)
		=> new(Q * scaleFactor, R * scaleFactor);

	public override int GetHashCode() => System.HashCode.Combine(Q, R);
	public override string ToString() => $"({Q}, {R}, {-Q - R})";
}
