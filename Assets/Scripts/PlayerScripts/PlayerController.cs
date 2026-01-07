using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public Transform movePoint;
    public LayerMask stopsMovement;

    public bool inText;
    
    public Follower follower = null;

    private float tempDist = 0;

    public int stepCount;

    public bool stepCheck;
    public UnityEvent Step;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movePoint.parent = null;
        SpawnFollowers();
        if (Step == null)
        {
            Step = new UnityEvent();
        }
        Step.AddListener(TakeStep);
    }

    // Update is called once per frame
    void Update()
    {
      
        if (inText == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, movePoint.position) < .05f)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) >= 0.9f)
                {
                    tempDist = Input.GetAxisRaw("Horizontal");
                    tempDist /= Math.Abs(tempDist);
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(tempDist, 0f, 0f), 0.2f, stopsMovement))
                    {
                        if (follower != null&&!stepCheck)
                        {
                            follower.Move();
                        }
                        stepCheck = true;
                        movePoint.position += new Vector3(tempDist, 0f, 0f);
                    }
                }

                if (Mathf.Abs(Input.GetAxisRaw("Vertical")) >= 0.9f)
                {
                    tempDist = Input.GetAxisRaw("Vertical");
                    tempDist /= Math.Abs(tempDist);
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, tempDist, 0f), 0.2f, stopsMovement))
                    {
                        if (follower != null&&!stepCheck)
                        {
                            follower.Move();
                        }
                        stepCheck = true;
                        movePoint.position += new Vector3(0f, tempDist, 0f);
                    }
                }
            }
            if (stepCheck == true)
            {
                Step.Invoke();
                stepCheck = false;
            }
            
        }
    }

    public void Follower(Follower newFollower)
    {
        if (follower == null)
        {
            follower = newFollower;
            newFollower.following = gameObject;
        }
        else
        {
            follower.Follower(newFollower);
        }
        SetPosition();
    }

    public void TakeStep()
    {
        stepCount += 1;
        //eventually add check to see if this is an encounter tile
        EncounterManager.Instance.EncounterCheck();
    }
    public void SetPosition()
    {
        movePoint.position = transform.position;
        if (follower != null)
        {
            follower.transform.position = transform.position;
            follower.SetPosition();
        }
    }

    public virtual void SpawnFollowers()
    {
        for(int i = 0; i < PartyManager.Instance.followers.Count; i++)
        {
            Instantiate(PartyManager.Instance.followers[i],gameObject.transform.position,gameObject.transform.rotation);
        }
    }
}
