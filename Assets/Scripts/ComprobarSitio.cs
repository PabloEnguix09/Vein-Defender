using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComprobarSitio : MonoBehaviour
{
    [HideInInspector]
    public List<Collision> colliders = new List<Collision>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.GetContact(collision.contactCount - 1).thisCollider.gameObject.tag == "Torreta" &&
        collision.GetContact(collision.contactCount - 1).otherCollider.gameObject.name == "Terreno")
        {
            Debug.Log("Hey");
            Rigidbody rb = collision.GetContact(collision.contactCount - 1).thisCollider.gameObject.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        if(collision.GetContact(collision.contactCount - 1).thisCollider.gameObject.tag == "Torreta" &&
        collision.GetContact(collision.contactCount - 1).otherCollider.gameObject.tag == "Torreta")
        {
            Debug.Log("Hay colisión");
            colliders.Add(collision);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Torreta" && collision.gameObject.tag == "Torreta") 
        {
            Debug.Log("Ya no hay colisión");
            colliders.Remove(collision);
        }
    }
}
