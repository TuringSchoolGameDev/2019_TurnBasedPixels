using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnStartClicked(int whichLevelToLoad)
	{
		PersistentData.whichLevelToLoad = whichLevelToLoad;
		SceneManager.LoadScene("Game");
	}
}
