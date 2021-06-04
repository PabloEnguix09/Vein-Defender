using UnityEngine;

// ---------------------------------------------------
// NAME: EnemigoVida.cs
// STATUS: DONE
// GAMEOBJECT: Enemigo
// DESCRIPTION: Guarda y modifica la vida de un enemigo
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: Optimizado en el nuevo Script
// ---------------------------------------------------

public class EnemigoVida : MonoBehaviour
{
    #region Variables
    EnemigoControlador controlador;

    float vida;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        controlador = GetComponent<EnemigoControlador>();
        vida = controlador.stats.vidaMaxima;
    }

    public void RecibirAtaque(Ataque ataque)
    {
        vida -= ataque.fuerza;
    }
}
