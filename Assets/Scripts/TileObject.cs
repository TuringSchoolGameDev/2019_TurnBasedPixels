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
	public bool isPickup;
	public int ownerID = -1;

	[SerializeField]
	private int _health;
	[SerializeField]
	private int _damage;

	public int range;

	public GameObject projectilePrefab;
	public GameObject deathParticlesPrefab;

	public int startingHealth;
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
			else if (_health > startingHealth)
			{
				_health = startingHealth;
			}
		}
	}

	public SpriteRenderer objectVisuals;

	private void Start()
	{
		startingHealth = health;
	}

	public List<GridTile> GetAvailableTiles(GridTile currentOccupiedGridTile)
	{
		List<GridTile> result = new List<GridTile>();
		GetNeighboursWithRange(currentGridTile.coords.x, currentGridTile.coords.y, result, range);
		return result;
	}
	private void GetNeighboursWithRange(int row, int column, List<GridTile> gridTileList, int range)
	{
		if (range == 0)
		{
			return;
		}
		else
		{
			GridTile gridTile = GetTileIfItMeetsCrit(row, column + 1);
			if (gridTile != null)
			{
				if (!gridTileList.Contains(gridTile))
				{
					gridTileList.Add(gridTile);
				}
				GetNeighboursWithRange(row, column + 1, gridTileList, range - 1);
			}
			gridTile = GetTileIfItMeetsCrit(row, column - 1);
			if (gridTile != null)
			{
				if (!gridTileList.Contains(gridTile))
				{
					gridTileList.Add(gridTile);
				}
				GetNeighboursWithRange(row, column - 1, gridTileList, range - 1);
			}
			gridTile = GetTileIfItMeetsCrit(row + 1, column);
			if (gridTile != null)
			{
				if (!gridTileList.Contains(gridTile))
				{
					gridTileList.Add(gridTile);
				}
				GetNeighboursWithRange(row + 1, column, gridTileList, range - 1);
			}
			gridTile = GetTileIfItMeetsCrit(row - 1, column);
			if (gridTile != null)
			{
				if (!gridTileList.Contains(gridTile))
				{
					gridTileList.Add(gridTile);
				}
				GetNeighboursWithRange(row - 1, column, gridTileList, range - 1);
			}
		}
	}
	private GridTile GetTileIfItMeetsCrit(int row, int column)
	{
		IEnumerable<GridTile> resultList = LevelManager.instance.allGridTiles.Where(x => x.coords.x == row && x.coords.y == column);
		if (resultList.Count() > 0)
		{
			GridTile gridTile = resultList.First();
			if (TileGridHelpers.TileGridIsOccupiedByEnemy(gridTile, ownerID) || !TileGridHelpers.TileGridIsOccupiedBySomething(gridTile) || TileGridHelpers.TileGridIsOccupiedByPickup(gridTile))
			{
				return gridTile;
			}
		}
		return null;
	}

	public void MakeAction(List<GridTile> availableTiles, GridTile oldGridTile, GridTile destinationGridTile, Action<bool> callback)
	{
		if (!TileGridHelpers.TileGridIsOccupiedBySomething(destinationGridTile) || TileGridHelpers.TileGridIsOccupiedByPickup(destinationGridTile))
		{
			Dictionary<Vector2Int, int> map = TileGridHelpers.GetGridMapForMovement(availableTiles);
			List<int> passableTiles = new List<int>() { 0 };
			List<Vector2Int> path = PathFinding2D.find4(oldGridTile.coords, destinationGridTile.coords, map, passableTiles);

			Move(path, oldGridTile,
				callback,
				() =>
				{
					TakeDamage(-1);
					GameObject newGO = Instantiate(deathParticlesPrefab, destinationGridTile.transform.position, Quaternion.identity);
					Vector3 tmpPosition = newGO.transform.position;
					tmpPosition.z -= 1;
					newGO.transform.position = tmpPosition;
				}
			);
		}
		else if (TileGridHelpers.TileGridIsOccupiedByEnemy(destinationGridTile, ownerID))
		{
			GameObject projectileGO = Instantiate(projectilePrefab);
			Projectile projectile = projectileGO.GetComponent<Projectile>();
			projectile.Init(gameObject.transform.position, destinationGridTile.transform.position, () =>
			{
				Attack(destinationGridTile);
				GameObject newGO = Instantiate(deathParticlesPrefab, destinationGridTile.transform.position, Quaternion.identity);
				Vector3 tmpPosition = newGO.transform.position;
				tmpPosition.z -= 1;
				newGO.transform.position = tmpPosition;
				callback?.Invoke(true);
			});
		}
	}

	private void Move(List<Vector2Int> path, GridTile startingTile, Action<bool> callback, Action pickupCallback)
	{
		StartCoroutine(MoveCoroutine(path, startingTile, callback, pickupCallback));
	}

	private IEnumerator MoveCoroutine(List<Vector2Int> path, GridTile startingTile, Action<bool> callback, Action pickupCallback)
	{
		GridTile tmpOldGridTile = startingTile;
		foreach (Vector2Int pathNode in path)
		{
			GridTile tmpGridTile = LevelManager.instance.GetGridTileByCoords(pathNode);
			if (tmpGridTile != null)
			{
				if (TileGridHelpers.TileGridIsOccupiedByPickup(tmpGridTile))
				{
					tmpGridTile.tileObject.Death();
					pickupCallback?.Invoke();
				}

				transform.position = tmpGridTile.transform.position;
				tmpGridTile.tileObject = this;
				currentGridTile = tmpGridTile;

				tmpOldGridTile.tileObject = null;
				tmpOldGridTile = tmpGridTile;

				if (AudioManager.instance != null)
				{
					AudioManager.instance.PlayAudio(AudioManager.instance.stepAudio);
				}

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
		if (AudioManager.instance != null)
		{
			AudioManager.instance.PlayAudio(AudioManager.instance.deathAudio);
		}

		GameObject newGO = Instantiate(deathParticlesPrefab, gameObject.transform.position, Quaternion.identity);
		Vector3 tmpPosition = newGO.transform.position;
		tmpPosition.z -= 1;
		newGO.transform.position = tmpPosition;

		currentGridTile.tileObject = null;
		List<Player> players = GameManager.instance.allActivePlayers.Where(x => ownerID == x.ownerID).ToList();
		if (players.Count > 0)
		{
			players[0].allOwnedTileObjects.Remove(this);
		}
		
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
