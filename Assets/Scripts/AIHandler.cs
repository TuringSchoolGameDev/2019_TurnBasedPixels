using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHandler : InputHandler
{
	public Player player;

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
		SelectClick(newSelectedGridTile);
		newSelectedGridTile = GetRandomNeighbour(availableTiles);
		ActionClick(newSelectedGridTile);
	}

	protected override GridTile GetCurrentObject()
	{
		return player.allOwnedTileObjects[0].currentGridTile;
	}

	private GridTile GetRandomNeighbour(List<GridTile> availableTiles)
	{
		return availableTiles[0];
	}

	protected override void SelectClick(GridTile newSelectedGridTile)
	{
		if (newSelectedGridTile != null)
		{
			if (newSelectedGridTile != selectedTile && newSelectedGridTile.IsThisTileSelectable())
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
