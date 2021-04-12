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

    public void maximaVida(float vida)
    {
        sliderVida.maxValue = vida;
        sliderVida.value = vida;
    }
    public void controlVida(float vida)
    {
        sliderVida.value = vida;
        Debug.Log("HeyEscucha");
    }
}
