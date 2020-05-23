using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHandler : InputHandler
{
	private void OnEnable()
	{
		HandleInput();
	}

	protected override void Update()
	{
	}

	protected override void HandleInput()
	{
		newSelectedGridTile = GetRandomTileObject();
		if (newSelectedGridTile != null)
		{
			SelectClick(newSelectedGridTile);
			newSelectedGridTile = GetRandomGridTile(availableTiles);
			ActionClick(newSelectedGridTile);
		}
	}
	protected override GridTile GetRandomTileObject()
	{
		if (player.allOwnedTileObjects.Count > 0)
		{
			int index = Random.Range(0, player.allOwnedTileObjects.Count);
			return player.allOwnedTileObjects[index].currentGridTile;
		}
		return null;
	}

	private GridTile GetRandomGridTile(List<GridTile> availableTiles)
	{
		if (availableTiles.Count > 0)
		{
			int index = Random.Range(0, availableTiles.Count);
			return availableTiles[index];
		}
		else
		{
			return null;
		}
	}

	protected override void SelectClick(GridTile newSelectedGridTile)
	{
		if (newSelectedGridTile != null)
		{
			if (newSelectedGridTile != selectedTile && newSelectedGridTile.IsThisTileSelectable(player.ownerID))
			{
				Deselect();
				DeselectAvailable();
				Select(newSelectedGridTile);
				MakeAvailable(newSelectedGridTile);
			}
		}
	}
	protected override void ActionClick(GridTile newSelectedGridTile)
	{
		if (newSelectedGridTile != null && selectedTile != null && newSelectedGridTile != selectedTile && selectedTile.CanActionBeMade(newSelectedGridTile))
		{
			selectedTile.MakeAction(availableTiles, newSelectedGridTile, (bool success) =>
			{
				Deselect();
				DeselectAvailable();
				GameManager.instance.Switch();
			});
		}
		else
		{
			Deselect();
			DeselectAvailable();
			GameManager.instance.Switch();
		}
	}
}
