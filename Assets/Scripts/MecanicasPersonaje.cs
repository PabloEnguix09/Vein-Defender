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
// FEATURES ADDED: usando axis para controles
// ---------------------------------------------------


[RequireComponent(typeof(Personaje))]
public class MecanicasPersonaje : MonoBehaviour
{
    private Personaje personaje;
    private InvocarTorreta invocar;

    public string correr, menuRadial, saltar, eliminarTorreta;

    private void Start()
    {
        personaje = GetComponent<Personaje>();
        invocar = GetComponent<InvocarTorreta>();
    }
    private void Update()
    {
        personaje.Mover(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

        if(Input.GetAxisRaw(correr) > 0)
        {
            personaje.Correr();
        } else
        {
            personaje.Caminar();
        }

        if (invocar.GetColocada())
        {
            if (Input.GetAxisRaw(menuRadial) > 0 && !invocar.menuRadial.activeSelf)
            {
                // En desuso, ahora se activa desde InvocarTorreta.cs Update()
                //invocar.SetColocada(false);
                //invocar.PreviewTorreta("torretaBasica");
                invocar.AlternarMenuRadial(true);
            } else if (Input.GetAxisRaw(menuRadial) <= 0 && invocar.menuRadial.activeSelf)
            {
                invocar.AlternarMenuRadial(false);
            }
        }

        if(Input.GetAxisRaw(saltar) > 0)
        {
            personaje.Saltar();
        }

        if(Input.GetAxisRaw(eliminarTorreta) > 0)
        {
            invocar.EliminarTorreta();
        }
    }
}
