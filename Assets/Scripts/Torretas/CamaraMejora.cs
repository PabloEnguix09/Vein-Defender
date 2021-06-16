using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ---------------------------------------------------
// NAME: CamaraMejora.cs
// STATUS: WIP
// GAMEOBJECT: CamaraMejora
// DESCRIPTION: Controla que solo haya una camara de mejora en la escena
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: Funcionamiento básico
// ---------------------------------------------------

public class CamaraMejora : MonoBehaviour
{
    public GameObject camara;
    // Start is called before the first frame update
    void Start()
    {
        Personaje personaje = FindObjectOfType<Personaje>();

        if(personaje.camaraMejora != null)
        {
            personaje.camaraMejora.GetComponent<Torreta>().DestruirTorreta();
        }
        // Asigna esta camara al Jugador como secundaria
        personaje.camaraMejora = gameObject;
    }
}
