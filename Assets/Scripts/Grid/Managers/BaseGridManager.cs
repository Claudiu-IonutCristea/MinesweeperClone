using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseGridManager : MonoBehaviour
{
	public event EventHandler<GridValueChangedEventArgs> OnGridValueChanged;
	public event EventHandler<PreviewTileEventArgs> OnPreviewTile;
	public event EventHandler<bool> OnGameOver;
	public event EventHandler<bool> OnDebugChanged;

	//Private fields
	//Serialized
	[SerializeField] protected GameObject _tilePrefab;
	[SerializeField] protected Camera _camera;

	//Public Fields
	public Sprite[] tileValueSprites;
	public Sprite bombSprite;
	public Sprite flagSprite;
	public Sprite questionSprite;
	public Sprite crossSprite;

	[HideInInspector] public int bombCount;
	[HideInInspector] public int maxSpreadTries;
	[HideInInspector] public bool debugGrid;
	[HideInInspector] public bool spreadBombs;
	[HideInInspector] public bool gameOver;

	public virtual int FlagCount { get; }
	public abstract int MaxBombCount { get; }

	private protected void GridValueChanged(BaseGridManager senderGrid, GridValueChangedEventArgs args) => OnGridValueChanged?.Invoke(senderGrid, args);
	private protected void PreviewTile(BaseGridManager senderGrid, PreviewTileEventArgs args) => OnPreviewTile?.Invoke(senderGrid, args);
	private protected void GameOver(BaseGridManager senderGrid, bool isWin) => OnGameOver?.Invoke(senderGrid, isWin);
	private protected void DebugChanged(BaseGridManager senderGrid, bool debug) => OnDebugChanged?.Invoke(senderGrid, debug);
}

