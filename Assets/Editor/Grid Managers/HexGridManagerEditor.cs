using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManagerHex))]
public class HexGridManagerEditor : GridManagerEditor
{
	GridManagerHex _gridManager;

	public override void OnInspectorGUI()
	{
		_gridManager = (GridManagerHex)target;

		base.OnInspectorGUI();
	}

	public override Action GenerateDebugGrid => _gridManager.GenerateDebugGrid;

	public override Action DeleteDebugGrid => _gridManager.DeleteDebugGrid;

	public override Action<bool> UpdateDebug => _gridManager.UpdateDebug;
}
