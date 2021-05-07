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
// FEATURES ADDED: Salud y energia a�adidos
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: Correcciones a energia y vida, control de partida, recibirAtaque, mejoras de personaje
//
//
// AUTHOR: Juan Ferrera Sala
// FEATURES ADDED: Creacion del dardo localizador
// ---------------------------------------------------

public class Personaje : MonoBehaviour
{
    private float adelante;
    private float derecha;
    private Vector3 velocidad;

    public ControlVida barraVida;
    public ControlEnergia barraEnergia;
    controlPartida controlPartida;
    public GameObject camaraMejora;

    public float saludMaxima = 10;

    public bool camaraSecundariaActivada = false;

    [SerializeField]
    float salud = 10;

    public GameObject dardoLocalizador;
    public Camera camaraJugador;

    public GameObject minimapa;

    public float alcance;

    public float Salud
    {
        get { return salud; }

        set
        {
            // comprueba que el valor est� dentro de los posibles
            value = Mathf.Clamp(value, 0, saludMaxima);
            salud = value;
            
            barraVida.controlVida(salud);
            // establece el maximo de vida en la barra
            barraVida.maximaVida(saludMaxima);
            if (salud <= 0)
            {
                controlPartida.GameOver();
            }
        }
    }

    [SerializeField]
    float energia = 10;

    public float energiaMaxima = 10;

    public float Energia
    {
        get { return energia; }

        set 
        {
            value = Mathf.Clamp(value, 0, energiaMaxima);
            // establece el maximo de energia en la barra
            barraEnergia.maximaEnergia(energiaMaxima);
            energia = value;
        }
    }

    [SerializeField]
    float escudo = 0;

    public float escudoMaximo = 0;

    public float escudoPorSegundo = 0.01f;

    public float Escudo
    {
        get { return escudo; }

        set
        {
            value = Mathf.Clamp(value, 0, escudoMaximo);
            escudo = value;
        }
    }

    public CameraController camara;
    public MovimientoPersonaje movimientoPersonaje;
    SistemaMejoras sistemaMejoras;

    private void Start()
    {
        // Busca el controlador de partida
        controlPartida = FindObjectOfType<controlPartida>();
        sistemaMejoras = FindObjectOfType<SistemaMejoras>();
        movimientoPersonaje = gameObject.GetComponent<MovimientoPersonaje>();

        // Activa el sistema de mejoras y las aplica al personaje
        sistemaMejoras.MejorasPersonaje(this);
    }

    // Reasigna los valores del personaje
    public void Setup()
    {
        // reinicia la energia y la vida actuales
        Salud = saludMaxima;
        Energia = energiaMaxima;
        Escudo = escudoMaximo;
    }

    private void Update()
    {
        // Regenerar el escudo
        if(Escudo < escudoMaximo)
        {
            Escudo = Escudo + escudoPorSegundo * Time.deltaTime;
        }

        //Disparo de dardo localizador
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject dardoLocalizadorObjeto = Instantiate(dardoLocalizador);

            dardoLocalizadorObjeto.transform.position = camaraJugador.transform.position + camaraJugador.transform.forward;

            dardoLocalizadorObjeto.transform.forward = camaraJugador.transform.forward;

        }

        if (Input.GetButtonDown("Fire2")){

            Interactuar();

        }

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
        movimientoPersonaje.Velocidad = objetivo;
    }

    public void Correr()
    {
        movimientoPersonaje.SetMovimientos(MovimientoPersonaje.Movimientos.Correr);
    }

    public void Caminar()
    {
        movimientoPersonaje.SetMovimientos(MovimientoPersonaje.Movimientos.Caminar);
    }

    public void Saltar()
    {
        movimientoPersonaje.Saltar();
    }

    public void RecibirAtaque(Ataque ataque)
    {
        if (Escudo > 0)
        {
            // Restamos la fuerza al escudo y el escudo a la fuerza
            float auxFuerza = ataque.fuerza;
            ataque.fuerza -= Escudo;
            Escudo -= auxFuerza;
        }
        // Despues restamos la fuerza que quede a la salud
        if (ataque.fuerza > 0)
        {
            Salud -= ataque.fuerza;
        }
    }

    // Cambia a la camara secundaria o la primaria
    public void CambiarCamara()
    {
        if(camaraMejora != null)
        {
            if(camaraMejora.GetComponentInChildren<Camera>().enabled)
            {
                camaraMejora.GetComponentInChildren<Camera>().enabled = false;
                camara.camara.GetComponent<Camera>().enabled = true;
                camaraSecundariaActivada = false;
            } else
            {
                camara.camara.GetComponent<Camera>().enabled = false;
                camaraMejora.GetComponentInChildren<Camera>().enabled = true;
                camaraSecundariaActivada = true;
            }
        }
    }

    public void Interactuar()
    {
        RaycastHit punto;
        // Comprueba que este apuntando a una torreta en el Layer Torreta
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out punto, alcance, LayerMask.GetMask("Interactuable")))
        {
            // Toma la torreta del RaycastHit
            GameObject torretaMarcada = punto.transform.gameObject;
            // Abrimos canvas
            torretaMarcada.GetComponent<Interaccion>().Interactuar();
        }
    }

    // SOLO PARA LAS ESCENAS: Muestra el rayo de apuntado
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 13f);
    }
}
