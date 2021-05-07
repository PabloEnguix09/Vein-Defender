// ---------------------------------------------------
// NAME: Interaccion.cs
// STATUS: WIP
// GAMEOBJECT: 
// DESCRIPTION: Este escript se implementa la posibilidad de interactuar con diversos elementos
//
// AUTHOR: Juan Ferrera Sala
// FEATURES ADDED: Activar el HUD y layer interactuables.
// ---------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaccion : MonoBehaviour
{

    public GameObject hud;

    public TipoItem tipoItem;
    public enum TipoItem
    {
        pcTorretas = 1
    }

    // Start is called before the first frame update
    void Start()
    {

        hud.SetActive(false);

    }

    public void Interactuar()
    {
        tipoItem = TipoItem.pcTorretas;
        hud.SetActive(!hud.activeSelf);
    }
}
