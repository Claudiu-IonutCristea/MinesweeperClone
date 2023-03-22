using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class GridManagerEditor: Editor
{
	private bool _debugGrid;
	private bool _oldDebugGrid;

	public abstract Action GenerateDebugGrid { get; }
	public abstract Action DeleteDebugGrid { get; }
	public abstract Action<bool> UpdateDebug { get; }

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		BaseGridManager gridManager = (BaseGridManager)target;
		gridManager.debugGrid = _debugGrid;
		

		//Spread bombs
		GUILayout.BeginHorizontal();

		EditorGUILayout.PrefixLabel("Spread Bombs");
		gridManager.spreadBombs = EditorGUILayout.Toggle(gridManager.spreadBombs);

		if(gridManager.spreadBombs)
		{
			gridManager.maxSpreadTries = EditorGUILayout.IntField(gridManager.maxSpreadTries);
			EditorGUILayout.LabelField("Tries");
		}

		GUILayout.EndHorizontal();

		//Bomb count

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Max Bomb Count");
		EditorGUILayout.LabelField(gridManager.MaxBombCount.ToString());
		EditorGUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();

		var coverage = (float)gridManager.bombCount / gridManager.MaxBombCount;

		EditorGUILayout.PrefixLabel($"Bomb Count ({coverage:P1})");
		gridManager.bombCount = EditorGUILayout.IntSlider(gridManager.bombCount, 0, gridManager.MaxBombCount);

		GUILayout.EndHorizontal();

		EditorGUILayout.Space();

		//Debug grid BUTTON
		GUILayout.BeginHorizontal();

		
		EditorGUILayout.PrefixLabel("Debug Grid");
		_debugGrid = GUILayout.Toggle(_debugGrid, "");
		if(EditorApplication.isPlaying && _oldDebugGrid != _debugGrid)
		{
			_oldDebugGrid = _debugGrid;
			UpdateDebug(_debugGrid);
		}

		GUILayout.EndHorizontal();

		//generate debug grid BUTTON and delete debug grid BUTTON
		if(!EditorApplication.isPlaying)
		{
			EditorGUILayout.BeginHorizontal();

			if(GUILayout.Button("Generate Debug Grid"))
			{
				GenerateDebugGrid();
			}

			if(GUILayout.Button("Delete Debug Grid"))
			{
				DeleteDebugGrid();
			}

			EditorGUILayout.EndHorizontal();
		}
	}
}
