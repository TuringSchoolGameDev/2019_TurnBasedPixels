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
	public void MakeAction(GridTile destinationGridTile)
	{
		tileObject.transform.position = destinationGridTile.transform.position;
		destinationGridTile.tileObject = tileObject;
		tileObject = null;
	}
}
