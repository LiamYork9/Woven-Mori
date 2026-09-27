

using UnityEngine;
using UnityEngine.SceneManagement;

public class RestoreScript : MonoBehaviour
{


    public Vector3 restLocation;

    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        for(int i = 0; i < PartyManager.Instance.party.Count; i++)
        {
            PartyManager.Instance.party[i].Restore();
        }

        PartyManager.Instance.restLocation = restLocation;
        PartyManager.Instance.bonfire = SceneManager.GetActiveScene().name;

    }
}

   
