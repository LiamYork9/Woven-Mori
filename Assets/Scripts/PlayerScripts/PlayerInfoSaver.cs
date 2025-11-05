using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfoSaver : MonoBehaviour
{

    
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerPosX"))
        {
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");
            transform.position = new Vector3(x, y, z);
            gameObject.GetComponent<PlayerController>().movePoint.position = transform.position;
        }
    }

    
    void Update()
    {

    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("PlayerPosX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerPosZ", transform.position.z);
        PlayerPrefs.Save();
    }


    
    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
