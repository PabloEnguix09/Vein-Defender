using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorretaBasica : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: nombre
    // STATUS: estado
    // GAMEOBJECT: objeto
    // DESCRIPTION: descripcion
    //
    // AUTHOR: Adrián
    // FEATURES ADDED: Añadidos valores de fuerza, vida, velocidadAtaque y rango
    //
    // AUTHOR: Jorge
    // FEATURES ADDED: Añadido gasto energetico
    // ---------------------------------------------------

    [Range(0, 1)]
    [SerializeField]
    public float vida = 0.3f;

    [Range(0, 1)]
    [SerializeField]
    public float fuerza = 0f;

    public float velocidadAtaque = 1f;

    public float rango = 40f;

    public float gastoEnergia = 1;

    private Torreta torreta;

    // Start is called before the first frame update
    void Start()
    {
        //asignar los valores a la torreta
        torreta = gameObject.GetComponent<Torreta>();
        torreta.fuerza = fuerza;
        torreta.vida = vida;
        torreta.velocidadAtaque = velocidadAtaque;
        torreta.rango = rango;
        torreta.gastoEnergia = gastoEnergia;
    }
}
