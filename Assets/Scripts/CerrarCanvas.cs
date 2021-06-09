using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CerrarCanvas : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    public GameObject canvas;
    public void OnPointerDown(PointerEventData eventData)
    {
        canvas.SetActive(false);
    }
}
