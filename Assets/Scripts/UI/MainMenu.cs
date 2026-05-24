using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour

{
     [SerializeField] private SaveSlotsMenu saveSlotsMenu;
  public void OnNewGame()
    {
        saveSlotsMenu.ActivateMenu(false);
        this.TurnOffMenu();
        //DataPersistenceManager.instance.NewGame();
        //SceneManager.LoadSceneAsync("NodeTesting");
    }

    public void OnLoadGame()
    {
        saveSlotsMenu.ActivateMenu(true);
        this.TurnOffMenu();
    }

    public void OnContinue()
    {
        if(DataPersistenceManager.instance.gameData != null)
        {
            DataPersistenceManager.instance.LoadGame();
            SceneManager.LoadSceneAsync(DataPersistenceManager.instance.gameData.sceneNameData);
        }
        else
        {
            Debug.Log("No Data to load");
        }
       
    }

    public void TurnOffMenu()
    {
        this.gameObject.SetActive(false);
    }
    public void TurnOnMenu()
    {
        this.gameObject.SetActive(true);
    }
}
