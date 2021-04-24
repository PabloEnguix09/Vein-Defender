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


[RequireComponent(typeof(Rigidbody))]
public class MovimientoPersonaje : MonoBehaviour
{
    public enum Movimientos { Caminar, Correr}

    private Vector3 velocidad;
    float maximaVelocidad = 0.1f;
    public float velocidadCaminar, velocidadCorrer;
    public float fuerzaSalto = 300f;
    private bool haSaltado = false;
    public Vector3 Velocidad { get => velocidad; set => velocidad = value; }

    public Transform personaje;
    private Rigidbody rb;

    private Movimientos tipos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(velocidad.normalized.x * maximaVelocidad, rb.velocity.y, velocidad.normalized.z * maximaVelocidad);

        if (velocidad.magnitude > 0)
        {
            personaje.rotation = Quaternion.LookRotation(velocidad);
        }
    }

    public void SetMovimientos(Movimientos movimiento)
    {
        tipos = movimiento;
        switch (movimiento)
        {
            case Movimientos.Caminar:
                {
                    maximaVelocidad = velocidadCaminar;
                    break;
                }
            case Movimientos.Correr:
                {
                    maximaVelocidad = velocidadCorrer;
                    break;
                }
        }
    }
    public Movimientos GetMovimientos()
    {
        return tipos;
    }

    internal void Saltar()
    {
        if(!haSaltado)
        {
            rb.AddForce(Vector3.up * fuerzaSalto);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.GetContact(collision.contactCount - 1).thisCollider.gameObject.tag == "Player" &&
            collision.GetContact(collision.contactCount - 1).otherCollider.gameObject.layer == 6)
        {
            haSaltado = false;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.layer == 6)
        {
            haSaltado = true;
        }
    }
}
