using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTile : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;

	public int row;
	public int column;

	public TileObject tileObject;

	public bool IsThisTileSelectable()
	{
		if (tileObject != null && tileObject.ownerID == GameManager.instance.currentPlayerID)
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
		if (tileObject != null && tileObject.ownerID == GameManager.instance.currentPlayerID &&
			(destinationGridTile.tileObject == null || 
			destinationGridTile.tileObject != null && !destinationGridTile.tileObject.isObstacle))
		{
			return true;
		}
		return false;
	}
	public bool MakeAction(List<GridTile> availableTiles, GridTile destinationGridTile)
	{
		if (availableTiles.Contains(destinationGridTile))
		{
			tileObject.transform.position = destinationGridTile.transform.position;
			destinationGridTile.tileObject = tileObject;
			tileObject = null;
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
