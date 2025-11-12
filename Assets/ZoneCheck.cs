using UnityEngine;

public class ZoneCheck : MonoBehaviour
{
    public bool activeZone = false;
     
     private void OnTriggerEnter2D(Collider2D collison)
    {
        if (collison.gameObject.CompareTag("Player"))
        {
          
            activeZone = true;
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
