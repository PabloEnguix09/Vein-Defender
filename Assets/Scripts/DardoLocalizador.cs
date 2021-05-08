using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: Personaje.cs
// STATUS: WIP
// GAMEOBJECT: DardoLocalizador
// DESCRIPTION: Este script contiene todo lo relacionado con el dardo localizador
//
// AUTHOR: Juan Ferrera Sala
// FEATURES ADDED: Creacion del dardo localizador
// ---------------------------------------------------

public class DardoLocalizador : MonoBehaviour
{

    public float velocidad = 50f;

    public float tiempoDeVida = 4f;

    public float contador;

    public Enemigo enemigo;

    // Start is called before the first frame update
    void Start()
    {
        contador = tiempoDeVida;
    }

    // Update is called once per frame
    void Update()
    {
        //Hacer que la bala se mueva.
        transform.position += transform.forward * velocidad * Time.deltaTime;

        //Comprobar si el dardo ha sido destruido
        tiempoDeVida -= Time.deltaTime;

        if (tiempoDeVida <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Recogemos todos los Enemigo de la zona
        GameObject[] Enemigo = GameObject.FindGameObjectsWithTag("Enemigo");

        for(int i = 0; i < Enemigo.Length; i++)
        {
            if (Enemigo[i].GetComponent<Enemigo>().marcado)
            {
                Enemigo[i].GetComponent<Enemigo>().marcado = false;

            }
        }

        enemigo = other.gameObject.GetComponent<Enemigo>();

        if(enemigo != null)
        {

            enemigo.marcado = true;

            Destroy(gameObject);
        }
    }
}
