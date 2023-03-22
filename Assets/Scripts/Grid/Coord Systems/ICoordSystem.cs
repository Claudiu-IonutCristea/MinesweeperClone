using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICoordSystem<TCoordSystem>
	where TCoordSystem : struct
{
	TCoordSystem GetNeighbour(int dir);

	TCoordSystem Scale(int scaleFactor);

	Vector2 CoordSystemToPoint();
}
