using System.Collections.Generic;
using UnityEngine;

public class TutorialCombat : MonoBehaviour
{
    public DialogueInteraction dialogueInteraction;

    public Dialogue dialogue;

    public GameObject battleUI;

    public List<DialogGraph> dialogGraphs;

    public List<GameObject> turnOff;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueInteraction.StartDialogueCombat();
    }



    // Update is called once per frame
    void Update()
    {
        if(BattleManager.Instance.globalTurn == 1)
        {
            OffAllBut(0,turnOff);
        }
         if(BattleManager.Instance.globalTurn == 2)
        {
            OffAllBut(1,turnOff);
        }
         
    }

    public void BattleOff()
    {
        battleUI.SetActive(false);
    }
      public void BattleOn()
    {
        battleUI.SetActive(true);
    }

    public void OffAllBut(int index, List<GameObject> objects)
    {
        for(int i = 0; i < objects.Count; i++)
        {
            if(i != index)
            {
                objects[i].SetActive(false);
            }
        }
    }

    public void buttonWork()
    {
        if(BattleManager.Instance.globalTurn == 1 && BattleManager.Instance.attacking == true)
        {
            dialogue.lines = dialogGraphs[0];
            dialogueInteraction.StartDialogueCombat();
            EndSegment();
            BattleManager.Instance.attacking = false;
        }
    }
    public void SkillTut()
    {
        dialogue.lines = dialogGraphs[1];
        dialogueInteraction.StartDialogueCombat();
        TurnOrderManager.Instance.turnPlayer.StartOfTurn.RemoveListener(SkillTut);
    }
    public void EndSegment()
    {
        TurnOrderManager.Instance.turnPlayer.StartOfTurn.RemoveListener(SkillTut);
        TurnOrderManager.Instance.turnPlayer.StartOfTurn.AddListener(SkillTut);
    }

    public void AttackTrue()
    {
        BattleManager.Instance.attacking = true;
        
    }
}
