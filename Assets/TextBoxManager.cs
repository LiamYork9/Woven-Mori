using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour
{
    public static TextBoxManager Instance;

    public static TextBoxManager GetInstance()
    {
        return Instance;
    }
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI skipText;

    public Image portrait;

    public TMP_Text nameText;
    public GameObject buttonPrefab;

    public Transform buttonParent;

     public GameObject topBox;

    public Animator textBox;

    public Animator topBoxAnim;

    public GameObject skip;

    public GameObject nameTextObj;

     public GameObject DiologueBox;
     


     public void Awake()
    {
        if (TextBoxManager.Instance != this && TextBoxManager.Instance != null)
        {
            Destroy(TextBoxManager.Instance);
            Instance = this;
        }
        else
        {
            Instance = this;
        }

    }
  
    void Start()
    {
        
       Instance.DiologueBox.SetActive(false);
       Instance.topBox.SetActive(false);
       Instance.skip.SetActive(false);
       Instance.nameTextObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
