using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayTable");
    }

	public void Exit()
	{
		Application.Quit ();
	}
}
