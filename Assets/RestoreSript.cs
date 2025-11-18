
using Unity.VisualScripting;
using UnityEngine;

public class RestoreSript : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        for(int i = 0; i < PartyManager.Instance.party.Count; i++)
        {
            PartyManager.Instance.party[i].Restore();
        }
    }
}

   
