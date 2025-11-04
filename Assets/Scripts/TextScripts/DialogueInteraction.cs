using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour
{
     public bool InRange = false;

     public GameObject DiologueBox;

    public GameObject topBox;

     public Dialogue text;

     public bool pauseGame = false;

     public bool once = false;

    public bool notAgain = false;

    public GameObject skip;

    public bool scriptedText;

    



    // Start is called before the first frame update
    void Start()
    {
        
        DiologueBox.SetActive(false);
        topBox.SetActive(false);
        skip.SetActive(false);
    }
 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DiologueBox.SetActive(true);
            topBox.SetActive(true);
            skip.SetActive(true);

            text.StartDiolague();
        }

         if(InRange == true)
            {
                if (Input.GetKeyDown(KeyCode.Space)&& text.textActive == false && scriptedText == false)
               {
                    
                    DiologueBox.SetActive(true);
                    topBox.SetActive(true);
                    skip.SetActive(true);
                    text.StartDiolague();
                    

                }
            }

        
    }

    void OnTriggerEnter2D(Collider2D other)
    {



        if (other.gameObject.CompareTag("Player"))
        {
            InRange = true;
        }
        
        if (other.gameObject.CompareTag("Player") && scriptedText == true)
        {
            DiologueBox.SetActive(true);
            topBox.SetActive(true);
            skip.SetActive(true);
            text.StartDiolague();
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

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            InRange = false;
        }
    }


    
}

