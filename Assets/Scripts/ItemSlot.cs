using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public HUD hud;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null) {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            hud.recibirMovimientos(this.name, eventData.pointerDrag.name);
            
        }
        
    }
}
