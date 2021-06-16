using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: nombre
// STATUS: estado
// GAMEOBJECT: objeto
// DESCRIPTION: descripcion
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: usando axis para controles, controlador de la camara
// ---------------------------------------------------


[RequireComponent(typeof(Personaje))]
public class MecanicasPersonaje : MonoBehaviour
{
    private Personaje personaje;
    private InvocarTorreta invocar;
    CameraController cameraController;

    public string correr, menuRadial, saltar, eliminarTorreta, cambiarCamara, pausa, disparar, interactuar, cerrarInteraccion, caer;

    private void Start()
    {
        personaje = GetComponent<Personaje>();
        invocar = GetComponent<InvocarTorreta>();
        cameraController = FindObjectOfType<CameraController>();
    }
    private void Update()
    {
        // Si el jugador no esta paralizado
        if(!personaje.paralizado)
        {
            personaje.Mover(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

            if (Input.GetAxisRaw(correr) > 0)
            {
                personaje.Caminar();
            }
            else
            {
                personaje.Correr();
            }

            if (Input.GetAxisRaw(saltar) > 0)
            {
                personaje.Saltar();
            }
        }

        // No pueden usarse con la camara secundaria
        if (!personaje.camaraSecundariaActivada)
        {
            if (invocar.GetColocada())
            {
                // No puede abrirse si ya hay otro abierto
                if (Input.GetAxisRaw(menuRadial) > 0 && !invocar.menuRadial.activeSelf)
                {
                    cameraController.BloquearCamara(true);
                    invocar.AlternarMenuRadial(true);
                }
                else if (Input.GetAxisRaw(menuRadial) <= 0 && invocar.menuRadial.activeSelf)
                {
                    cameraController.BloquearCamara(false);
                    invocar.AlternarMenuRadial(false);
                }
            }

            if (Input.GetAxisRaw(eliminarTorreta) > 0)
            {
                invocar.EliminarTorreta();
            }
        }
        
        if(Input.GetButtonDown(cambiarCamara))
        {
            personaje.CambiarCamara();
        }
        if(Input.GetButtonDown(pausa))
        {
            personaje.PausarPartida();
        }
        //Disparo de dardo localizador
        if (Input.GetButtonDown(disparar) && invocar.GetColocada())
        {
            personaje.DispararDardo();
        }
        // Interactua con un objeto
        if (Input.GetButtonDown(interactuar))
        {
            personaje.Interactuar();
        }
        // Cierra el menu actual
        if (Input.GetButtonDown(cerrarInteraccion))
        {
            personaje.CerrarInteraccion();
        }
        // Cae mas rapido
        if (Input.GetAxisRaw(caer) > 0)
        {
            personaje.Caer();
        }
    }
}
