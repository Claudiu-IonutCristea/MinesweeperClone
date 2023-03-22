using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridValueChangedEventArgs : EventArgs
{
	public GridTile Tile { get; set; }

	public bool DebugValues { get; set; }

	public bool PreviewTile { get; set; } = false;

	public bool GameOver { get; set; } = false;
}
