using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{
    public GameObject panel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panel.GetComponent<Image>().sprite = PartyManager.Instance.battleBackground;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
