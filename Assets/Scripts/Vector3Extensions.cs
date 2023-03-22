using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
	private static readonly float _1div3 = 0.33333333333f;
	private static readonly float _2div3 = 0.66666666667f;
	private static readonly float _sqrt3div3 = 0.57735026919f;

	public static AxialVector PointToAxialVector(this Vector3 point)
	{
		var fQ = _sqrt3div3 * point.x - _1div3 * point.y;
		var fR = _2div3 * point.y;

		var fS = -fQ - fR;

		var q = Mathf.RoundToInt(fQ);
		var r = Mathf.RoundToInt(fR);
		var s = Mathf.RoundToInt(fS);

		var qDiff = Mathf.Abs(q - fQ);
		var rDiff = Mathf.Abs(r - fR);
		var sDiff = Mathf.Abs(s - fS);

		if(qDiff > rDiff && qDiff > sDiff)
		{
			q = -r - s;
		}
		else if(rDiff > sDiff)
		{
			r = -q - s;
		}

		return new(q, r);
	}

	public static SquareVector PointToSquareVector(this Vector3 point)
		=> new((int)point.x, (int)point.y);
}
