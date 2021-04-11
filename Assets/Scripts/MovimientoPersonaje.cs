using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  AUTHOR: Pablo Enguix Llopis
 *  STATUS: WIP
 *  NAME: MovimientoPersonaje.cs
 *  GAMEOBJECT: Jugador
 *  DESCRIPTION: This script is used to make the different moves of the character
 */

[RequireComponent(typeof(Rigidbody))]
public class MovimientoPersonaje : MonoBehaviour
{
    public enum Movimientos { Caminar, Correr}

    private Vector3 velocidad;
    public float maximaVelocidad = 0.1f;
    public float fuerzaSalto = 300f;
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
                    maximaVelocidad = 5f;
                    break;
                }
            case Movimientos.Correr:
                {
                    maximaVelocidad = 20f;
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
        if(personaje.transform.position.y == 0)
        {
            rb.AddForce(Vector3.up * fuerzaSalto);
        }
    }
}
