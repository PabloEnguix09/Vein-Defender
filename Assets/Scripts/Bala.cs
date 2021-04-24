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

        if (radioExplosion > 0)
        {
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, radioExplosion);

            // Inflinge daño a todos los objetivos dentro del rango
            for (int i = 0; i < colliders.Length; i++)
            {


                if (colliders[i].CompareTag("Enemigos"))
                {
                    Enemigo otroEnemigo = colliders[i].gameObject.GetComponent<Enemigo>();
                    otroEnemigo.vidaActual -= danyoExplosion;
                    Debug.Log("Golpeo un enemigo");
                }

            }
        }
        Debug.Log("Explota");
        Destroy(gameObject);
    }
}
