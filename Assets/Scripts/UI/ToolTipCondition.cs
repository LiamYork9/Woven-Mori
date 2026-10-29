using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class ToolTipCondition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent<GameObject> hoverEvent;

     public UnityEvent unHoverEvent;
     
     public TMP_Text text;

     public Condition condition;

      public void OnPointerEnter(PointerEventData eventData)
    {
        hoverEvent.Invoke(this.gameObject);
     
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        unHoverEvent.Invoke();
         
    }
}
