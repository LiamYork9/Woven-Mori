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
        if(DataPersistenceManager.instance.gameData != null)
        {
            DataPersistenceManager.instance.LoadGame();
            SceneManager.LoadSceneAsync("NodeTesting");
        }
        else
        {
            Debug.Log("No Data to load");
        }
       
    }
}
