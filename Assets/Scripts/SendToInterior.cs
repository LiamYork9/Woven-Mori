using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SendToInterior : MonoBehaviour
{
    public  Vector3 TargetSpawnPosition;
    public string sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PartyManager.Instance.SpawnLocation = TargetSpawnPosition;
         SceneManager.LoadScene(sceneName);
    }
}
