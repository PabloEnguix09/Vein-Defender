using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: nombre
// STATUS: estado
// GAMEOBJECT: objeto
// DESCRIPTION: descripcion
//
// AUTHOR: autor
// FEATURES ADDED: cosas hechas
//
// AUTHOR: Adrian Maldonado
// FEATURES ADDED: Comprobacion de torretas, props y escudos
// ---------------------------------------------------

public class ComprobarSitio : MonoBehaviour
{
    public List<Collision> colliders = new List<Collision>();

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Torreta" || collision.gameObject.tag == "Prop" || collision.gameObject.tag == "Escudo")
        {
            //Debug.Log("Hay colisión con una torreta");
            colliders.Add(collision);
        }
        

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Torreta" && collision.gameObject.tag == "Torreta") 
        {
            //Debug.Log("Ya no hay colisión");
            colliders.Remove(collision);
        }
    }
}
