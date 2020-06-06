using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public static LevelManager instance;

	public GameObject tilePrefab;
	public GameObject tileGridParent;

	public List<GameObject> tileObstaclePrefabs;
	public GameObject tileObstacleParent;

	public List<GameObject> charactersPrefabs;
	public GameObject characterParent;

	public GameObject healthBarPrefab;
	public Transform healthBarParent;

	public List<GridTile> allGridTiles = new List<GridTile>();
	public List<TileObject> allObsticles = new List<TileObject>();

	private void Awake()
	{
		instance = this;
	}


	public void StartNewLevel(string levelName, List<Player> allActivePlayers)
	{
		LoadLevel(levelName, allActivePlayers);
	}


	private void LoadLevel(string levelName, List<Player> allActivePlayers)
	{
		LevelInfo levelInfo = GetLevelInfo(levelName);

		allGridTiles = LoadGrid(levelInfo.rowCount, levelInfo.columnCount);
		allObsticles = LoadTileObjects(allGridTiles, levelInfo.obstacles, "Obstacle", tileObstaclePrefabs, tileObstacleParent);

		allActivePlayers[0].allOwnedTileObjects = LoadTileObjects(allGridTiles, levelInfo.player0Objects, "Player0Character", charactersPrefabs, characterParent);
		ConfigPlayerCharacter(allActivePlayers[0].allOwnedTileObjects, allActivePlayers[0]);

		allActivePlayers[1].allOwnedTileObjects = LoadTileObjects(allGridTiles, levelInfo.player1Objects, "Player1Character", charactersPrefabs, characterParent);
		ConfigPlayerCharacter(allActivePlayers[1].allOwnedTileObjects, allActivePlayers[1]);
	}


	private LevelInfo GetLevelInfo(string levelName)
	{
		TextAsset textAsset = Resources.Load<TextAsset>("Levels/" + levelName);
		string[] lines = textAsset.text.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

		LevelInfo levelInfo = new LevelInfo();

		for (int i = 0; i < lines.Length; i++)
		{
			if (i == 0)
			{
				levelInfo.rowCount = int.Parse(lines[i]);
			}
			else if (i == 1)
			{
				levelInfo.columnCount = int.Parse(lines[i]);
			}
			else if (i == 2)
			{
				List<Vector3> tileObjectInfo = SplitTextLineToTileObjectInfo(lines[i]);
				levelInfo.obstacles = tileObjectInfo;
			}
			else if (i == 3)
			{
				List<Vector3> tileObjectInfo = SplitTextLineToTileObjectInfo(lines[i]);
				levelInfo.player0Objects = tileObjectInfo;
			}
			else if (i == 4)
			{
				List<Vector3> tileObjectInfo = SplitTextLineToTileObjectInfo(lines[i]);
				levelInfo.player1Objects = tileObjectInfo;
			}
		}

		return levelInfo;
	}
	private List<Vector3> SplitTextLineToTileObjectInfo(string lineInfo)
	{
		List<Vector3> resultList = new List<Vector3>();

		string[] obstaclesInfos = lineInfo.Split("//".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < obstaclesInfos.Length; i++)
		{
			string[] TileObjectInfoStr = obstaclesInfos[i].Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
			Vector3 tileObjectInfo = new Vector3(int.Parse(TileObjectInfoStr[0]), int.Parse(TileObjectInfoStr[1]), int.Parse(TileObjectInfoStr[2]));
			resultList.Add(tileObjectInfo);
		}

		return resultList;
	}



	private List<GridTile> LoadGrid(int rowCount, int columnCount)
	{
		List<GridTile> allGridTiles = new List<GridTile>();
		for (int i = 0; i < rowCount; i++)
		{
			for (int j = 0; j < columnCount; j++)
			{
				GameObject tile = Instantiate(tilePrefab, tileGridParent.transform);
				GridTile gridTile = tile.GetComponent<GridTile>();
				allGridTiles.Add(gridTile);


				tile.name = "Tile:" + i + " " + j;

				Vector3 tmpPosition = tile.transform.position;
				tmpPosition.x += i;
				tmpPosition.y += j;
				tile.transform.position = tmpPosition;

				gridTile.coords.x = j;
				gridTile.coords.y = i;
			}
		}
		return allGridTiles;
	}
	private List<TileObject> LoadTileObjects(List<GridTile> allGridTiles, List<Vector3> coords, string tileObjectName, List<GameObject> prefabs, GameObject parent)
	{
		List<TileObject> allTileObjects = new List<TileObject>();
		for (int i = 0; i < coords.Count; i++)
		{
			List<GridTile> allFilteredTiles = allGridTiles.Where(x => x.coords.x == coords[i].x && x.coords.y == coords[i].y).ToList();
			if (allFilteredTiles != null && allFilteredTiles.Count > 0)
			{
				GameObject tileObjectGO = Instantiate(prefabs[(int)coords[i].z], parent.transform);
				tileObjectGO.name = tileObjectName + coords[i].x + " " + coords[i].y;

				tileObjectGO.transform.position = allFilteredTiles[0].transform.position;

				TileObject tileObject = tileObjectGO.GetComponent<TileObject>();
				allFilteredTiles[0].tileObject = tileObject;
				tileObject.currentGridTile = allFilteredTiles[0];   //ant objekto sukurimo mes priskyriam jo dabartine vieta
				allTileObjects.Add(tileObject);
			}
		}
		return allTileObjects;
	}

	private void ConfigPlayerCharacter(List<TileObject> allTileObjects, Player player)
	{
		for (int i = 0; i < allTileObjects.Count; i++)
		{
			allTileObjects[i].objectVisuals.color = player.playerColor;
			allTileObjects[i].ownerID = player.ownerID;

			GameObject healthBar = Instantiate(healthBarPrefab, healthBarParent);
			healthBar.GetComponent<HealthHandler>().Init(allTileObjects[i]);
		}
	}

	public GridTile GetGridTileByCoords(Vector2Int coords)
	{
		List<GridTile> allTiles = allGridTiles.Where(x => x.coords == coords).ToList();
		if (allTiles != null && allTiles.Count > 0)
		{
			return allTiles[0];
		}
		return null;
	}
}
