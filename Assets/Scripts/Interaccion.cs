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
    }

    public void Interactuar()
    {
        // Si es un PC de torretas interactua con el HUD de las torretas
        if(tipoItem == TipoItem.pcTorretas)
        {
            if (hud.activeSelf)
            {
                // Actualiza las torretas en uso
                hud.GetComponent<HUD>().actualizarTorretasUso();
            }
            // Abre o cierra el menu
            hud.SetActive(!hud.activeSelf);
            // Cuando el HUD esta activado queremos tener la camara bloqueada
            cameraController.BloquearCamara(hud.activeSelf);
        }
    }
}
