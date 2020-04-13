using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public List<Player> allActivePlayers = new List<Player>();

	public int currentPlayerID;

	private void Awake()
	{
		instance = this;
	}

	void Start()
    {
		LevelManager.instance.StartNewLevel("Level" + PersistentData.whichLevelToLoad, allActivePlayers);
		Switch();
	}

	public void Switch()
	{
		allActivePlayers[currentPlayerID].EndTurn();
		currentPlayerID++;
		if (currentPlayerID >= allActivePlayers.Count)
		{
			currentPlayerID = 0;
		}
		allActivePlayers[currentPlayerID].StartTurn();
	}
}