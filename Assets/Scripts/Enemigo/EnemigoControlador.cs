using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: EnemigoControlador.cs
// STATUS: WIP
// GAMEOBJECT: Enemigo
// DESCRIPTION: centro de control de todos los comandos del enemigo
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: centro de control de todos los comandos del enemigo
// ---------------------------------------------------

public class EnemigoControlador : MonoBehaviour
{
    #region Variables
    // Objeto enemigo
    public EnemigoBasico stats;

    // Componentes del enemigo
    EnemigoAnimacion animacion;
    EnemigoAtaque ataque;
    EnemigoVida vida;
    EnemigoMovimiento movimiento;

    bool pausa;
    #endregion

    private void Awake()
    {
        animacion = GetComponent<EnemigoAnimacion>();
        ataque = GetComponent<EnemigoAtaque>();
        vida = GetComponent<EnemigoVida>();
        movimiento = GetComponent<EnemigoMovimiento>();
    }

    public void TorretaEnRango()
    {

    }

    public void RecibeAtaque(Ataque ataque)
    {
        vida.RecibirAtaque(ataque);
    }

    public void Caminar()
    {

    }

    public void Muerte()
    {
        Destroy(gameObject);
    }

    public void Disparar()
    {

    }

    public void Potenciado(bool estado, GameObject potenciador)
    {
        if(stats.tipoAtaque != EnemigoBasico.Tipo.potenciador)
        {
            ataque.Potenciado(estado, potenciador);
        }
    }

    public void Pausa()
    {
        pausa = true;
    }

    public void Reanudar()
    {
        pausa = false;
    }

    public void AsignarBases(Base b1, Base b2, Base b3)
    {
        movimiento.AsignarBases(b1, b2, b3);
    }

}
