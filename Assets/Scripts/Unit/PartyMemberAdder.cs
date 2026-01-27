using Unity.VisualScripting;
using UnityEngine;

public class PartyMemberAdder : MonoBehaviour
{

    public PlayerCharacter partyMember;
    public bool willFollow = true;

    public GameObject follower;

    public GameObject movePoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(PartyManager.Instance.PM1 == true)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToParty()
    {
        PartyManager.Instance.PM1 = true;
        PartyManager.Instance.AddPartyMemeber(partyMember);
       Instantiate(follower,gameObject.transform.position,gameObject.transform.rotation);
       PartyManager.Instance.followers.Add(follower);
       Destroy(gameObject);
        
    }
}
