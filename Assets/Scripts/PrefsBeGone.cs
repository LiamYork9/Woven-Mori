using UnityEngine;

public class PrefsBeGone : MonoBehaviour
{
    
    public void OnTriggerStay2D(Collider2D collision)
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Prefs gone");
     
    }
}
