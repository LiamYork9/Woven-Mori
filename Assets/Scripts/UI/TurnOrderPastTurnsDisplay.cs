using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MoriSkills;


public class TurnOrderPastTurnsDisplay : MonoBehaviour
{
    public TMP_Text displayName;

    public int turnPostion;

    public Sprite baseSprite;
    public Animator animator;
  
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
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

        if(BattleManager.Instance.SBS?.selectedSkill?.skillId!=SkillId.None && (BattleManager.Instance.SBS?.selectedSkill?.turnShift*-1)-1==turnPostion&&TurnOrderManager.Instance.recentTurns.Count>turnPostion)
        {
            animator.SetBool("TargetShift",true);
        }
        else
        {
            animator.SetBool("TargetShift",false);
        }
    }
}

