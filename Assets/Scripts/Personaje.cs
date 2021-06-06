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
// FEATURES ADDED: Salud y energia. Funcionalidad a la funcion Interactuar
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: Correcciones a energia y vida, control de partida, recibirAtaque, mejoras de personaje
//
//
// AUTHOR: Juan Ferrera Sala
// FEATURES ADDED: Creacion del dardo localizador, esqueleto de la función Interactuar
// ---------------------------------------------------

public class Personaje : MonoBehaviour
{
    private float adelante;
    private float derecha;
    private Vector3 velocidad;

    public ControlVida barraVida;
    public ControlEnergia barraEnergia;
    controlPartida controlPartida;
    // Se usa en mecanicasPersonaje.cs para bloquear el movimiento
    public bool paralizado;

    public GameObject camaraMejora;

    public float saludMaxima = 10;
    public bool camaraSecundariaActivada = false;
    [SerializeField]
    float salud = 10;

    public GameObject dardoLocalizador;
    public Camera camaraJugador;

    private CameraController controladorCamara;

    public GameObject minimapa;

    Interaccion interaccionActual;

    public float alcance;

    AnimTByte animTByte;

    public float Salud
    {
        get { return salud; }

        set
        {
            // comprueba que el valor est� dentro de los posibles
            value = Mathf.Clamp(value, 0, saludMaxima);
            salud = value;
            // establece el maximo de vida en la barra
            barraVida.maximaVida(saludMaxima);

            if (salud <= 0)
            {
                // Vuelve la camara al jugador si esta la secundaria activada
                if (camaraMejora != null)
                {
                    if (camaraMejora.GetComponentInChildren<Camera>().enabled)
                    {
                        camaraMejora.GetComponentInChildren<Camera>().enabled = false;
                        camara.camara.GetComponent<Camera>().enabled = true;
                        camaraSecundariaActivada = false;
                    }
                }
                animTByte.Muerte();
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

    public GameObject campo;

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
        // Busca componentes internos
        controlPartida = FindObjectOfType<controlPartida>();
        sistemaMejoras = FindObjectOfType<SistemaMejoras>();
        movimientoPersonaje = gameObject.GetComponent<MovimientoPersonaje>();
        controladorCamara = transform.GetComponentInChildren<CameraController>();
        // Script de control de las animaciones
        animTByte = GetComponent<AnimTByte>();
        // Activa el sistema de mejoras y las aplica al personaje
        sistemaMejoras.MejorasPersonaje(this);
        sistemaMejoras.DesbloquearTorreta();
        sistemaMejoras.MejorasUtilidades();
        sistemaMejoras.lladamaProvisonalTorretas();

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
        if (Escudo < escudoMaximo)
        {            Escudo += escudoPorSegundo * Time.deltaTime;
        }
        // Si en algun momento la camara es destruida, vuelve la vista al jugador
        if(camaraMejora == null && camaraSecundariaActivada)
        {
            camara.camara.GetComponent<Camera>().enabled = true;
            camaraSecundariaActivada = false;
        }

        //Disparo de dardo localizador
        if (Input.GetButtonDown("Fire1"))
        {
            DispararDardo();
            animTByte.LanzarDardo();
        }
        // Interactua con un objeto
        if (Input.GetButtonDown("Fire2"))
        {
            Interactuar();
        }
        // Cierra el menu actual
        if(Input.GetButtonDown("Cancelar"))
        {
            CerrarInteraccion();
        }
    }

    // Marca enemigos con raycast directamente
    public void DispararDardo()
    {
        /*GameObject dardoLocalizadorObjeto = Instantiate(dardoLocalizador);
        dardoLocalizadorObjeto.transform.position = camaraJugador.transform.position;
        dardoLocalizadorObjeto.transform.forward = camaraJugador.transform.forward;
        */

        RaycastHit punto;
        // Comprueba que este apuntando a un item en el Layer Enemigo
        if (Physics.Raycast(camaraJugador.transform.position, camaraJugador.transform.forward, out punto, alcance, LayerMask.GetMask("Enemigo")))
        {
            Debug.Log(punto.transform.gameObject.name);
            // Comprueba que sea un enemigo y recoge su script Enemigo
            if (punto.transform.gameObject.TryGetComponent<Enemigo>(out Enemigo enemigo))
            {
                if(sistemaMejoras.mejoraDebilitante)
                {
                    enemigo.enemigo.ataque -= enemigo.enemigo.ataque * 0.5f;
                }
                enemigo.Marcar();
            }
        }

        if (Physics.Raycast(camaraJugador.transform.position, camaraJugador.transform.forward, out punto, alcance, LayerMask.GetMask("Torreta")))
        {
            Debug.Log(punto.transform.gameObject.name);
            // Comprueba que sea un enemigo y recoge su script Enemigo
            if (punto.transform.gameObject.TryGetComponent<Torreta>(out Torreta torreta))
            {
                if(torreta.gameObject.name == "torretaBasica(Clone)")
                {
                    if (sistemaMejoras.mejoraSparky)
                    {
                        torreta.ataque += torreta.ataque * 0.5f;
                        torreta.radioExplosion +=  torreta.radioExplosion * 0.5f;
                        torreta.vidaMaxima += torreta.vidaMaxima * 0.5f;
                        torreta.cadenciaDisparo += torreta.cadenciaDisparo * 0.5f;
                    }
                }
            }
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
        animTByte.Salto();

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
            } 
            else
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

        // No puedes interactuar con cosas cuando estas usando la camara secundaria
        if(!camaraSecundariaActivada)
        {
            // Comprueba que este apuntando a un item en el Layer Torreta
            if (Physics.Raycast(camaraJugador.transform.position, camaraJugador.transform.forward, out punto, alcance, LayerMask.GetMask("Interactuable")))
            {
                // Toma la item del RaycastHit
                GameObject itemMarcado = punto.transform.gameObject;
                if (itemMarcado.TryGetComponent(out Fantasma torreta))
                {
                    torreta.activarInvisibilidad(itemMarcado.GetComponent<Torreta>());
                }
                if (itemMarcado.TryGetComponent(out Interaccion interaccion))
                {
                    // Abrimos menu
                    interaccionActual = interaccion.Interactuar();
                    paralizado = true;
                }
            }
        }
    }

    public void CerrarInteraccion()
    {
        if(interaccionActual != null)
        {
            interaccionActual.Cerrar();
        }
        interaccionActual = null;
        paralizado = false;
    }

    // SOLO PARA LAS ESCENAS: Muestra el rayo de apuntado
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(camaraJugador.transform.position, camaraJugador.transform.forward * alcance);
    }
}
