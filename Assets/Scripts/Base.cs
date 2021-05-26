using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: Base.cs
    // STATUS: WIP
    // GAMEOBJECT: base
    // DESCRIPTION: Este script recoge las caracteristicas de la base. Por ahora, la salud.
    //
    // AUTHOR: Jorge Grau
    // FEATURES ADDED: Salud de la base
    // ---------------------------------------------------

    [SerializeField]
    public float salud = 1;

    public GameObject modelo;

    public float Salud
    {
        get { return salud; }

        set
        {
            value = Mathf.Clamp01(value);
            salud = value;
            if (salud <= 0)
            {
                // Debug.Log("Destruido");
                modelo.SetActive(false);
            }
        }
    }

    private void Start()
    {
        modelo.SetActive(true);
    }

    public void RecibirAtaque(Ataque ataque)
    {
        salud -= ataque.fuerza;
        if(salud <= 0)
        {
            Destroy(gameObject);
        }
    }
}
