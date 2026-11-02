using UnityEngine;

public class DemoChecks : MonoBehaviour
{
    public bool hasShopped;
    public bool hasTut;
    
    public GameObject wall;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasShopped == true && hasTut == true)
        {
            wall.SetActive(false);
        }
    }
}
