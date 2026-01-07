using System;
using UnityEngine;

public class Follower: PlayerController
{

    public GameObject following = null;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movePoint.parent = null;
        if (following == null)
        {
            Follow(GameObject.FindWithTag("Player").GetComponent<PlayerController>());
        }
        else
        {
            if (following.GetComponent<PlayerController>().follower != this)
            {
                Follow(following.GetComponent<PlayerController>());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Following");
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }

    public void Move()
    {

        if (follower != null)
        {
            follower.Move();
        }
        movePoint.position = following.GetComponent<PlayerController>().movePoint.position;
    }    
    public void Follow(PlayerController target)
    {
        target.Follower(this);
    }

    public override void SpawnFollowers()
    {
        
    }
}
