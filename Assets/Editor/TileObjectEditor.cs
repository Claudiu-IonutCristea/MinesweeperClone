using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileObject))]
public class TileObjectEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		EditorGUILayout.Space();

		var obj = (TileObject)target;
		var hex = obj.tile;

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Hex Values: ");

		if(hex == null)
		{
			EditorGUILayout.LabelField("NULL!");

			EditorGUILayout.EndHorizontal();
		}
		else
		{
			EditorGUILayout.LabelField("");
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			if(hex.HasBomb)
			{
				EditorGUILayout.PrefixLabel("Bomb");
				EditorGUILayout.LabelField(hex.HasBomb.ToString().ToUpper());
			}
			else
			{
				EditorGUILayout.PrefixLabel("Value");
				EditorGUILayout.LabelField(hex.Value.ToString());
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Flag");
			EditorGUILayout.LabelField(hex.HasFlag.ToString().ToUpper());
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Question Mark");
			EditorGUILayout.LabelField(hex.HasQuestion.ToString().ToUpper());
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Value Visible");
			EditorGUILayout.LabelField(hex.IsValueVisible.ToString().ToUpper());
			EditorGUILayout.EndHorizontal();
			}
	}
}
