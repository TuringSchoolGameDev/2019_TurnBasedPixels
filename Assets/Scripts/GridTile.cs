using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public Vector2Int coords;

	public TileObject tileObject;

	public bool IsThisTileSelectable(int ownerID)
	{
		if (TileGridHelpers.TileGridIsOccupiedByMyUnit(this, ownerID))
		{
			return true;
		}
		return false;
	}

	public void SelectTile()
	{
		spriteRenderer.color = Color.blue;
	}

	public void MakeTileAvailable()
	{
		spriteRenderer.color = Color.green;
	}

	public void DeselectTile()
	{
		spriteRenderer.color = Color.white;
	}

	public bool CanActionBeMade(GridTile destinationGridTile)
	{
		if (TileGridHelpers.TileGridIsOccupiedByEnemy(destinationGridTile, tileObject.ownerID) || !TileGridHelpers.TileGridIsOccupiedBySomething(destinationGridTile))
		{
			return true;
		}
		return false;
	}
	public bool MakeAction(List<GridTile> availableTiles, GridTile destinationGridTile)
	{
		if (availableTiles.Contains(destinationGridTile))
		{
			tileObject.MakeAction(this, destinationGridTile);
			return true;
		}
		return false;
	}

	public List<GridTile> GetAvailableTiles()
	{
		if (tileObject != null)
		{
			return tileObject.GetAvailableTiles(this);
		}
		return new List<GridTile>();
	}
}
