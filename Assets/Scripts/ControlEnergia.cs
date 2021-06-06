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
    // FEATURES ADDED: Se muestra la energia del personaje y se actualiza cuando cambia la propiedad Energia, se usa un gradiente para se�alizar el gasto.
    // AUTHOR: Luis Belloch
    // FEATURES ADDED: El slider se actualiza a tiempo real.
    // ---------------------------------------------------

    public Slider sliderEnergia;
    public Gradient gradienteEnergia;
    public Image imagen;
    public Text texto;

    Personaje personaje;

    float energiaMax;

    private void Start()
    {
        personaje = FindObjectOfType<Personaje>();
    }
    public void maximaEnergia(float energia)
    {
        energiaMax = energia;
        sliderEnergia.maxValue = energia;
        sliderEnergia.value = energiaMax - energia;
        texto.text = (energiaMax - energia).ToString() + "%"; 
        
        imagen.color = gradienteEnergia.Evaluate(1f);
    }

    private void Update()
    {
        sliderEnergia.value = energiaMax - personaje.Energia;
        imagen.color = gradienteEnergia.Evaluate(sliderEnergia.normalizedValue);
        texto.text = ((energiaMax - personaje.Energia)*10).ToString() + "%";
    }
}
