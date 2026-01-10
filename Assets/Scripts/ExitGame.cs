using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
        PlayerPrefs.DeleteAll();
    }
}
