using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class TurnOrderDisplay : MonoBehaviour
{
    public TMP_Text displayName;

    public int turnPostion;
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        displayName.text = TurnOrderManager.Instance.turnOrder[turnPostion].GetComponent<Unit>().unitName;
        gameObject.GetComponent<Image>().sprite = TurnOrderManager.Instance.turnOrder[turnPostion].GetComponent<Unit>().chSprite;
        
     
       
        if (TurnOrderManager.Instance.turnOrder[turnPostion].GetComponent<Unit>().partyMember == true)
        {
            gameObject.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }
    }
}
