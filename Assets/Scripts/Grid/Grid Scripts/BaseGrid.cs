using System.Collections;
using System.Collections.Generic;

public abstract class BaseGrid<TCoordSystem> : IEnumerable<KeyValuePair<TCoordSystem, GridTile>>
	where TCoordSystem : struct, ICoordSystem<TCoordSystem>
{
	private protected readonly Dictionary<TCoordSystem, GridTile> _grid;

	public BaseGrid()
	{
		_grid = new();
	}


	public int Count => _grid.Count;
	public GridTile this[TCoordSystem vector] => _grid[vector];
	public Dictionary<TCoordSystem, GridTile>.KeyCollection Vectos => _grid.Keys;
	public Dictionary<TCoordSystem, GridTile>.ValueCollection Tiles => _grid.Values;

	public bool Contains(TCoordSystem vector) => _grid.ContainsKey(vector);
	public bool TryGetValue(TCoordSystem vector, out GridTile tile) => _grid.TryGetValue(vector, out tile);


	public abstract int NeighbourCount { get; }
	public abstract void GenerateGrid();


	public IEnumerator<KeyValuePair<TCoordSystem, GridTile>> GetEnumerator()
		=> _grid.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator()
		=> GetEnumerator();
}
