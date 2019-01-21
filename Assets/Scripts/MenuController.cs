using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    //Start Game
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayTable");
    }

    //Exit Game
	public void Exit()
	{
        SceneManager.LoadScene("MainMenu");
    }
}
