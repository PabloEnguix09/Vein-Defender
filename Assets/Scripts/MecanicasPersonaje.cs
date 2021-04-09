using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                invocar.SetColocada(false);
                invocar.PreviewTorreta("Basica");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                invocar.SetColocada(false);
                invocar.PreviewTorreta("Pesada");
            }
        }
    }
}
