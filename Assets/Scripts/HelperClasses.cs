﻿
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