using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public List<Player> allActivePlayers = new List<Player>();

	public int currentPlayerID;

	public GameObject gameEndGO;
	public Image gameEndBackground;
	public Text gameEndText;
	private bool gameEnded;
	private void Awake()
	{
		instance = this;
	}

	void Start()
    {
		//startingBackgroundAlphaValue = gameEndBackground.color.a;//pakeista
		//startingTextAlphaValue = gameEndText.color.a;//pakeista

		LevelManager.instance.StartNewLevel("Level" + PersistentData.whichLevelToLoad, allActivePlayers);
		Switch();
	}

	private void Update()//pakeista
	{
		GameEnd();
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

	private void GameEnd()
	{
		if (!gameEnded)
		{
			int victoriousPlayer = CheckForGameEnd();
			if (victoriousPlayer != -1)
			{
				gameEnded = true;
				SetGameVisuals(victoriousPlayer);
				StartCoroutine(ReloadEverythingInXSeconds(5));
			}
		}
	}

	private void SetGameVisuals(int victoryPlayerID)
	{
		string victoryText = string.Format("Player {0} won", victoryPlayerID);//"Player 1 won"
		gameEndGO.SetActive(true);
		gameEndText.text = victoryText;

	}

	private IEnumerator ReloadEverythingInXSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		SceneManager.LoadScene(0);
	}

	private int CheckForGameEnd()
	{
		bool gameEnded = false;
		int victoriousPlayer = -1;
		foreach (Player player in allActivePlayers)
		{
			if (player.allOwnedTileObjects.Count > 0)
			{
				if (!gameEnded)
				{
					gameEnded = true;
					victoriousPlayer = player.ownerID;
				}
				else
				{
					return -1;
				}
			}
		}
		return victoriousPlayer;
	}
}