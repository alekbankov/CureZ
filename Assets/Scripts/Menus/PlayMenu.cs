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
		SceneManager.LoadScene("FriendsList");
    }
	public void LoadGuild()
	{
		SceneManager.LoadScene("GuildMenu");
	}

	public void LoadRessources()
    {
		SceneManager.LoadScene("ResourcesMenu");
    }

	public void Quit()
    {
		Application.Quit();
    }
}
