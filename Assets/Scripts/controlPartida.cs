using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  AUTHOR: Adrián Maldonado Llambies
 *  STATUS: WIP
 *  NAME: controlPartida.cs
 *  GAMEOBJECT: None
 *  DESCRIPTION: This script is used to control the game start, restart and end; 
 */
public class controlPartida : MonoBehaviour
{
    Base[] bases;
    Spawner[] spawners;
    public GameObject textoEstado;
    public GameObject textoRonda;

    private bool finDePartida;
    private bool finDeRonda;
    private bool todosSpawneados;
    private int ronda;
    // Start is called before the first frame update
    void Start()
    {
        bases = (Base[])GameObject.FindObjectsOfType(typeof(Base));
        spawners = (Spawner[])GameObject.FindObjectsOfType(typeof(Spawner));
        ronda = 1;
    }

    // Update is called once per frame
    void Update()
    {
        finDePartida = true;
        foreach (Base b in bases)
        {
            if (b.salud > 0)
            {
                finDePartida = false;
            }
        }

        finDeRonda = false;
        todosSpawneados = false;
        Enemigo[] enemigos = (Enemigo[])GameObject.FindObjectsOfType(typeof(Enemigo));
        foreach (Spawner s in spawners)
        {
            if(s.conteo == s.limitePrimerEnemigo && s.conteo2 == s.limiteSegundoEnemigo)
            {
                
                todosSpawneados = true;
            }
        }

        if (todosSpawneados && enemigos.Length == 0)
        {
            finDeRonda = true;
        }

        if (finDeRonda)
        {
            ronda++;
            switch (ronda)
            {
                case 2:
                    foreach (Spawner s in spawners)
                    {
                        s.conteo = 0;
                        s.conteo2 = 0;
                        s.limitePrimerEnemigo = 10;
                        s.limiteSegundoEnemigo = 10;
                        textoEstado.GetComponent<UnityEngine.UI.Text>().text = "Pulsa la tecla <K> para empezar la ronda 2";
                        textoRonda.GetComponent<UnityEngine.UI.Text>().text = "Ronda 2";
                    }
                    break;
                case 3:
                    foreach (Spawner s in spawners)
                    {
                        s.conteo = 0;
                        s.conteo2 = 0;
                        s.limitePrimerEnemigo = 15;
                        s.limiteSegundoEnemigo = 15;
                        textoEstado.GetComponent<UnityEngine.UI.Text>().text = "Pulsa la tecla <K> para empezar la ronda 3";
                        textoRonda.GetComponent<UnityEngine.UI.Text>().text = "Ronda 3";
                    }
                    break;
                case 4:
                    foreach (Spawner s in spawners)
                    {
                        textoEstado.GetComponent<UnityEngine.UI.Text>().text = "Has ganado, pulsa <R> para volver a empezar";
                        textoRonda.GetComponent<UnityEngine.UI.Text>().text = "Final";
                    }
                    break;
                default:

                    break;
            }
        }



        if (finDePartida)
        {
            GameObject[] torretas = (GameObject[])GameObject.FindGameObjectsWithTag("Torretas");
            foreach (GameObject t in torretas)
            {
                Destroy(t);
            }
            textoEstado.GetComponent<UnityEngine.UI.Text>().text = "Has perdido, pulsa la tecla <R> para volver a empezar";
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Empesar partida");
            foreach(Spawner s in spawners)
            {
                StartCoroutine(s.Aparicion());
                StartCoroutine(s.AparicionBombas());
                textoEstado.GetComponent<UnityEngine.UI.Text>().text = "";
            }
            
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Fin de partida");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            textoEstado.GetComponent<UnityEngine.UI.Text>().text = "Pulsa la tecla <K> para empezar la partida";
            foreach (Enemigo e in enemigos)
            {
                Destroy(e.gameObject);
            }
            GameObject[] torretas = (GameObject[])GameObject.FindGameObjectsWithTag("Torretas");
            foreach (GameObject t in torretas)
            {
                Destroy(t);
            }

            foreach (Spawner s in spawners)
            {
                s.conteo = 0;
                s.conteo2 = 0;
            }

            foreach (Base b in bases)
            {
                b.salud = 1f;
            }
            Personaje[] personajes = (Personaje[])GameObject.FindObjectsOfType(typeof(Personaje));
            foreach (Personaje p in personajes)
            {
                p.transform.position = Vector3.zero;
                StopAllCoroutines();
            }
            
            Debug.Log("Reiniciar");
        }

    }
}
