using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public class PartyManager : MonoBehaviour, IDataPersistence
{

    [SerializeReference]
    public List<PlayerCharacter> party;
    public static PartyManager Instance;

    public  List<PlayerCharacter> starter;

    public List<GameObject> followers; 

    public string sceneName;

     public string bonfire;

    public bool PM1,PM2,PM3;

    public bool staticEncounter;

    public bool rest;

    public int funds;

    public Vector3 SpawnLocation;

    public Vector3 restLocation;

    public static PartyManager GetInstance()
    {
        return Instance;
    }
    



    public void Awake()
    {
        if (PartyManager.Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);

    }
    void Start()
    {
        
    }

    public void LoadData(GameData data)
    {
        
        this.party = data.party;
    }

     public void SaveData( GameData data)
    {
        data.party = this.party;
    }

    // Update is called once per frame
    void Update()
    {
        if(funds > 99999)
        {
            funds = 99999;
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            for (int i = 0; i < PartyManager.Instance.party.Count; i++)
            {
                PartyManager.Instance.party[i].ResetLevel();
            }
        }
    }

    public void AddPartyMemeber(PlayerCharacter playerCharacter)
    {
        if (!party.Contains(playerCharacter))
        {
            party.Add(playerCharacter);
        }

       
        

        
    }

    
}
