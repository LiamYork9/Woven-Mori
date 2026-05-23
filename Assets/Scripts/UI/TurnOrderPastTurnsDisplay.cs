using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class TurnOrderPastTurnsDisplay : MonoBehaviour
{
    public TMP_Text displayName;

    public int turnPostion;

    public Sprite baseSprite;
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnOrderManager.Instance.recentTurns.Count > turnPostion)
        {
            displayName.text = TurnOrderManager.Instance.recentTurns[turnPostion].unit.name;
            gameObject.GetComponent<Image>().sprite = TurnOrderManager.Instance.recentTurns[turnPostion].unit.chSprite;



            if (TurnOrderManager.Instance.recentTurns[turnPostion].unit.partyMember == true)
            {
                gameObject.GetComponent<Image>().color = new Color32(0, 0, 200, 190);
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color32(200, 0, 0, 190);
            }

            if (TurnOrderManager.Instance.recentTurns[turnPostion].fated == true)
            {
                displayName.color = Color.yellow;
            }
            else if (TurnOrderManager.Instance.recentTurns[turnPostion].visited>0)
            {
                displayName.color = Color.red;
            }
            else
            {
                displayName.color = Color.black;
            }
        }
        else
        {
            displayName.text = " ";
            gameObject.GetComponent<Image>().sprite = baseSprite;
            gameObject.GetComponent<Image>().color = new Color32(100,100,100,190);
        }
    }
}

