using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileObject : MonoBehaviour
{
	public GridTile currentGridTile;

	//bool - true arba false (1 arba 0)
	public bool isObstacle;
	public int ownerID = -1;

	[SerializeField]
	private int _health;

	public int health
	{
		get
		{
			return _health;
		}
		set
		{
			_health = value;
			if (_health <= 0)
			{
				Death();
			}
		}
	}

	public SpriteRenderer objectVisuals;

	public List<GridTile> GetAvailableTiles(GridTile currentOccupiedGridTile)
	{
		List<GridTile> result = new List<GridTile>();

		int currentRow = currentOccupiedGridTile.row;
		int currentColumn = currentOccupiedGridTile.column;

		AddGridTileToTheListIfItMeetsCrit(currentColumn + 1, currentRow, result);
		AddGridTileToTheListIfItMeetsCrit(currentColumn - 1, currentRow, result);
		AddGridTileToTheListIfItMeetsCrit(currentColumn, currentRow + 1, result);
		AddGridTileToTheListIfItMeetsCrit(currentColumn, currentRow - 1, result);
		AddGridTileToTheListIfItMeetsCrit(currentColumn + 1, currentRow + 1, result);
		AddGridTileToTheListIfItMeetsCrit(currentColumn - 1, currentRow - 1, result);
		AddGridTileToTheListIfItMeetsCrit(currentColumn - 1, currentRow + 1, result);
		AddGridTileToTheListIfItMeetsCrit(currentColumn + 1, currentRow - 1, result);

		return result;
	}

	private void AddGridTileToTheListIfItMeetsCrit(int column, int row, List<GridTile> gridTileList)
	{
		IEnumerable<GridTile> resultList = LevelManager.instance.allGridTiles.Where(x => x.row == row && x.column == column);
		if (resultList.Count() > 0)
		{
			GridTile gridTile = resultList.First();
			if (gridTile != null && (gridTile.tileObject == null || (gridTile.tileObject != null && !gridTile.tileObject.isObstacle)))
			{
				gridTileList.Add(gridTile);
			}
		}
	}

	private void Death()
	{
	}

	public bool doDamageNow;
	private void Update()
	{
		if (doDamageNow)
		{
			doDamageNow = false;
			health--;
		}
	}
}
