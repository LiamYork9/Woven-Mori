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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToParty()
    {
        PartyManager.Instance.AddPartyMemeber(partyMember);
        if (willFollow)
        {
            if (follower.GetComponent<Follower>() != null)
            {
                  follower.GetComponent<Follower>().Follow(GameObject.FindWithTag("Player").GetComponent<PlayerController>());
            }
            else
            {
               Follower temp = follower.AddComponent<Follower>();
               temp.movePoint = Instantiate(movePoint,follower.transform.position,follower.transform.rotation).transform;
            }
        }
    }
}
