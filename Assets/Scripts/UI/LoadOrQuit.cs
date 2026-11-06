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
        PartyManager.Instance.party.Clear();
        PartyManager.Instance.party.Add(units[0]);
        PartyManager.Instance.funds = 100;
        PartyManager.Instance.SpawnLocation = new Vector3(5.5f,-15.5f,0);
        PartyManager.Instance.restLocation = PartyManager.Instance.SpawnLocation;
        PartyManager.Instance.encounterID = "";
        PartyManager.Instance.hasShopped = false;
        PartyManager.Instance.hasTut = false;
        PartyManager.Instance.PM1 = false;
        SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
