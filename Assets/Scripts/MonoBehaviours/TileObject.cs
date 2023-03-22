using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
	private bool _isInDebug;

	[SerializeField] SpriteRenderer _tileRenderer;
	[SerializeField] SpriteRenderer _flagRenderer;
	[SerializeField] SpriteRenderer _valueRenderer;

	[HideInInspector] public GridTile tile;
	[HideInInspector] public BaseGridManager gridManager;

	void Start()
    {
		gridManager.OnGridValueChanged += UpdateTile;
		gridManager.OnDebugChanged += DebugChanged;
		gridManager.OnPreviewTile += PreviewTile;
		gridManager.OnGameOver += GameOver;

		HideValue();
	}

	public void InitialiseValueSprite()
	{
		_valueRenderer.sprite = tile.HasBomb ? gridManager.bombSprite : gridManager.tileValueSprites[tile.Value];
	}

	public void Delete()
	{
		gridManager.OnGridValueChanged -= UpdateTile;
		gridManager.OnDebugChanged -= DebugChanged;
		gridManager.OnPreviewTile -= PreviewTile;
		gridManager.OnGameOver -= GameOver;
#if UNITY_EDITOR
		DestroyImmediate(gameObject);
#else
		Destroy(gameObject);
#endif
	}

	#region Event Methods

	private void UpdateTile(object sender, GridValueChangedEventArgs e)
	{
		if(e.Tile != tile)
			return;

		if(tile.IsValueVisible)
		{
			ShowValue();
		}
		else if(tile.HasFlag)
		{
			_flagRenderer.sprite = gridManager.flagSprite;
		}
		else if(tile.HasQuestion)
		{
			_flagRenderer.sprite = gridManager.questionSprite;
		}
		else
		{
			_flagRenderer.sprite = null;
		}
	}

	private void PreviewTile(object sender, PreviewTileEventArgs e)
	{
		if(tile.IsValueVisible || e.Tile != tile)
			return;

		if(e.Preview)
		{
			_tileRenderer.color = Color.white;
		}
		else
		{
			if(_isInDebug)
			{
				ShowValueDebug();
			}
			else
			{
				HideValue();
			}
		}
	}

	private void GameOver(object sender, bool hasWon)
	{
		if(tile.HasFlag || tile.HasQuestion)
		{
			ShowValueDebug();
			_tileRenderer.color = Color.white;

			if(!tile.HasBomb)
			{
				_flagRenderer.sprite = gridManager.crossSprite;
				_flagRenderer.color = Color.white - new Color(0f, 0f, 0f, .5f);
			}
		}
		else if(tile.HasBomb)
		{
			ShowValue();
		}
	}

	private void DebugChanged(object sender, bool debug)
	{
		if(tile.IsValueVisible)
			return;

		_isInDebug = debug;

		if(_isInDebug)
			ShowValueDebug();
		else
			HideValue();
	}

	#endregion

	private void HideValue()
	{
		var color = _valueRenderer.color;
		_valueRenderer.color = new Color(color.r, color.g, color.b, 0f);
		_tileRenderer.color = new Color(.5f, .5f, .5f);
	}
	private void ShowValueDebug()
	{
		var color = _valueRenderer.color;
		_valueRenderer.color = new Color(color.r, color.g, color.b, .5f);
		_tileRenderer.color = new Color(.75f, .75f, .75f);
	}
	private void ShowValue()
	{
		var color = _valueRenderer.color;
		_valueRenderer.color = new Color(color.r, color.g, color.b, 1f);
		_tileRenderer.color = new Color(1f, 1f, 1f);
		_flagRenderer.sprite = null;
	}
}
