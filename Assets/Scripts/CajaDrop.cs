using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: CajaDrop.cs
// STATUS: WIP
// GAMEOBJECT: Caja Drop
// DESCRIPTION: controla el comportamiento de la caja de drops
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: invocacion de torreta y disolver la caja
// ---------------------------------------------------
public class CajaDrop : MonoBehaviour
{
    public GameObject torreta;
    public Transform centro;
    Animator animator;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Terreno")
        {
            // Se paraliza la caja
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.detectCollisions = false;
            // Se pone la variable en el animator para disolver la caja
            animator.SetBool("Disolver", true);
            // Se invoca la torreta correspondiente en el sitio
            GameObject aux = Instantiate(torreta.gameObject, centro.position, transform.rotation);
            if (aux.GetComponent<Torreta>())
            {
                aux.GetComponent<Torreta>().enabled = true;
            }

            Destroy(gameObject, 1);
        }
    }
}
