using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
  public void OnNewGame()
    {
        DataPersistenceManager.instance.NewGame();
        SceneManager.LoadSceneAsync("NodeTesting");
    }

    public void OnContinue()
    {
        DataPersistenceManager.instance.LoadGame();
        SceneManager.LoadSceneAsync("NodeTesting");
    }
}
