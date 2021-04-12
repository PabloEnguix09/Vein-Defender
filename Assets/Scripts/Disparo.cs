using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparo : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: nombre
    // STATUS: estado
    // GAMEOBJECT: objeto
    // DESCRIPTION: descripcion
    //
    // AUTHOR: autor
    // FEATURES ADDED: cosas hechas
    // ---------------------------------------------------

    public GameObject bala;

    public void Disparar()
    {
        bala.GetComponent<Bala>().fuerza = GetComponentInParent<Torreta>().fuerza;
        Instantiate(bala, transform.position, transform.rotation);
    }
}
