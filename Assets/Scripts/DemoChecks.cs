using UnityEngine;

public class DemoChecks : MonoBehaviour
{
  
    public DialogueInteraction dialogue;

    
    public DialogueInteraction menuDialogue;

    public GameObject lookPoint;

    public GameObject wall;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         Invoke("LateStart", 0.1f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PartyManager.Instance.hasShopped == true && PartyManager.Instance.hasTut == true && PartyManager.Instance.PM1 == true)
        {
            wall.SetActive(false);
        }
    }

    public void LateStart()
    {
        if(PartyManager.Instance.hasTut == false)
        {
            dialogue.StartDialogue();
        }
    }

    public void HasShopped()
    {
        PartyManager.Instance.hasShopped = true;
    }
    
    public void OpenMenu()
    {

       // if(PartyManager.Instance.hasShopped == false)
        
            menuDialogue.StartDialogue();
            
        
    }

     

    
   
}
