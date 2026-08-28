using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class IconAdder : MonoBehaviour
{
    public GameObject grid;

    public UnitBody slot;

    public GameObject iconPrefab;

     public TMP_Text conText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<UnitBody>().updateConditions.AddListener(IconPlacer);
    }

    // Update is called once per frame
    void Update()
    {
        //IconPlacer();
    }

    public void IconPlacer()
    {
        foreach (Transform child in grid.transform) 
        {
            GameObject.Destroy(child.gameObject);
        }
        for(int i = 0; i < slot.conditions.Count; i++)
        {
            GameObject newButton = Instantiate(iconPrefab, grid.transform);
            newButton.GetComponent<Image>().sprite = slot.conditions[i].conditionSprite;
            
            
        }
        for (int i = 0; i < slot.conditions.Count; i++)
        {
            if (IconHolder.Instance.GetComponent<ToolTipCondition>() != null)
            {
                IconHolder.Instance.GetComponent<ToolTipCondition>().hoverEvent.AddListener(ToolTipAdder);
                IconHolder.Instance.GetComponent<ToolTipCondition>().unHoverEvent.AddListener(ToolTipRemover);
            }
        }

    }

     public void ToolTipAdder(GameObject button)
    {
        Condition temp = button.GetComponent<ToolTipCondition>().condition;
        conText.text = temp.description;


    }

    public void ToolTipRemover()
    {
        conText.text = " ";
    }
}
