using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
	public GridTile selectedTile;
	public List<GridTile> availableTiles;
	private GridTile newSelectedGridTile;

	void Update()
    {
        HandleInput();
	}
    
    private void HandleInput()
    {
		newSelectedGridTile = GetCurrentObjectBehindCursor();
		SelectClick(newSelectedGridTile);
		ActionClick(newSelectedGridTile);
	}

	private GridTile GetCurrentObjectBehindCursor()
	{
		GridTile result = null;
			 
		Vector2 coords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D[] raycastHits = Physics2D.RaycastAll(coords, Vector2.zero, 100f);
		if (raycastHits != null)
		{
			foreach (RaycastHit2D raycastHit in raycastHits)
			{
				if (raycastHit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("GroundTile")))
				{
					GridTile gridTile = raycastHit.collider.gameObject.GetComponent<GridTile>();
					if (gridTile != null)
					{
						result = gridTile;
					}
				}
				break;
			}
		}

		return result;
	}

	private void SelectClick(GridTile newSelectedGridTile)
	{
		if (Input.GetMouseButtonDown(0))
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
	}
	private void ActionClick(GridTile newSelectedGridTile)
	{
		if (Input.GetMouseButtonDown(1))
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

	private void Select(GridTile newSelectedGridTile)
	{
		selectedTile = newSelectedGridTile;
		selectedTile.SelectTile();
	}

	private void MakeAvailable(GridTile selectedTile)
	{
		availableTiles = selectedTile.GetAvailableTiles();
		for (int i = 0; i < availableTiles.Count; i++)
		{
			availableTiles[i].MakeTileAvailable();
		}
	}
	private void Deselect()
	{
		if (selectedTile != null)
		{
			selectedTile.DeselectTile();
			selectedTile = null;
		}
	}

	private void DeselectAvailable()
	{
		for (int i = 0; i < availableTiles.Count; i++)
		{
			availableTiles[i].DeselectTile();
		}
	}
}
