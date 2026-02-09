using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
  

    public DialogGraph lines;


    public DialogSegment activeSegment;
    public float textSpeed;

    private int index;


    public PlayerController pc;

    public bool textActive = false;
    public UnityEvent EndDialogueEvent;

    public UnityEvent<string> StartNodeEvent;

    public UnityEvent<string> EndNodeEvent;

     public string[] dialogText;


    void Start()
    {
        if (EndDialogueEvent == null)
        {
            EndDialogueEvent = new UnityEvent();
        }

        if (EndNodeEvent == null)
        {
            EndNodeEvent = new UnityEvent<string>();
        }

        if (StartNodeEvent == null)
        {
            StartNodeEvent = new UnityEvent<string>();
        }
       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && textActive == true)
        {
            LineSkip();
        }

    }

    public void LineSkip()
    {
        
        if ( TextBoxManager.Instance.textComponent.text == activeSegment.DialogText[index])
        {
             if (index < activeSegment.DialogText.Length - 1)
             {
                NextLine();
             }
            else
            {
                NextNode();
            }
        }
        else
        {
            StopAllCoroutines();
             TextBoxManager.Instance.textComponent.text = activeSegment.DialogText[index];
        }
    }
    
     

    public void StartDiolague()
    {
        Debug.Log("Start Text");
         foreach (DialogSegment node in lines.nodes)
            {
                if (!node.GetInputPort("input").IsConnected)
                {
                    UpdateDialog(node);
                }
            }
         TextBoxManager.Instance.nameText.text = activeSegment.speakerName;
         TextBoxManager.Instance.portrait.sprite = activeSegment.portrait;
        textActive = true;
        pc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        TextBoxManager.Instance.textBox.Play("TextBoxAnimation");
         TextBoxManager.Instance.topBoxAnim.Play("TopBox");
         TextBoxManager.Instance.textComponent.text = string.Empty;
        index = 0;
        pc.inText = true;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in activeSegment.DialogText[index].ToCharArray())
        {
             TextBoxManager.Instance.textComponent.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }

    }

    public void NextLine()
    {
       
        
            index++;
             TextBoxManager.Instance.textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        
      
    }

    public void NextNode()
    {
        for(int i = 0; i < activeSegment.endNodeEvent.Count;i++)
        {
            EndNodeEvent.Invoke(activeSegment.endNodeEvent[i]);
        }
        if(activeSegment is DialogAnswerSegments){
            
            if((activeSegment as DialogAnswerSegments).Answers.Count > 0)
            {
                 int answerIndex = 0;
                  foreach (Transform child in  TextBoxManager.Instance.buttonParent)
                    {
                        Destroy(child.gameObject);
                    }
                 
                 foreach(string answer in (activeSegment as DialogAnswerSegments).Answers)
                {
                   
                    GameObject btn = Instantiate( TextBoxManager.Instance.buttonPrefab,  TextBoxManager.Instance.buttonParent);
                    btn.GetComponentInChildren<TMP_Text>().text = answer;

                    int index = answerIndex;

                     btn.GetComponentInChildren<Button>().onClick.AddListener((() => { AnswerClicked(index); }));

                    answerIndex++;
                }
            }
            else
            {
                if (activeSegment.GetPort("output").IsConnected)
                {
                    UpdateDialog(activeSegment.GetPort("output").Connection.node as DialogSegment);
                     TextBoxManager.Instance.textComponent.text = string.Empty;
                    StartCoroutine(TypeLine());
                }
                else
                {
                    EndDialogue();
                }
            }
           
        }
        
           
          
        else if(activeSegment is RetalkSegment)
        {
            
            if(!(activeSegment as RetalkSegment).hasTalked)
            {
                 (activeSegment as RetalkSegment).hasTalked = true;
                UpdateDialog(activeSegment.GetPort("outputPaths 0").Connection.node as DialogSegment);
                TextBoxManager.Instance.textComponent.text = string.Empty;
                StartCoroutine(TypeLine());
              
            }
            else
            {
                UpdateDialog(activeSegment.GetPort("outputPaths 1").Connection.node as DialogSegment);
                TextBoxManager.Instance.textComponent.text = string.Empty;
                StartCoroutine(TypeLine());
                
            }
        }
          
        else
        {
             if (activeSegment.GetPort("output").IsConnected)
            {
                UpdateDialog(activeSegment.GetPort("output").Connection.node as DialogSegment);
                  TextBoxManager.Instance.textComponent.text = string.Empty;
                 StartCoroutine(TypeLine());
            }
            else
            {
                EndDialogue();
            }
           
        }
        
    
           
        
}

     public void AnswerClicked(int clickedIndex)
    {
        XNode.NodePort port = activeSegment.GetPort("Answers " + clickedIndex);
        if (port.IsConnected)
        {
            UpdateDialog(port.Connection.node as DialogSegment);
            LineSkip();
        }

        else
        {
            EndDialogue();
        }
            
    }

    public void EndDialogue()
    {
        Debug.Log("End here");
       
         foreach (Transform child in  TextBoxManager.Instance.buttonParent)
        {
            Destroy(child.gameObject);
        }
        textActive = false;
        pc.inText = false;
        TextBoxManager.Instance.textBox.Play("CloseBox");
        TextBoxManager.Instance.topBoxAnim.Play("CloseTopBox");
        //gameObject.SetActive(false);
        TextBoxManager.Instance.skip.SetActive(false);
        TextBoxManager.Instance.nameTextObj.SetActive(false);
        Time.timeScale = 1.0f;
         TextBoxManager.Instance.textComponent.text = string.Empty;
        //topBox.SetActive(false);
        EndDialogueEvent.Invoke();
        
    }

     private void UpdateDialog(DialogSegment newSegment)
        {
            index = 0;
            activeSegment = newSegment;
            for(int i = 0; i < activeSegment.startNodeEvent.Count;i++)
            {
                StartNodeEvent.Invoke(activeSegment.startNodeEvent[i]);
            }
            dialogText = newSegment.DialogText;
             TextBoxManager.Instance.nameText.text = activeSegment.speakerName;
             TextBoxManager.Instance.portrait.sprite = activeSegment.portrait;
             foreach (Transform child in  TextBoxManager.Instance.buttonParent)
            {
                Destroy(child.gameObject);
            }

        }

}
