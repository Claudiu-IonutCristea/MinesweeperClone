using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(GridManagerSqr))]
public class SqrGridManagerEditor : GridManagerEditor
{
	GridManagerSqr _gridManager;

	public override void OnInspectorGUI()
	{
		_gridManager = (GridManagerSqr)target;

		base.OnInspectorGUI();
	}

	public override Action GenerateDebugGrid => _gridManager.GenerateDebugGrid;

	public override Action DeleteDebugGrid => _gridManager.DeleteDebugGrid;

	public override Action<bool> UpdateDebug => _gridManager.UpdateDebug;
}
