using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PartyManager : MonoBehaviour
{

    [SerializeReference]
    public List<PlayerCharacter> party;
    public static PartyManager Instance;

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

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPartyMemeber(PlayerCharacter playerCharacter)
    {
        if (!party.Contains(playerCharacter))
        {
            party.Add(playerCharacter);
        }
        

        
    }

    
}
