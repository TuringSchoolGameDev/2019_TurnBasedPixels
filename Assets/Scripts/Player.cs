using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public List<TileObject> allOwnedTileObjects = new List<TileObject>();
	public Color playerColor;
	public int ownerID;
}
