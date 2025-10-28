using System;
using UnityEngine;

public class Follower: PlayerController
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask stopsMovement;

    public GameObject following = null;
    public Follower follower = null;
    

    private float tempDist = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
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
                    Debug.Log(tempDist + " Horizontal");
                    movePoint.position += new Vector3(tempDist, 0f, 0f);
                }
            }

            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) >= 0.9f)
            {
                tempDist = Input.GetAxisRaw("Vertical");
                tempDist /= Math.Abs(tempDist);
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, tempDist, 0f), 0.2f, stopsMovement))
                {
                    Debug.Log(tempDist + " Vertical");
                    movePoint.position += new Vector3(0f, tempDist, 0f);
                }
            }
        }
    }
    
    public void Follow(PlayerController target)
    {
        
    }
}
