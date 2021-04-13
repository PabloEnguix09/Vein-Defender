using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: Personaje.cs
// STATUS: WIP
// GAMEOBJECT: Jugador
// DESCRIPTION: Este script contiene todo lo relacionado con el personaje, movimiento, salto, vida...
//
// AUTHOR: Pablo Enguix Llopis
// FEATURES ADDED: cosas hechas
//
// AUTHOR: Jorge Grau
// FEATURES ADDED: Salud y energia añadidos
// ---------------------------------------------------

public class Personaje : MonoBehaviour
{
    private float adelante;
    private float derecha;
    private Vector3 velocidad;

    public ControlVida barraVida;
    public ControlEnergia barraEnergia;

    [Range(0, 1)]
    [SerializeField]
    public float saludMaxima = 1;

    public float salud = 1;

    public float Salud
    {
        get { return salud; }

        set
        {
            
            value = Mathf.Clamp01(value);
            salud = value;
            //Debug.Log(salud);
            barraVida.controlVida(salud);
            if (salud <= 0)
            {
                //Debug.Log("Destruido");
                Destroy(gameObject);
            }
        }
    }

    public float energia = 10;

    public float energiaMaxima = 10;

    public float Energia
    {
        get { return energia; }

        set 
        { 
            energia = value;
        }
    }

    public float alcance = 12.5f;

    public CameraController camara;
    public MovimientoPersonaje personaje;

    private void Start()
    {
        salud = saludMaxima;
        energia = energiaMaxima;

        barraVida.maximaVida(salud);

        barraEnergia.maximaEnergia(energia);
        
    }
    public void Mover(float adelante, float derecha)
    {
        this.adelante = adelante;
        this.derecha = derecha;

        Vector3 objetivo = adelante * camara.transform.forward;
        objetivo += derecha * camara.transform.right;
        objetivo.y = 0;

        if (objetivo.magnitude > 0)
        {
            velocidad = objetivo;
        }
        else
        {
            velocidad = Vector3.zero;
        }
        personaje.Velocidad = objetivo;
    }

    public void Correr()
    {
        if(personaje.GetMovimientos() != MovimientoPersonaje.Movimientos.Correr)
        {
            personaje.SetMovimientos(MovimientoPersonaje.Movimientos.Correr);
        }
        else
        {
            personaje.SetMovimientos(MovimientoPersonaje.Movimientos.Caminar);
        }
    }

    public void Saltar()
    {
        personaje.Saltar();
    }

    // SOLO PARA LAS ESCENAS: Muestra el rayo de apuntado
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * alcance);
    }
}
