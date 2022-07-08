using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
	public Button playButton;
	public Button friendsButton;
	public Button ressourcesButton;
	public Button quitButton;
	public void LoadGame()
	{
		SceneManager.LoadScene("Game");
	}

	public void LoadFriends()
    {

    }

	public void LoadRessources()
    {

    }

	public void Quit()
    {
		
    }
}
