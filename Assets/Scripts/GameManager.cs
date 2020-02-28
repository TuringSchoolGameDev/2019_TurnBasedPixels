using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public GameObject tilePrefab;
	public GameObject tileGridParent;
    void Start()
    {
		Debug.Log("Dabar loadinsim sita lygi: " + PersistentData.whichLevelToLoad);

		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 10; j++)
			{ 
				GameObject tile = Instantiate(tilePrefab, tileGridParent.transform);

				Vector3 tmpPosition = tile.transform.position;
				tmpPosition.x += i;
				tmpPosition.y += j;
				tile.transform.position = tmpPosition;
			}
		}
    }
}
