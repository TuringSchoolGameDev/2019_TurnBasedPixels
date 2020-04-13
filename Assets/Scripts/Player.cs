using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public List<TileObject> allOwnedTileObjects = new List<TileObject>();
	public Color playerColor;
	public int ownerID;

	public InputHandler inputHandler;
	public AIHandler aiHandler;
	public void StartTurn()
	{
		if (inputHandler != null)
		{
			inputHandler.enabled = true;
		}
		if (aiHandler != null)
		{
			aiHandler.enabled = true;
		}
	}

	public void EndTurn()
	{
		if (inputHandler != null)
		{
			inputHandler.enabled = false;
		}
		if (aiHandler != null)
		{
			aiHandler.enabled = false;
		}
	}
}
