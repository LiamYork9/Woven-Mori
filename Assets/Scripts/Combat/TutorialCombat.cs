using UnityEngine;

public class TutorialCombat : MonoBehaviour
{
    public DialogueInteraction dialogueInteraction;

    public GameObject battleUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueInteraction.StartDialogueCombat();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void BattleOff()
    {
        battleUI.SetActive(false);
    }
      public void BattleOn()
    {
        battleUI.SetActive(true);
    }
}
