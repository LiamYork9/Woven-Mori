using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData 
{
  
    public Vector3 playerPosition;

   
    public List<PlayerCharacter> party;
    

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData() 
    {
        
      
        playerPosition = Vector3.zero;
       
        
      
    }
}
