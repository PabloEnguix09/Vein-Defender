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
    // AUTHOR: Adrián
    // FEATURES ADDED: La bala con impulso y se destruye al golpear algo.
    // ---------------------------------------------------

    public float velocidad;

    public float fuerza;

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
        //Destruirse al golpear
        Destroy(gameObject);
    }
}
