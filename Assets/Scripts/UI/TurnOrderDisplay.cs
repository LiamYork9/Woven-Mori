using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MoriSkills;


public class TurnOrderDisplay : MonoBehaviour
{
    public TMP_Text displayName;

    public int turnPostion;
    public Animator animator;
  
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(TurnOrderManager.Instance.turnOrder[turnPostion]!=null)
        {
            displayName.text = TurnOrderManager.Instance.turnOrder[turnPostion].unit.name;
            gameObject.GetComponent<Image>().sprite = TurnOrderManager.Instance.turnOrder[turnPostion].unit.chSprite;
        }
        
     
        if (TurnOrderManager.Instance.turnOrder[turnPostion] is PolyTurn)
        {
            gameObject.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
        }
        else if (TurnOrderManager.Instance.turnOrder[turnPostion].unit.partyMember == true)
        {
            gameObject.GetComponent<Image>().color = new Color32(0, 0, 255, 255);
        }
        else
        {
            gameObject.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        }

         if (TurnOrderManager.Instance.turnOrder[turnPostion].fated == true)
        {
            displayName.color = Color.yellow;
        }
        else if (TurnOrderManager.Instance.turnOrder[turnPostion].visited>0)
        {
            displayName.color = Color.red;
        }
        else
        {
            displayName.color = Color.black;
        }

        if(BattleManager.Instance.SBS.selectedSkill.skillId!=SkillId.None && (BattleManager.Instance.SBS.selectedSkill.turnShift==turnPostion|| (turnPostion==1&&BattleManager.Instance.SBS.selectedSkill.turnShift*-1>TurnOrderManager.Instance.recentTurns.Count)) )
        {
            animator.SetBool("TargetShift",true);
        }
        else
        {
            animator.SetBool("TargetShift",false);
        }
    }
}
