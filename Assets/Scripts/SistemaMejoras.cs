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

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SistemaMejoras : MonoBehaviour
{
    // Scripts accesibles
    Personaje personaje;

    public List<int> indice = new List<int>();

    public string[] nombres;

    [Header("Personaje")]
    // Mejoras de personaje
    public bool vidaTbyte;
    public bool escudoTbyte;
    public bool mejoraMinimapa;
    public bool mejoraMagnetismo;
    public bool mejoraImpulso;
    public bool mejoraEnergia;
    public bool mejoraMarcador;
    public bool mejoraDebilitante;
    public bool mejoraSparky;
    public bool explosionPEM;

    [Header("Desbloquear Torreta")]
    // Mejoras de torreta
    public bool hoplitaMohawk;
    public bool inmortalFantasma;
    public bool scutumBerserker;
    public bool balearTrampa;

    [Header("Torreta")]
    // Mejoras de torreta
    public bool costeEnergia;
    public bool balasReforzadas;
    public bool laseresMejorados;
    public bool cadenciaMejorada;
    public bool escanerAmenazas;
    public bool escudoEnergia;
    public bool balaExplosiva;
    public bool laserPerforante;
    public bool laserPerseguidor;
    public bool balaPEM;

    [Header("Utilidades")]
    // Mejoras de utilidades
    public bool mejoraCamara;
    public bool mejoraCaminos;
    public bool escudoDefensor;

    public TorretasDisponibles torretasDisponibles;
    public Camino[] camino;
    public DragDrop[] iconos;

    public GameObject hud;

    private void Start()
    {
        GameObject objetos = SceneManager.GetSceneByName("Nave").GetRootGameObjects()[1].GetComponentInChildren<Interaccion>().GetComponentInChildren<CinemachineVirtualCamera>(true).GetComponentInChildren<HUD>(true).gameObject;
        iconos = objetos.GetComponentsInChildren<DragDrop>();
    }
    public void MejorasPersonaje(Personaje personaje)
    {
        // Sube la vida de T-Byte un 35%
        if(vidaTbyte)
        {
            // Sube la vida un 35%
            personaje.saludMaxima += (personaje.saludMaxima / 100) * 35;
            // Reasigna la vida base
        }
        // Pone un escudo al jugador del 50% de su vida maxima
        if(escudoTbyte)
        {
            personaje.escudoMaximo = (personaje.Salud / 100) * 50;
        }
        // Pone un minimapa
        if(mejoraMinimapa)
        {
            personaje.minimapa.SetActive(true);
            GameObject[] minimapa = GameObject.FindGameObjectsWithTag("Minimapa");
            foreach(GameObject mapa in minimapa)
            {
                mapa.SetActive(true);
            }
        }
        if(mejoraMagnetismo)
        {
            personaje.movimientoPersonaje.maximaVelocidad += personaje.movimientoPersonaje.maximaVelocidad * 0.3f;
        }
        if(mejoraImpulso)
        {
            personaje.movimientoPersonaje.fuerzaSalto += personaje.movimientoPersonaje.fuerzaSalto * 0.3f;
        }
        if(mejoraEnergia)
        {
            personaje.energiaMaxima += 5;
        }

        personaje.Setup();
    }
    
    public void MejorasTorreta(Torreta torreta)
    {
        /*
        if (mejoraMarcador)
        {
            torreta.disparoMarcado = true;
        }
        // Reduce el coste de todas las Torreta -1 si son mayor de 1
        if (costeEnergia)
        {
            if (torreta.energia > 1)
            {
                torreta.energia -= 1;
            }
        }
        if(balasReforzadas)
        {
            if(torreta.balaObjeto.name == "Bala")
            {
                torreta.ataque += torreta.ataque * 0.2f;
            }
        }
        if(laseresMejorados)
        {
            if (torreta.balaObjeto.name == "BalaLaser")
            {
                torreta.ataque += torreta.ataque * 0.2f;
            }
        }
        if(cadenciaMejorada)
        {
            if(torreta.cadenciaDisparo > 1)
            {
                torreta.cadenciaDisparo -= 1;
            }
        }
        if (escanerAmenazas)
        {
            torreta.distanciaDisparo += 5;  
        }
        if(escudoEnergia)
        {
            if(!torreta.escudo)
            {
                torreta.escudo = true;
                torreta.escudoMaximo = 10;
                torreta.escudoRegen = 0.5f;
            }
            else
            {
                torreta.escudoMaximo += torreta.escudoMaximo * 0.5f;
            }
        }
        if(balaExplosiva)
        {
            if(torreta.balaObjeto.name == "Bala")
            {
                if (torreta.danyoExplosion == 0)
                {
                    torreta.danyoExplosion = torreta.ataque * 0.5f;
                    torreta.radioExplosion = 1;
                }
                else
                {
                    torreta.radioExplosion += 1;
                }
            }
        }
        if(laserPerforante)
        {
            if(torreta.balaObjeto.name == "BalaLaser")
            {
                torreta.perforante = true;
            }
        }
        if(laserPerseguidor)
        {
            if (torreta.balaObjeto.name == "BalaLaser")
            {
                torreta.perseguidor = true;
            }
        }
        if(balaPEM)
        {
            if(torreta.balaObjeto.name == "Bala")
            {
                torreta.disparoPEM = true;
            }
        }
        */
    }

    public void DesbloquearTorreta()
    {
        /*
        // Añade al indice la id de la torreta holpita y mohawk
        if (hoplitaMohawk)
        {
            iconos[2].gameObject.SetActive(true);
            indice.Add(2);
            iconos[3].gameObject.SetActive(true);
            indice.Add(3);
        }
        // Añade al indice la id de la torreta inmortal y la fantasma
        if(inmortalFantasma)
        {
            iconos[4].gameObject.SetActive(true);
            indice.Add(4);
            iconos[5].gameObject.SetActive(true);
            indice.Add(5);
        }
        if (scutumBerserker)
        {
            iconos[6].gameObject.SetActive(true);
            indice.Add(6);
            iconos[7].gameObject.SetActive(true);
            indice.Add(7);
        }
        if (balearTrampa)
        {
            iconos[8].gameObject.SetActive(true);
            indice.Add(8);
            iconos[9].gameObject.SetActive(true);
            indice.Add(9);
        }
        */
    }

    public void MejorasUtilidades()
    {
        /*
        if (mejoraCamara)
        {
            iconos[1].gameObject.SetActive(true);
            indice.Add(1);
        }
        */
        if (!mejoraCaminos)
        {
            foreach (Camino ruta in camino)
            {
                Destroy(ruta.gameObject);
            }
        }
        /*
        if (escudoDefensor)
        {
            iconos[10].gameObject.SetActive(true);
            indice.Add(10);
        }
        */
    }

    public void lladamaProvisonalTorretas()
    {
        torretasDisponibles.asignarTorretasDisponibles(indice);   
    }
}
