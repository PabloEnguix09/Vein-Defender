using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: AnimTByte.cs
// STATUS: WIP
// GAMEOBJECT: Jugador
// DESCRIPTION: Controla las animaciones del jugador
//
// AUTHOR: Juan Ferrera Sala
// FEATURES ADDED: 
// ---------------------------------------------------
public class AnimTByte : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SeleccionDeTorreta(bool estado)
    {
        animator.SetBool("SeleccionDeTorreta",estado);
    }

    public void InvocarTorreta()
    {
        animator.SetTrigger("InvocarTorreta");
    }

    public void LanzarDardo()
    {
        animator.SetTrigger("Dardo");
    }

    public void Salto()
    {
        animator.SetTrigger("Salto");
    }

    public void Muerte()
    {
        animator.SetBool("Muerte", true);
    }
}
