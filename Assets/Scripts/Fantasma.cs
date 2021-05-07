using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fantasma : MonoBehaviour
{
    private TorretaBasica torreta;
    private Personaje personaje;
    private float energia;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        personaje = player[0].GetComponent<Personaje>();
    }

    private void activarInvisibilidad()
    {
        energia = torreta.energiaAlt - torreta.energia;
        if (torreta.invisibilidad)
        {
            // Aumenta la energia del jugador y desactiva la invisibilidad
            torreta.invisibilidad = false;
            personaje.Energia += energia;
        }
        else
        {
            // Reduce la energia del jugadory y activa la invisibilidad
            if (!(personaje.Energia - energia < 0))
            {
                torreta.invisibilidad = true;
                personaje.Energia -= energia;
            }
        }
    }
}
