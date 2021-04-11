using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  AUTHOR: Pablo Enguix Llopis
 *  STATUS: WIP
 *  NAME: ComprobarSitio.cs
 *  GAMEOBJECT: All turrets
 *  DESCRIPTION: This script is used to check if, when the character wants to deploy a turret, if the place to deploy is free
 */

public class ComprobarSitio : MonoBehaviour
{
    [HideInInspector]
    public List<Collision> colliders = new List<Collision>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.GetContact(collision.contactCount - 1).thisCollider.gameObject.tag == "Torretas" &&
        collision.GetContact(collision.contactCount - 1).otherCollider.gameObject.layer == 6)
        {
            Debug.Log("Hey");
            Rigidbody rb = collision.GetContact(collision.contactCount - 1).thisCollider.gameObject.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        if(collision.GetContact(collision.contactCount - 1).thisCollider.gameObject.tag == "Torretas" &&
        collision.GetContact(collision.contactCount - 1).otherCollider.gameObject.tag == "Torretas")
        {
            Debug.Log("Hay colisión");
            colliders.Add(collision);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Torretas" && collision.gameObject.tag == "Torretas") 
        {
            Debug.Log("Ya no hay colisión");
            colliders.Remove(collision);
        }
    }
}
