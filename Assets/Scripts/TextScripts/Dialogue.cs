using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;

    public TextMeshProUGUI skipText;
    public string[] lines;
    public float textSpeed;

    private int index;

    public GameObject topBox;

    public Animator textBox;

    public Animator topBoxAnim;

    public GameObject skip;

    public PlayerController pc;

    public bool textActive = false;


    void Start()
    {
        
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
        if (textComponent.text == lines[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            textComponent.text = lines[index];
        }
    }
    
     

    public void StartDiolague()
    {
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
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(textSpeed);
        }

    }

    public void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            textActive = false;
            pc.inText = false;
            textBox.Play("CloseBox");
            topBoxAnim.Play("CloseTopBox");
            //gameObject.SetActive(false);
            skip.SetActive(false);
            Time.timeScale = 1.0f;
            textComponent.text = string.Empty;
            //topBox.SetActive(false);
        }
    }
    
}
