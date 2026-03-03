using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class EquipmentToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent<GameObject> hoverEvent;

     public UnityEvent unHoverEvent;

     public Equipment equipment;

      public void OnPointerEnter(PointerEventData eventData)
    {
        hoverEvent.Invoke(this.gameObject);
     
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        unHoverEvent.Invoke();
         
    }
    
}
