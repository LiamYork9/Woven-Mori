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

    [SerializeField] private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

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

    public void NewGame() 
    {
        this.gameData = new GameData();
       
        
    }

    public void LoadGame()
    {
        // load any saved data from a file using the data handler
       this.gameData = dataHandler.Load();
        
        // if no data can be loaded, initialize to a new game
        if (this.gameData == null) 
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        // push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.SaveData(gameData);
        }

        
        dataHandler.Save(gameData);

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
}
