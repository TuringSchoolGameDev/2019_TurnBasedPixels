using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
	//bool - true arba false (1 arba 0)
	public bool isObstacle;
	public int ownerID = -1;

	public SpriteRenderer objectVisuals;
}
