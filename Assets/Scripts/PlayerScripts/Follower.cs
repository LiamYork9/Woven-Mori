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
        movePoint.position = following.transform.position;
        if (follower != null)
        {
            follower.Move();
        }
    }    
    public void Follow(PlayerController target)
    {
        target.Follower(this);
    }
}
