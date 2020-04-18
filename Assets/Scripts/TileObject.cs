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
			if (TileGridHelpers.TileGridIsOccupiedByEnemy(gridTile, ownerID) || !TileGridHelpers.TileGridIsOccupiedBySomething(gridTile))
			{
				gridTileList.Add(gridTile);
			}
		}
	}

	public void MakeAction(GridTile oldGridTile, GridTile destinationGridTile)
	{
		if (!TileGridHelpers.TileGridIsOccupiedBySomething(destinationGridTile))
		{
			Move(oldGridTile, destinationGridTile);
		}
		else if (TileGridHelpers.TileGridIsOccupiedByEnemy(destinationGridTile, ownerID))
		{
			Attack(destinationGridTile);
		}
	}

	private void Move(GridTile oldGridTile, GridTile destinationGridTile)
	{
		transform.position = destinationGridTile.transform.position; //tile objektas teleportuojasi
		destinationGridTile.tileObject = this;        //nauja vieta suzino apie objekta kuris atsiteleportavo
		currentGridTile = destinationGridTile;   //atsileportaves objektas suzino apie nauja vieta
		oldGridTile.tileObject = null;  //sena vieta turi pamirsti apie sena objekta
	}

	private void Attack(GridTile destinationGridTile)
	{
		destinationGridTile.tileObject.TakeDamage(1);
	}
	public void TakeDamage(int damage)
	{
		health -= damage;
	}
	private void Death()
	{
		Debug.Log("Dead");
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
