using System;
using System.Reflection;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask stopsMovement;

    public bool inText;

    public int battleCheck = 1;

    public int encounterMod;

    public int min;

    public int max;
    
    
    public Follower follower = null;

    private float tempDist = 0;

    public int stepCount;

    public bool stepCheck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movePoint.parent = null;
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
                stepCount += 1;
                battleCheck = UnityEngine.Random.Range(min, max);
                stepCheck = false;
                FightCheck();
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
    }
    
    public void FightCheck()
    {
        if(battleCheck <= stepCount + encounterMod)
        {
            Debug.Log("FIGHT!");
            stepCount = 0;
                
        }
    }
}
