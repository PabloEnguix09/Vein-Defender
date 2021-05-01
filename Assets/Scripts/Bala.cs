using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: Bala.cs
    // STATUS: WIP
    // GAMEOBJECT: Bala
    // DESCRIPTION: Control de las balas disparadas por las torretas
    //
    // AUTHOR: Adri�n
    // FEATURES ADDED: La bala con impulso y se destruye al golpear algo.
    //
    // AUTHOR: Pau
    // FEATURES ADDED: Inflingir daño a player, torretas y base
    // ---------------------------------------------------


    public float velocidad;

    public float fuerza;

    public float radioExplosion;

    public float danyoExplosion;

    public GameObject explosion;

    Rigidbody rb;

    void Start()
    {
        velocidad = 100f;
        rb = GetComponent<Rigidbody>();

        //Darle impulso de la bala
        rb.AddForce(transform.forward * velocidad, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // compruebo si golpeo una base y aplico daño
       
        if (collision.gameObject.CompareTag("Bases"))
        {
            Base estructura = collision.gameObject.gameObject.GetComponent<Base>();
            estructura.Salud -= fuerza;
            
        }
        // compruebo si golpeo una torreta y aplico daño
        else if (collision.gameObject.CompareTag("Torretas"))
        {
            Torreta estructura = collision.gameObject.gameObject.GetComponent<Torreta>();

            if (estructura.escudoActual > 0)
            {
                if (estructura.escudoActual < fuerza)
                {
                    float aux = fuerza - estructura.escudoActual;
                    estructura.escudoActual = 0;
                    estructura.vidaActual -= aux;
                }
                else
                {
                    estructura.escudoActual -= fuerza;
                }
            }
            else
            {
                estructura.vidaActual -= fuerza;
            }
            
        }
        // compruebo si golpeo un jugador y aplico daño
        else if (collision.gameObject.CompareTag("Player"))
        {
            Personaje personaje = collision.gameObject.gameObject.GetComponent<Personaje>();
            personaje.RecibirAtaque(fuerza);
            
        }
    // destruyo la bala
    Destroy(gameObject);
    }
}
