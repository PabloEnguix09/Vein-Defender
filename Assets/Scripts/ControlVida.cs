using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlVida : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: ContralVida.cs
    // STATUS: WIP
    // GAMEOBJECT: Vida
    // DESCRIPTION: La barra de vida muestra la vida del personaje
    //
    // AUTHOR: Jorge Grau
    // FEATURES ADDED: Se muestra la vida del personaje y se actualiza cuando cambia la propiedad Salud
    // ---------------------------------------------------

    public Slider sliderVida;
    public Text texto;

    Personaje personaje;

    private void Start()
    {
        personaje = FindObjectOfType<Personaje>();
    }

    public void maximaVida(float vida)
    {
        sliderVida.maxValue = vida;
        sliderVida.value = vida;
        texto.text = (vida*10f).ToString();
    }


    private void Update()
    {
        sliderVida.value = personaje.Salud;
        texto.text = (personaje.Salud * 10f).ToString();
    }
}
