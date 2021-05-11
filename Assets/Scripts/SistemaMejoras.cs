// ---------------------------------------------------
// NAME: SistemaMejoras.cs
// STATUS: WIP
// GAMEOBJECT: GameManager
// DESCRIPTION: Manipula los objetos del juego para establecer mejoras
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: script creado
//
// AUTHOR: Juan Ferrera Sala
// FEATURES ADDED: Se han introducido las mejoras de Camara,inmortal,fantasma,mejoraMinimapa
// ---------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaMejoras : MonoBehaviour
{
    // Scripts accesibles
    Personaje personaje;

    public List<int> indice = new List<int>();

    [Header("Personaje")]
    // Mejoras de personaje
    public bool mejorap01;
    public bool mejorap02;
    public bool mejoraMinimapa;

    [Header("Desbloquear Torreta")]
    // Mejoras de torreta
    public bool inmortal;
    public bool fantasma;

    [Header("Torreta")]
    // Mejoras de torreta
    public bool mejorat01;

    [Header("Utilidades")]
    // Mejoras de utilidades
    public bool mejoraCamara;

    public TorretasDisponibles torretasDisponibles;

    public void MejorasPersonaje(Personaje personaje)
    {

        // Sube la vida de T-Byte un 35%
        if(mejorap01)
        {
            // Sube la vida un 35%
            personaje.saludMaxima += (personaje.saludMaxima / 100) * 35;
            // Reasigna la vida base
        }

        // Pone un escudo al jugador del 50% de su vida maxima
        if(mejorap02)
        {
            personaje.escudoMaximo = (personaje.Salud / 100) * 50;
        }
        // Pone un minimapa
        if(mejoraMinimapa)
        {
            personaje.minimapa.SetActive(true);
        }

        personaje.Setup();
    }

    public void MejorasTorreta(Torreta torreta)
    {
        // Reduce el coste de todas las Torreta -1 si son mayor de 1
        if (mejorat01)
        {
            if (torreta.energia > 1)
            {
                torreta.energia -= 1;
            }
        }
    }

    public void DesbloquearTorreta()
    {
        // Añade al indice la id de la torreta inmortal
        if(inmortal)
        {
            indice.Add(2);
        }
        if (fantasma)
        {
            indice.Add(3);
        }
    }

    public void MejorasUtilidades()
    {
        if (mejoraCamara)
        {
            indice.Add(1);
        }
    }

    public void lladamaProvisonalTorretas()
    {
        torretasDisponibles.torretasElegidas(indice);   
    }
}
