using System;
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
	[SerializeField]
	private int _damage;

	public GameObject deathParticlesPrefab;

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

		//int currentRow = currentOccupiedGridTile.coords.x;
		//int currentColumn = currentOccupiedGridTile.coords.y;

		for (int i = 0; i < LevelManager.instance.allGridTiles.Count; i++)
		{
			AddGridTileToTheListIfItMeetsCrit(LevelManager.instance.allGridTiles[i].coords.x, LevelManager.instance.allGridTiles[i].coords.y, result);
		}
		/*AddGridTileToTheListIfItMeetsCrit(currentColumn + 1, currentRow, result);
		AddGridTileToTheListIfItMeetsCrit(currentColumn - 1, currentRow, result);
		AddGridTileToTheListIfItMeetsCrit(currentColumn, currentRow + 1, result);
		AddGridTileToTheListIfItMeetsCrit(currentColumn, currentRow - 1, result);
		AddGridTileToTheListIfItMeetsCrit(currentColumn + 1, currentRow + 1, result);
		AddGridTileToTheListIfItMeetsCrit(currentColumn - 1, currentRow - 1, result);
		AddGridTileToTheListIfItMeetsCrit(currentColumn - 1, currentRow + 1, result);
		AddGridTileToTheListIfItMeetsCrit(currentColumn + 1, currentRow - 1, result);*/

		return result;
	}

	private void AddGridTileToTheListIfItMeetsCrit(int row, int column, List<GridTile> gridTileList)
	{
		IEnumerable<GridTile> resultList = LevelManager.instance.allGridTiles.Where(x => x.coords.x == row && x.coords.y == column);
		if (resultList.Count() > 0)
		{
			GridTile gridTile = resultList.First();
			if (TileGridHelpers.TileGridIsOccupiedByEnemy(gridTile, ownerID) || !TileGridHelpers.TileGridIsOccupiedBySomething(gridTile))
			{
				gridTileList.Add(gridTile);
			}
		}
	}

	public void MakeAction(List<GridTile> availableTiles, GridTile oldGridTile, GridTile destinationGridTile, Action<bool> callback)
	{
		if (!TileGridHelpers.TileGridIsOccupiedBySomething(destinationGridTile))
		{
			Dictionary<Vector2Int, int> map = TileGridHelpers.GetGridMapForMovement(availableTiles);
			List<int> passableTiles = new List<int>() { 0 };
			List<Vector2Int> path = PathFinding2D.find4(oldGridTile.coords, destinationGridTile.coords, map, passableTiles);

			Move(path, oldGridTile, callback);
		}
		else if (TileGridHelpers.TileGridIsOccupiedByEnemy(destinationGridTile, ownerID))
		{
			Attack(destinationGridTile);
			callback?.Invoke(true);
		}
	}

	private void Move(List<Vector2Int> path, GridTile startingTile, Action<bool> callback)
	{
		StartCoroutine(MoveCoroutine(path, startingTile, callback));
	}

	private IEnumerator MoveCoroutine(List<Vector2Int> path, GridTile startingTile, Action<bool> callback)
	{
		GridTile tmpOldGridTile = startingTile;
		foreach (Vector2Int pathNode in path)
		{
			GridTile tmpGridTile = LevelManager.instance.GetGridTileByCoords(pathNode);
			if (tmpGridTile != null)
			{
				transform.position = tmpGridTile.transform.position;
				tmpGridTile.tileObject = this;
				currentGridTile = tmpGridTile;

				tmpOldGridTile.tileObject = null;
				tmpOldGridTile = tmpGridTile;

				yield return new WaitForSeconds(0.5f);
			}
			else
			{
				break;
			}
		}

		callback?.Invoke(true);
		yield break;
	}

	private void Attack(GridTile destinationGridTile)
	{
		destinationGridTile.tileObject.TakeDamage(_damage);
	}
	public void TakeDamage(int damage)
	{
		health -= damage;
	}
	private void Death()
	{
		GameObject newGO = Instantiate(deathParticlesPrefab, gameObject.transform.position, Quaternion.identity);
		Vector3 tmpPosition = newGO.transform.position;
		tmpPosition.z -= 1;
		newGO.transform.position = tmpPosition;

		currentGridTile.tileObject = null;
		Player player = GameManager.instance.allActivePlayers.Where(x => ownerID == x.ownerID).First();
		player.allOwnedTileObjects.Remove(this);
		Destroy(gameObject);
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
