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
            displayName.text = TurnOrderManager.Instance.recentTurns[turnPostion].unit.unitName;
            gameObject.GetComponent<Image>().sprite = TurnOrderManager.Instance.recentTurns[turnPostion].unit.chSprite;



            if (TurnOrderManager.Instance.recentTurns[turnPostion].unit.partyMember == true)
            {
                gameObject.GetComponent<Image>().color = new Color32(0, 0, 200, 190);
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color32(200, 0, 0, 190);
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

