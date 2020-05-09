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
		newSelectedGridTile = GetCurrentObject();
		if (newSelectedGridTile != null)
		{
			SelectClick(newSelectedGridTile);
			newSelectedGridTile = GetRandomNeighbour(availableTiles);
			ActionClick(newSelectedGridTile);
		}
	}

	protected override GridTile GetCurrentObject()
	{
		if (player.allOwnedTileObjects.Count > 0)
		{
			return player.allOwnedTileObjects[0].currentGridTile;
		}
		return null;
	}

	private GridTile GetRandomNeighbour(List<GridTile> availableTiles)
	{
		return availableTiles[0];
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
			if (selectedTile.MakeAction(availableTiles, newSelectedGridTile))
			{
				Deselect();
				DeselectAvailable();
				GameManager.instance.Switch();
			}
		}
	}
}
