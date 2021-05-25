using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: Fantasma.cs
// STATUS: WIP
// GAMEOBJECT: Enemigo
// DESCRIPTION: Este script activa la invisibilidad de la torreta fantasma.
//
// AUTHOR: Jorge Grau
// FEATURES ADDED: Activación de la invisibilidad y comprobación de la energia.
// ---------------------------------------------------

public class Fantasma : MonoBehaviour
{
    private Personaje personaje;
    private float energia;
    AudioHandler audioHandler;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        personaje = player[0].GetComponent<Personaje>();

        audioHandler = gameObject.GetComponent<AudioHandler>();
    }

    public void activarInvisibilidad(Torreta fantasma)
    {
        // Sonido invisibildad
        audioHandler.PlaySound(1, false);

        energia = fantasma.energiaAlt - fantasma.energia;
        if (fantasma.invisibilidad)
        {
            // Aumenta la energia del jugador y desactiva la invisibilidad
            gameObject.gameObject.GetComponent<Animator>().SetBool("Invisible", false);
            fantasma.invisibilidad = false;
            fantasma.energiaEnUso = fantasma.energia;
            personaje.Energia += energia;

        }
        else
        {
            // Reduce la energia del jugadory y activa la invisibilidad
            if (!(personaje.Energia - energia < 0))
            {
                gameObject.gameObject.GetComponent<Animator>().SetBool("Invisible", true);
                fantasma.invisibilidad = true;
                fantasma.energiaEnUso = fantasma.energiaAlt;
                personaje.Energia -= energia;
            }
        }
    }
}
