using UnityEngine;

public class DemoChecks : MonoBehaviour
{
  
    public DialogueInteraction dialogue;

    
    public DialogueInteraction menuDialogue;

    public GameObject lookPoint;

    public GameObject wall, shopeeper, ime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         Invoke("LateStart", 0.1f);
        
    }

    // Update is called once per frame
    void Update()
    {
       if( PartyManager.Instance.PM1 == true)
        {
            shopeeper.SetActive(true);
            ime.SetActive(true);
        }
        if(PartyManager.Instance.hasShopped == true && PartyManager.Instance.hasTut == true && PartyManager.Instance.PM1 == true)
        {
            wall.SetActive(false);
        }
    }

    public void LateStart()
    {
        if(PartyManager.Instance.PM1 == false)
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

       if(PartyManager.Instance.hasTut == false)
        {
            menuDialogue.StartDialogue();
        }
        
            
            
        
    }

    public void Shopped()
    {
        PartyManager.Instance.hasShopped = true;
    }
    public void Battled()
    {
        PartyManager.Instance.hasTut = true;
    }

     

    
   
}
