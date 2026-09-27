using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class ZoneCheck : MonoBehaviour
{
    public bool activeZone = false;
    public Sprite background;
     
     private void OnTriggerEnter2D(Collider2D collison)
    {
        if (collison.gameObject.CompareTag("Player"))
        {
          
            activeZone = true;
            PartyManager.Instance.battleBackground = background;
        }
    }

    private void OnTriggerExit2D(Collider2D collison)
    {
        if (collison.gameObject.CompareTag("Player"))
        {
            activeZone = false;
        }
    }
    
    
   
}
