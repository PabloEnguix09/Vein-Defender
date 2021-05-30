// ---------------------------------------------------
// NAME: Interaccion.cs
// STATUS: WIP
// GAMEOBJECT: objetos interactuables en el Layer = 
// DESCRIPTION: Este escript se implementa la posibilidad de interactuar con diversos elementos
//
// AUTHOR: Juan Ferrera Sala
// FEATURES ADDED: Activar el HUD y layer interactuables.
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: Interaccion con el HUD
// ---------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaccion : MonoBehaviour
{

    public GameObject hud;

    public TipoItem tipoItem;
    //esta variable guardará el animator
    Animator animator;
    public GameObject nave;
    CameraController cameraController;
    public enum TipoItem
    {
        pcTorretas = 1
    }

    // Start is called before the first frame update
    void Start()
    {
        // Busca el camera controller
        cameraController = FindObjectOfType<CameraController>();
        // Lo desactiva al principio por si acaso
        hud.SetActive(false);
        animator = nave.GetComponent<Animator>(); 

    }

    public Interaccion Interactuar()
    {
        // Si es un PC de torretas interactua con el HUD de las torretas
        if(tipoItem == TipoItem.pcTorretas)
        {
            // Abre el menu
            hud.SetActive(true);
            // Cuando el HUD esta activado queremos tener la camara bloqueada
            cameraController.BloquearCamara(true);
            
            return this;
        }
        
        return null;
    }

    public void Cerrar()
    {
        if (tipoItem == TipoItem.pcTorretas)
        {
            // Actualiza las torretas en uso
            hud.GetComponent<HUD>().actualizarTorretasUso();
            // Cierra el menu
            hud.SetActive(false);
            // Cuando el HUD esta desactivado queremos tener la camara liberada
            cameraController.BloquearCamara(false);
            abrirPuerta();
        }
    }

    public void abrirPuerta()
    {
        animator.SetBool("tieneTorretas", true);
    }
}
