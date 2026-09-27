using UnityEngine;
using System.Collections.Generic;

public class TurnOrderUIChanger : MonoBehaviour
{
     public List<GameObject> turnIcons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GetComponent<TurnOrderHover>().hoverEvent.AddListener(UIChange);
         this.GetComponent<TurnOrderHover>().unHoverEvent.AddListener(UIChangeUndo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

      public void UIChange( GameObject ui)
    {
        for (int i = 0; i < turnIcons.Count; i++)
        {
            turnIcons[i].SetActive(true);
        }
    }
    
    public void UIChangeUndo()
    {
        for (int i = 0; i < turnIcons.Count; i++)
        {
            turnIcons[i].SetActive(false);
        }
    }
}
