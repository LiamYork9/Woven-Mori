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

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }

    public void StartDiolague()
    {
        textBox.Play("TextBoxAnimation");
        topBoxAnim.Play("TopBox");
        textComponent.text = string.Empty;
        index = 0;
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
            textBox.Play("CloseBox");
            topBoxAnim.Play("CloseTopBox");
            //gameObject.SetActive(false);
            Time.timeScale = 1.0f;
            textComponent.text = string.Empty;
            //topBox.SetActive(false);
        }
    }
    
}
