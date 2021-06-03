using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: EnemigoAnimacion.cs
// STATUS: WIP
// GAMEOBJECT: Enemigo
// DESCRIPTION: Controla las animaciones del enemigo actual
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: Idle, Disparo, Bloquear en el sitio
// ---------------------------------------------------

public class EnemigoAnimacion : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Bloqueado(bool estado)
    {
        animator.SetBool("Bloqueado", estado);
    }

    public void Dispara()
    {
        animator.SetTrigger("Disparo");
    }

    public void Destruido()
    {
        animator.SetBool("Destruido", true);
    }
}
