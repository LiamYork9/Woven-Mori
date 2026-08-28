using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ToolTipCondition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent<GameObject> hoverEvent;

     public UnityEvent unHoverEvent;

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
