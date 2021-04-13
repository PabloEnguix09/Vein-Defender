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


[RequireComponent(typeof(Personaje))]
public class MecanicasPersonaje : MonoBehaviour
{
    private Personaje personaje;
    private InvocarTorreta invocar;

    private void Start()
    {
        personaje = GetComponent<Personaje>();
        invocar = GetComponent<InvocarTorreta>();
    }
    private void Update()
    {
        personaje.Mover(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            personaje.Correr();
        }

        if (invocar.GetColocada())
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // En desuso, ahora se activa desde InvocarTorreta.cs Update()
                //invocar.SetColocada(false);
                //invocar.PreviewTorreta("torretaBasica");
                invocar.AlternarMenuRadial();
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            personaje.Saltar();
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            invocar.EliminarTorreta();
        }
    }
}
