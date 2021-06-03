using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoVida : MonoBehaviour
{
    #region Variables
    EnemigoControlador controlador;

    float vida;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        vida = controlador.stats.vidaMaxima;
    }

    public void RecibirAtaque(Ataque ataque)
    {
        vida -= ataque.fuerza;
    }
}
