using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;

    public TextMeshProUGUI skipText;
    public DialogGraph lines;

    public GameObject buttonPrefab;

    public Transform buttonParent;


    public DialogSegment activeSegment;
    public float textSpeed;

    private int index;

    public GameObject topBox;

    public Animator textBox;

    public Animator topBoxAnim;

    public GameObject skip;

    public PlayerController pc;

    public bool textActive = false;
    public UnityEvent EndDialogueEvent;

     public string[] dialogText;


    void Start()
    {
        if (EndDialogueEvent == null)
        {
            EndDialogueEvent = new UnityEvent();
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
        
        if (textComponent.text == activeSegment.DialogText[index])
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
            textComponent.text = activeSegment.DialogText[index];
        }
    }
    
     

    public void StartDiolague()
    {
         foreach (DialogSegment node in lines.nodes)
            {
                if (!node.GetInputPort("input").IsConnected)
                {
                    UpdateDialog(node);
                }
            }
        textActive = true;
        pc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        textBox.Play("TextBoxAnimation");
        topBoxAnim.Play("TopBox");
        textComponent.text = string.Empty;
        index = 0;
        pc.inText = true;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in activeSegment.DialogText[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }

    }

    public void NextLine()
    {
       
        
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        
      
    }

    public void NextNode()
    {
        if(!(activeSegment is DialogAnswerSegments)){
        
            if (activeSegment.GetPort("output").IsConnected)
            {
                UpdateDialog(activeSegment.GetPort("output").Connection.node as DialogSegment);
                 textComponent.text = string.Empty;
                 StartCoroutine(TypeLine());
            }
            else
            {
                EndDialogue();
            }
        }
        else
        {
            if((activeSegment as DialogAnswerSegments).Answers.Count > 0)
            {
                 int answerIndex = 0;
                 
                 foreach(string answer in (activeSegment as DialogAnswerSegments).Answers)
                {
                    GameObject btn = Instantiate(buttonPrefab, buttonParent);
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
                    textComponent.text = string.Empty;
                    StartCoroutine(TypeLine());
                }
                else
                {
                    EndDialogue();
                }
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
         foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }
        textActive = false;
        pc.inText = false;
        textBox.Play("CloseBox");
        topBoxAnim.Play("CloseTopBox");
        //gameObject.SetActive(false);
        skip.SetActive(false);
        Time.timeScale = 1.0f;
        textComponent.text = string.Empty;
        //topBox.SetActive(false);
        EndDialogueEvent.Invoke();
        
    }

     private void UpdateDialog(DialogSegment newSegment)
        {
            index = 0;
            activeSegment = newSegment;
            dialogText = newSegment.DialogText;
             foreach (Transform child in buttonParent)
            {
                Destroy(child.gameObject);
            }

        }

}
