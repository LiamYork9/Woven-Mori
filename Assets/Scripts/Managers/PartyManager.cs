using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PartyManager : MonoBehaviour
{

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
            Destroy(this);
        }

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    
}
