using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiolagueInteraction : MonoBehaviour
{
     public bool InRange = false;

     public GameObject DiologueBox;

    public GameObject topBox;

     public Dialogue text;

     public bool pauseGame = false;

     public bool once = false;

     public bool notAgain = false;





    // Start is called before the first frame update
    void Start()
    {
        DiologueBox.SetActive(false);
        topBox.SetActive(false);
    }
 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DiologueBox.SetActive(true);
            topBox.SetActive(true);

            text.StartDiolague();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

    
        if (other.gameObject.CompareTag("Player"))
        {
            DiologueBox.SetActive(true);
            topBox.SetActive(true);

            text.StartDiolague();
            Debug.Log("InText");

        }

        if (other.gameObject.CompareTag("Player")&& pauseGame == true && once == false){
             DiologueBox.SetActive(true);
           
            text.StartDiolague();
            Debug.Log("InText");
            Time.timeScale = 0.0f;
             
            once = true;

        }

        if(other.gameObject.CompareTag("Player")&& notAgain == true && once == false)
        {
            DiologueBox.SetActive(true);
            text.StartDiolague();
            Debug.Log("InText");
            
            once = true;
        }

        if(other.gameObject.CompareTag("Player")&& notAgain == true && once == false &&  notAgain == false)
        {
            DiologueBox.SetActive(true);
            text.StartDiolague();
            Debug.Log("InText");
            
            once = true;
        }

       
    }

     void OnTriggerExit(Collider other) 
    {
        if(InRange == true)
        {
         DiologueBox.SetActive(false);
         text.textComponent.text = string.Empty;
        }
        
    }
    
}

