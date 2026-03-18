using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class ItemToolTipScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent<GameObject> hoverEvent;

     public UnityEvent unHoverEvent;

     public Item item;

      public void OnPointerEnter(PointerEventData eventData)
    {
        hoverEvent.Invoke(this.gameObject);
     
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        unHoverEvent.Invoke();
         
    }
}