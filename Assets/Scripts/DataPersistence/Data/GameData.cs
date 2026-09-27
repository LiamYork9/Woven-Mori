using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData 
{
  
    public Vector3 playerPosition;

    public long lastUpdated;

   
    public List<PlayerCharacter> party;

    public string sceneNameData;
    

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData() 
    {
        
        sceneNameData = "NodeTesting";
      
        playerPosition = Vector3.zero;
       
        
      
    }
}
