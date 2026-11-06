using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOrQuit : MonoBehaviour
{
    public List<PlayerCharacter> units;
    public string sceneName;

    public void Load()
    {
        foreach (PlayerCharacter character in units)
        {
            character.ResetLevel();
            character.ClearEquipment();
            character.LevelUp(9);
        }
        Destroy(LevelUpManager.Instance.gameObject);
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
