// ---------------------------------------------------
// NAME: SistemaMejoras.cs
// STATUS: WIP
// GAMEOBJECT: ControlPartida
// DESCRIPTION: Manipula los objetos del juego para establecer mejoras
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: script creado
// ---------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaMejoras : MonoBehaviour
{
    // Scripts accesibles
    Personaje personaje;

    // Mejoras de personaje
    public bool mejora001;
    public bool mejora002;

    private void Start()
    {
        // Busca los scripts accesibles
        personaje = FindObjectOfType<Personaje>();

        // Sube la vida de T-Byte un 35%
        if(mejora001)
        {
            // Sube la vida un 35%
            personaje.saludMaxima += (personaje.saludMaxima / 100) * 35;
            // Reasigna la vida base
            personaje.Setup();
        }

        // Pone un escudo al jugador del 50% de su vida maxima
        if(mejora002)
        {
            personaje.escudoMaximo = (personaje.Salud / 100) * 50;
            personaje.Setup();
        }
    }
}
