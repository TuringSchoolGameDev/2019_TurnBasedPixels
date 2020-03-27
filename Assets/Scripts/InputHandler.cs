using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
	public GridTile selected;
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
				if (newSelectedGridTile != selected && newSelectedGridTile.IsThisTileSelectable())
				{
					Deselect();
					Select(newSelectedGridTile);
				}
			}
		}
	}
	private void ActionClick(GridTile newSelectedGridTile)
	{
		if (Input.GetMouseButtonDown(1))
		{
			if (newSelectedGridTile != null && selected != null && newSelectedGridTile != selected && selected.CanActionBeMade(newSelectedGridTile))
			{
				selected.MakeAction(newSelectedGridTile);
				Deselect();
				GameManager.instance.Switch();
			}
		}
	}

	private void Select(GridTile newSelectedGridTile)
	{
		selected = newSelectedGridTile;
		selected.SelectTile();
	}
	private void Deselect()
	{
		if (selected != null)
		{
			selected.DeselectTile();
			selected = null;
		}
	}
}
