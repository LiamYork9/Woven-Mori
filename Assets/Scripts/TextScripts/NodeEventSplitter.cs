using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NodeEventSplitter : MonoBehaviour
{
    public List<UnityEvent> eventList;

    public List<string> eventNames;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ListenEvent(string heard)
    {
        for(int i=0; i<eventNames.Count; i++)
        {
            if(heard == eventNames[i])
            {
                eventList[i].Invoke();
            }
        }
    }
}
