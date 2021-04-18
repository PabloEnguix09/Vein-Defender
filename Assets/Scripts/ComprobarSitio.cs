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
// ---------------------------------------------------

public class ComprobarSitio : MonoBehaviour
{
    [HideInInspector]
    public List<Collision> colliders = new List<Collision>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.GetContact(collision.contactCount - 1).thisCollider.gameObject.tag == "Torretas" &&
        collision.GetContact(collision.contactCount - 1).otherCollider.gameObject.layer == 6)
        {
            //Debug.Log("La torreta ha tocado el suelo");
            Rigidbody rb = collision.GetContact(collision.contactCount - 1).thisCollider.gameObject.GetComponent<Rigidbody>();
        }
        if(collision.GetContact(collision.contactCount - 1).thisCollider.gameObject.tag == "Previews" &&
        collision.GetContact(collision.contactCount - 1).otherCollider.gameObject.tag == "Torretas")
        {
            //Debug.Log("Hay colisión");
            colliders.Add(collision);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Torretas" && collision.gameObject.tag == "Torretas") 
        {
            //Debug.Log("Ya no hay colisión");
            colliders.Remove(collision);
        }
    }
}
