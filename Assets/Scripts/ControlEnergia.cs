using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlEnergia : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: ContralEnergia.cs
    // STATUS: WIP
    // GAMEOBJECT: Energia
    // DESCRIPTION: La barra de vida muestra la energia del personaje, se llena conforme usa energia.
    //
    // AUTHOR: Jorge Grau
    // FEATURES ADDED: Se muestra la energia del personaje y se actualiza cuando cambia la propiedad Energia, se usa un gradiente para señalizar el gasto.
    // ---------------------------------------------------

    public Slider sliderEnergia;
    public Gradient gradienteEnergia;
    public Image imagen;

    float energiaMax;
    public void maximaEnergia(float energia)
    {
        energiaMax = energia;
        sliderEnergia.maxValue = energia;
        sliderEnergia.value = energiaMax - energia;
        
        imagen.color = gradienteEnergia.Evaluate(1f);
    }
    public void controlEnergia(float energia)
    {
        Debug.Log(energia.ToString());
        sliderEnergia.value = energiaMax - energia;
        imagen.color = gradienteEnergia.Evaluate(sliderEnergia.normalizedValue);
    }
}
