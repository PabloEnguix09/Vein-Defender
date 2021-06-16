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
    public enum Movimientos { Caminar = 0, Correr = 1}

    private Vector3 velocidad;
    public float maximaVelocidad = 0.1f;
    public float maximaVelocidadSalto = 40f;
    public float velocidadCaminar, velocidadCorrer;
    public float fuerzaSalto = 300f;
    public bool volando = true;
    public Vector3 Velocidad { get => velocidad; set => velocidad = value; }

    public Transform personaje;
    private Rigidbody rb;

    private Movimientos tipos;

    AudioHandler audioHandler;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        maximaVelocidad = velocidadCaminar;

        audioHandler = gameObject.GetComponent<AudioHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(velocidad.normalized.x * maximaVelocidad, rb.velocity.y, velocidad.normalized.z * maximaVelocidad);

        if (velocidad.magnitude > 0)
        {
            personaje.rotation = Quaternion.LookRotation(velocidad);
        }
        //Si la velocidad del eje Y es mas grande a maximaVelocidadSalto
        if (rb.velocity.y > maximaVelocidadSalto)
        {
            rb.AddForce(Vector3.down * fuerzaSalto);//Creamos una fuerza opueta para frenar el salto
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
        if(!volando)
        {
            rb.AddForce(Vector3.up * fuerzaSalto);
            audioHandler.Play(4);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.GetContact(collision.contactCount - 1).thisCollider.gameObject.tag == "Player" &&
            collision.GetContact(collision.contactCount - 1).otherCollider.gameObject.layer == 6)
        {
            volando = false;
            audioHandler.Play(5);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.layer == 6)
        {
            volando = true;
        }
    }
}
