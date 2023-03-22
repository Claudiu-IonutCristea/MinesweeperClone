using System;
public class PreviewTileEventArgs : EventArgs
{
	public GridTile Tile { get; set; }
	public bool Preview { get; set; }
}
