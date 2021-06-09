using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
// ---------------------------------------------------
// NAME: DragDrop.cs
// STATUS: DONE
// GAMEOBJECT: Objetos de UI de torretas
// DESCRIPTION: Objeto de UI seleccionable
//
// AUTHOR: Jorge Grau
// FEATURES ADDED: propiedades base
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: toma del indice de la torreta, se pone el sprite de la torreta
// ---------------------------------------------------
public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler {

    Canvas canvas;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    TorretasDisponibles torretasDisponibles;

    public int indiceTorreta;

    private void Awake()
    {
        torretasDisponibles = FindObjectOfType<TorretasDisponibles>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = transform.GetComponentInParent<Canvas>();

        // Pone el sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = torretasDisponibles.torretasTotales[indiceTorreta].visual.imagen;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Input.GetAxis("Fire1") > 0;
    }
}
