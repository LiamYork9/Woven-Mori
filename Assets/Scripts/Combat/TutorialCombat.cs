using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class TutorialCombat : MonoBehaviour
{
    public DialogueInteraction dialogueInteraction;

    public Dialogue dialogue;

    public GameObject battleUI;

    public List<DialogGraph> dialogGraphs;

    public List<UnitBody> myGuys;

    public List<Turn> fuckYouTurnManger;

    public List<GameObject> turnOff;

    public bool visited = false;

    public Item item;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < PartyManager.Instance.party.Count; i++)
        {
            PartyManager.Instance.party[i].Restore();
        }
        InventoryManager.Instance.AddItem(item,1);
        dialogueInteraction.StartDialogueCombat();
        BodyToTurn(myGuys);
    }



    // Update is called once per frame
    void Update()
    {
        if(BattleManager.Instance.globalTurn == 1)
        {
            OffAllBut(0,turnOff);
        }
         if(BattleManager.Instance.globalTurn == 2 || BattleManager.Instance.globalTurn == 4 || BattleManager.Instance.globalTurn == 5 || BattleManager.Instance.globalTurn == 7 || BattleManager.Instance.globalTurn == 10)
        {
            OffAllBut(1,turnOff);
           
        }
        
       
        
         
         
    }

    public void BodyToTurn(List<UnitBody> units)
    {
       
       foreach(GameObject gameObject in TurnOrderManager.Instance.allFighters)
        {
            gameObject.GetComponent<UnitBody>().turnsAdded =0;
        }
        foreach(UnitBody tempUnit in units)
        {
            
        
            Turn tempTurn = new Turn();
            tempTurn.PopulateTurn(tempUnit,tempUnit.CheckFate(),tempUnit.GetFate());
            fuckYouTurnManger.Add(tempTurn);
            tempUnit.turnsAdded++;
        }
        TurnOrderManager.Instance.turnOrder = fuckYouTurnManger;
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
        if(BattleManager.Instance.globalTurn == 2 && BattleManager.Instance.usingSkill == true)
        {
            dialogue.lines = dialogGraphs[2];
            dialogueInteraction.StartDialogueCombat();
            OffAllBut(1,BattleManager.Instance.SBS.skillButtons);
            EndSegment();
            
        }
        if(BattleManager.Instance.globalTurn == 4 && BattleManager.Instance.usingSkill == true || BattleManager.Instance.globalTurn == 5 && BattleManager.Instance.usingSkill == true)
        {
            
             OffAllBut(0,BattleManager.Instance.SBS.skillButtons);
            EndSegment();
        }
        if(BattleManager.Instance.globalTurn == 7 && BattleManager.Instance.usingSkill == true)
        {
             OffAllBut(1,BattleManager.Instance.SBS.skillButtons);
              EndSegment();
        }
         if(BattleManager.Instance.globalTurn == 9 && BattleManager.Instance.usingSkill == true)
        {
             OffAllBut(2,BattleManager.Instance.SBS.skillButtons);
            EndSegment();
        }
        if(BattleManager.Instance.globalTurn == 10 && BattleManager.Instance.usingSkill == true)
        {
             OffAllBut(1,BattleManager.Instance.SBS.skillButtons);
              EndSegment();
        }
       
        
         
        
    }
    public void SkillTut()
    {
        if(BattleManager.Instance.globalTurn == 2)
        {
             dialogue.lines = dialogGraphs[1];
            dialogueInteraction.StartDialogueCombat();
            TurnOrderManager.Instance.turnPlayer.StartOfTurn.RemoveListener(SkillTut);
        }
       
        if(BattleManager.Instance.globalTurn == 4)
        {
            dialogue.lines = dialogGraphs[3];
            dialogueInteraction.StartDialogueCombat();
            TurnOrderManager.Instance.turnPlayer.StartOfTurn.RemoveListener(SkillTut);
        }
         if(BattleManager.Instance.globalTurn == 5)
        {
            dialogue.lines = dialogGraphs[4];
            dialogueInteraction.StartDialogueCombat();
            TurnOrderManager.Instance.turnPlayer.StartOfTurn.RemoveListener(SkillTut);
        }
        if(BattleManager.Instance.globalTurn == 7)
        {
            dialogue.lines = dialogGraphs[5];
            dialogueInteraction.StartDialogueCombat();
            TurnOrderManager.Instance.turnPlayer.StartOfTurn.RemoveListener(SkillTut);
        }
         if(BattleManager.Instance.globalTurn == 9 && visited == false)
        {
            
            dialogue.lines = dialogGraphs[6];
            dialogueInteraction.StartDialogueCombat();
            TurnOrderManager.Instance.turnPlayer.StartOfTurn.RemoveListener(SkillTut);
        }
        if(BattleManager.Instance.globalTurn == 9 && visited == true)
        {
            dialogue.lines = dialogGraphs[7];
            dialogueInteraction.StartDialogueCombat();
            TurnOrderManager.Instance.turnPlayer.StartOfTurn.RemoveListener(SkillTut);
        }
        if(BattleManager.Instance.globalTurn == 10)
        {
            dialogue.lines = dialogGraphs[8];
            dialogueInteraction.StartDialogueCombat();
            TurnOrderManager.Instance.turnPlayer.StartOfTurn.RemoveListener(SkillTut);
        }
        if(BattleManager.Instance.globalTurn == 12)
        {
            dialogue.lines = dialogGraphs[9];
            dialogueInteraction.StartDialogueCombat();
            TurnOrderManager.Instance.turnPlayer.StartOfTurn.RemoveListener(SkillTut);
        }
        
         
    }
    public void EndSegment()
    {
        foreach(GameObject player in BattleManager.Instance.playerSlots)
        {
            player.GetComponent<UnitBody>().StartOfTurn.RemoveListener(SkillTut);
            player.GetComponent<UnitBody>().StartOfTurn.AddListener(SkillTut);
        }
       
    }

    public void AttackTrue()
    {
        BattleManager.Instance.attacking = true;
        
    }
    public void VisitedTrue()
    {
       visited = true;
    }

    public void Turn9()
    {
        
        
             OffAllBut(1,turnOff);
        
    }
    public void turn92()
    {
       
        
            OffAllBut(2,turnOff);
        
    }
}
