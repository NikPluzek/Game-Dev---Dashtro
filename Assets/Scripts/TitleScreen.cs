using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScree : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }

}
