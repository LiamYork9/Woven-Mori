using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
      [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";

    [SerializeField] public GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    private string selectedProfileId = "test";

    public static DataPersistenceManager instance { get; private set; }

    private void Awake() 
    {
        
        if (instance != null) 
        {
          
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        instance = this;
         this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
         this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
        }
    }

    private void Start() 
    {
      
    }

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
         SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
         SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       
         this.dataPersistenceObjects = FindAllDataPersistenceObjects();
       
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("Yes");
        SaveGame();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;

        LoadGame();
    }

    public void NewGame() 
    {
        this.gameData = new GameData();
       
        
    }

    public void LoadGame()
    {
        if (disableDataPersistence)
        {
            return;
        }
        // load any saved data from a file using the data handler
       this.gameData = dataHandler.Load(selectedProfileId);
        

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (disableDataPersistence)
        {
            return;
        }
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.SaveData(gameData);
        }

        gameData.lastUpdated = System.DateTime.Now.ToBinary();
        dataHandler.Save(gameData,selectedProfileId);

        Debug.Log("saved Game");


    }

    private void OnApplicationQuit() 
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
