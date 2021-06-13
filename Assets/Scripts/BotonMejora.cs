using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BotonMejora : MonoBehaviour, IPointerClickHandler
{
    public SeleccionarMejora mejora;
    public Button boton;
    public Sprite botonNormal;
    public Sprite botonPulsado;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(boton.GetComponent<Image>().sprite.Equals(boton))
        {
            boton.GetComponent<Image>().sprite = botonPulsado;
        }
        else 
        {
            boton.GetComponent<Image>().sprite = botonNormal;
            mejora.MostrarMejoras();
        }
    }
}