[RequireComponent(typeof(PlayerInput))]
public abstract class BaseGridManager<TGrid, TCoordSystem> : BaseGridManager
	where TCoordSystem : struct, ICoordSystem<TCoordSystem>
	where TGrid : BaseGrid<TCoordSystem>
{
	private bool _bombsPlanted;
	private GridTile _tilePreview;
	private List<GridTile> _tilesPreview;

	private CameraMovement _cameraMovement;

	public TGrid grid;


	public abstract Func<Vector3, TCoordSystem> PointToCoordSystemFunc { get; }
	public abstract Func<TCoordSystem, Vector3> CoordSystemToPointFunc { get; }
	public abstract Func<TGrid> NewGridFunc { get; }


	public virtual void Awake()
	{
		_tilesPreview = new();
		_cameraMovement = _camera.GetComponent<CameraMovement>();

		if(GameManager.Instance != null)
		{
			bombCount = GameManager.Instance.minesCount;
		}
	}

	public virtual void Start()
	{
		InitialiseGrid();
	}


	public virtual void UpdateDebug(bool debug)
	{
		debugGrid = debug;
		DebugChanged(this, debugGrid);
	}

	public virtual void GenerateDebugGrid()
	{
		InitialiseGrid();
		PlantBombs(default, bombCount);
		SetTileValues();
		InitialiseTileObjectSprites();
	}

	public virtual void DeleteDebugGrid()
	{
		for(int i = transform.childCount; i > 0; i--)
		{
			var child = transform.GetChild(0).GetComponent<TileObject>();
			child.Delete();
		}
	}

	public virtual void InitialiseGrid()
	{
		for(int i = transform.childCount; i > 0; i--)
		{
			var child = transform.GetChild(0).GetComponent<TileObject>();
			child.Delete();
		}

		grid = NewGridFunc();
		grid.GenerateGrid();
		foreach(var pair in grid)
		{
			GameObject go = Instantiate(_tilePrefab, CoordSystemToPointFunc(pair.Key), Quaternion.identity, transform);
			var tile = go.GetComponent<TileObject>();
			tile.tile = pair.Value;
			tile.gridManager = this;
			go.name = "Tile " + pair.Key.ToString();
		}
	}

	public virtual void GameEnded(bool hasWon)
	{
		if(gameOver)
			return;

		gameOver = true;
		foreach(var hex in grid.Tiles)
		{
			GameOver(this, hasWon);
		}

		Debug.Log("Game OVER!");
		Debug.Log("Player " + ( hasWon ? "WON" : "LOST" ));
	}

	public virtual void SetTileShowValueFunction()
	{
		foreach(var pair in grid)
		{
			var hex = pair.Value;
			if(hex.Value == 0)
			{
				hex.ShowValue = () =>
				{
					if(hex.HasQuestion)
						hex.CycleFlag();

					hex.IsValueVisible = true;
					GridValueChanged(this, new GridValueChangedEventArgs { Tile = hex, DebugValues = debugGrid });
					for(int i = 0; i < grid.NeighbourCount; i++)
					{
						if(grid.TryGetValue(pair.Key.GetNeighbour(i), out GridTile neighbour))
						{
							if(!neighbour.IsValueVisible)
							{
								if(neighbour.HasFlag)
									continue;

								neighbour.ShowValue();
								GridValueChanged(this, new GridValueChangedEventArgs { Tile = neighbour, DebugValues = debugGrid });
							}
						}
					}

					if(IsGameWon())
						GameEnded(true);
				};
			}
			else
			{
				hex.ShowValue = () =>
				{
					if(hex.HasQuestion)
						hex.CycleFlag();

					hex.IsValueVisible = true;
					GridValueChanged(this, new GridValueChangedEventArgs { Tile = hex, DebugValues = debugGrid });

					if(IsGameWon())
						GameEnded(true);
				};
			}
		}
	}

	public virtual void PlantBombs(TCoordSystem firstTileVector, int bombCount)
	{
		_bombsPlanted = true;
		if(bombCount > grid.Count)
		{
#if UNITY_EDITOR
			Debug.LogError($"There are not enough tiles for this number of bombs! \n TileCount: {grid.Count} \n BombCount: {bombCount}");
			EditorApplication.isPlaying = false;
#endif
			throw new ArgumentException($"There are not enough tiles for this number of bombs! \n TileCount: {grid.Count} \n BombCount: {bombCount}");
		}

		var vectors = grid.Vectos.ToList();
		var bombNearby = false;
		var tries = maxSpreadTries;

		while(bombCount > 0)
		{
			var rndIndex = UnityEngine.Random.Range(0, grid.Count);


			if(vectors[rndIndex].Equals(firstTileVector) || grid[vectors[rndIndex]].HasBomb)
				continue;

			if(spreadBombs)
			{
				bombNearby = false;

				if(tries > 0)
				{
					for(int i = 0; i < grid.NeighbourCount; i++)
					{

						var neighbourVector = vectors[rndIndex].GetNeighbour(i);

						if(!grid.Contains(neighbourVector))
							continue;

						if(grid[neighbourVector].HasBomb)
						{
							bombNearby = true;
							break;
						}
					}
				}

				if(bombNearby)
				{
					tries--;
					continue;
				}
				tries = maxSpreadTries;
			}

			grid[vectors[rndIndex]].PlantBomb();
			bombCount--;
		}
	}

	public virtual void SetTileValues()
	{
		foreach(var pair in grid)
		{
			var vector = pair.Key;

			if(grid[vector].HasBomb)
				continue;

			for(int i = 0; i < grid.NeighbourCount; i++)
			{
				var neighbourVector = vector.GetNeighbour(i);
				if(!grid.Contains(neighbourVector))
					continue;

				if(grid[neighbourVector].HasBomb)
				{
					grid[vector].Value++;
				}
			}
		}
	}

	public virtual void InitialiseTileObjectSprites()
	{
		foreach(var tile in GetComponentsInChildren<TileObject>())
		{
			tile.InitialiseValueSprite();
		}
	}

	public virtual KeyValuePair<TCoordSystem, GridTile> GetPairFromMousePos()
	{
		var point = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		var vector = PointToCoordSystemFunc(point);

		if(!grid.Contains(vector))
			return new();

		return new(vector, grid[vector]);
	}

	public virtual bool IsGameWon()
	{
		//If there are ANY tiles that are NOT visible and DON'T have a bomb
		//method returns false ( inverts the Any() )
		return !grid.Tiles.Any(tile => !tile.IsValueVisible && !tile.HasBomb);
	}


	#region Input Events
#pragma warning disable IDE0051

	private void OnShowTileValue()
	{
		if(gameOver || _cameraMovement.drag)
			return;

		var pair = GetPairFromMousePos();
		var vector = pair.Key;
		var tile = pair.Value;

		if(tile == null || tile.IsValueVisible || tile.HasFlag)
			return;

		if(_bombsPlanted == false)
		{
			PlantBombs(vector, bombCount);
			SetTileValues();
			InitialiseTileObjectSprites();
			SetTileShowValueFunction();
			tile.ShowValue();
		}
		else
		{
			tile.ShowValue();
		}

		if(tile.HasBomb)
			GameEnded(false);

	}

	private void OnCycleFlag()
	{
		if(gameOver)
			return;

		var pair = GetPairFromMousePos();
		var tile = pair.Value;

		if(tile == null || tile.IsValueVisible)
			return;

		tile.CycleFlag();
		GridValueChanged(this, new GridValueChangedEventArgs { Tile = tile, DebugValues = debugGrid });
	}

	private void OnShowNeighbours()
	{
		if(gameOver)
			return;

		var pair = GetPairFromMousePos();

		if(_tilePreview == pair.Value)
			return;

		var tempTilesPreview = new List<GridTile>() { pair.Value };
		for(int i = 0; i < grid.NeighbourCount; i++)
		{
			var neighbourVector = pair.Key.GetNeighbour(i);

			if(!grid.Contains(neighbourVector))
				continue;

			tempTilesPreview.Add(grid[neighbourVector]);
		}

		//Debug.Log("Turned OFF " + _hexesPreview.Except(_tempHexesPreview).Count() + " Tiles");
		foreach(var tile in _tilesPreview.Except(tempTilesPreview))
		{
			PreviewTile(this, new() { Tile = tile, Preview = false });
		}

		//Debug.Log("Turned ON " + _tempHexesPreview.Except(_hexesPreview).Count() + " Tiles");
		foreach(var tile in tempTilesPreview.Except(_tilesPreview))
		{
			PreviewTile(this, new() { Tile = tile, Preview = true });
		}

		_tilesPreview = tempTilesPreview;
	}

	private void OnShowNeighboursOff()
	{
		if(gameOver)
			return;

		//var pair = GetPairFromMousePos();
		//Debug.Log("Turned OFF " + _hexesPreview.Count + " Tiles");
		foreach(var tile in _tilesPreview)
		{
			PreviewTile(this, new() { Tile = tile, Preview = false });
		}
		_tilesPreview.Clear();
	}

	void OnCameraMove()
	{
		_cameraMovement.CameraMove();
	}

	void OnCameraMoveStop()
	{
		_cameraMovement.StopCameraMove();
	}

	void OnCameraZoom()
	{
		//_cameraMovement.Zoom(Mouse.current.scroll.ReadValue());
		_cameraMovement.Zoom(Mouse.current.scroll.ReadValue().normalized.y);
	}
#pragma warning restore IDE0051
	#endregion
}
