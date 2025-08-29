using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using MoriSkills;
public class ToolTipSkill : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    public SkillId skillId;

    public UnityEvent<GameObject> hoverEvent;
    public UnityEvent unHoverEvent;
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverEvent.Invoke(this.gameObject);
        Debug.Log("hover happen");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        unHoverEvent.Invoke();
         Debug.Log("unhover happen");
    }

 
}
