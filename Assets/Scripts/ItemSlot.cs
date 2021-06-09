using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// ---------------------------------------------------
// NAME: ItemSlot.cs
// STATUS: DONE
// GAMEOBJECT: Slot de torreta
// DESCRIPTION: Slot donde poner torretas seleccionadas
//
// AUTHOR: Jorge Grau
// FEATURES ADDED: propiedades base
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: propiedades base
// ---------------------------------------------------
public class ItemSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public HUD hud;
    public int posicion;
    public int indiceTorretaActual;
    public Image imagen;
    public Sprite cuadrado;
    public Sprite cuadradoEncima;

    private void Start()
    {
        hud = FindObjectOfType<HUD>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null) {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            indiceTorretaActual = eventData.pointerDrag.GetComponent<DragDrop>().indiceTorreta;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        imagen.sprite = cuadradoEncima;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        imagen.sprite = cuadrado;
    }
}
