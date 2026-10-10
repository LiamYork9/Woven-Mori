using System;
using UnityEngine;

public class Follower: PlayerController
{

    public GameObject following = null;
    public int followNumber = 0;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
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
        if (Vector3.Distance(transform.position, movePoint.position) < .01f)
        {
            animator.SetBool("walking", false);
        }
        else
        {
            animator.SetBool("walking", true);

            if(transform.position.x - movePoint.position.x > .05f)
            {
                animator.SetInteger("facing",1);
            }
            else if(movePoint.position.x - transform.position.x > .05f)
            {
                
                animator.SetInteger("facing",3);
            }
            if(transform.position.y - movePoint.position.y > .05f)
            {
                animator.SetInteger("facing",0);
            }
            else if(movePoint.position.y - transform.position.y > .05f)
            {
                
                animator.SetInteger("facing",2);
            }
        }

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
        target.AddFollower(this);
        if(followNumber<PartyManager.Instance.party.Count)
        {
            animator.runtimeAnimatorController = PartyManager.Instance.party[followNumber].animator;
        }
    }
    
    public override void AddFollower(Follower newFollower)
    {
        if (follower == null)
        {
            follower = newFollower;
            newFollower.following = gameObject;
            newFollower.followNumber = followNumber+1;
        }
        else
        {
            follower.AddFollower(newFollower);
        }
        SetPosition();
    }
    public override void SpawnFollowers()
    {
        
    }
}
