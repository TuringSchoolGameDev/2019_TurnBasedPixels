
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo
{
	public int rowCount;
	public int columnCount;
	public List<Vector3> obstacles = new List<Vector3>();
	public List<Vector3> player0Objects = new List<Vector3>();
	public List<Vector3> player1Objects = new List<Vector3>();
}


public class TileGridHelpers
{
	public static Dictionary<Vector2Int, int> GetGridMapForMovement(List<GridTile> gridTiles)
	{
		Dictionary<Vector2Int, int> result = new Dictionary<Vector2Int, int>();
		foreach (GridTile gridTile in gridTiles)
		{
			result.Add(gridTile.coords, TileGridIsOccupiedByObstacle(gridTile) ? 1 : 0);
		}
		return result;
	}

	public static bool TileGridIsOccupiedBySomething(GridTile gridTile)
	{
		if (gridTile.tileObject != null)
		{
			return true;
		}
		return false;
	}
	public static bool TileGridIsOccupiedByObstacle(GridTile gridTile)
	{
		if (gridTile.tileObject != null && gridTile.tileObject.isObstacle)
		{
			return true;
		}
		return false;
	}
	public static bool TileGridIsOccupiedByPickup(GridTile gridTile)
	{
		if (gridTile.tileObject != null && gridTile.tileObject.isPickup)
		{
			return true;
		}
		return false;
	}

	public static bool TileGridIsOccupiedByMyUnit(GridTile gridTile, int ownerID)
	{
		if (gridTile.tileObject != null && gridTile.tileObject.ownerID == ownerID)
		{
			return true;
		}
		return false;
	}
	public static bool TileGridIsOccupiedByEnemy(GridTile gridTile, int ownerID)
	{
		if (gridTile.tileObject != null && gridTile.tileObject.ownerID > -1 && gridTile.tileObject.ownerID != ownerID)
		{
			return true;
		}
		return false;
	}
}

public class CustomFloat
{
	public float customFloat { get; set; }
}