using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ---------------------------------------------------
// NAME: controlPartida.cs
// STATUS: WIP
// GAMEOBJECT: objeto
// DESCRIPTION: Controlar el inicio de partida, las rondas y la derrota
//
// AUTHOR: Adrian
// FEATURES ADDED: Se puede iniciar partidas, rondas y la partida acaba al perder las bases.
//
// AUTHOR: Jorge Grau
// FEATURES ADDED: Si T-Byte muere acaba la partida. A�adida camara secundaria.
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: Restart()
// ---------------------------------------------------

public class controlPartida : MonoBehaviour
{
    Base[] bases;
    Spawner[] spawners;
    public GameObject jugador;
    Personaje personaje;
    public Camera CamaraSecundaria;

    public GameObject textoEstado;
    public GameObject textoRonda;

    public GameObject mejora;

    private bool finDePartida;
    private bool finDeRonda;
    private bool todosSpawneados;
    private int ronda;

    ControladorEntidad[] enemigos;

    GameManager gameManager;

    private Camino camino;
    void Start()
    {
        bases = (Base[])GameObject.FindObjectsOfType(typeof(Base));
        spawners = (Spawner[])GameObject.FindObjectsOfType(typeof(Spawner));
        personaje = jugador.GetComponent<Personaje>();
        CamaraSecundaria.gameObject.SetActive(false);
        mejora.SetActive(false);
        ronda = 1;

        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        finDePartida = true;
        //si todas las bases mueren, se acaba la partida
        foreach (Base b in bases)
        {
            //comprueba si alguna base tiene vida
            if (b.salud > 0)
            {
                finDePartida = false;
            }
        }

        finDeRonda = false;
        todosSpawneados = false;
        enemigos = (ControladorEntidad[])GameObject.FindObjectsOfType(typeof(ControladorEntidad));

        //comprobar si han spawneado todos los Enemigo
        foreach (Spawner s in spawners)
        {
            if(s.conteo == s.limitePrimerEnemigo && s.conteo2 == s.limiteSegundoEnemigo)
            {
                todosSpawneados = true;
            }
        }

        //si han aparecido todos los Enemigo y est�n todos muertos se acaba la ronda
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
                        //inicio ronda 2
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
                        //inicio ronda 3
                        s.conteo = 0;
                        s.conteo2 = 0;
                        s.limitePrimerEnemigo = 15;
                        s.limiteSegundoEnemigo = 15;
                        textoEstado.GetComponent<UnityEngine.UI.Text>().text = "Pulsa la tecla <K> para empezar la ronda 3";
                        textoRonda.GetComponent<UnityEngine.UI.Text>().text = "Ronda 3";
                    }
                    break;
                case 4:
                    //ronda 4 o en este caso fin de la zona
                    foreach (Spawner s in spawners)
                    {
                        int misionesCompletadas = PlayerPrefs.GetInt("misiones_completadas");
                        PlayerPrefs.SetInt("misiones_completadas", misionesCompletadas + 1);
                        Cursor.lockState = CursorLockMode.None;
                        mejora.SetActive(true);
                        //SceneManager.LoadScene(SceneManager.GetSceneAt(1).ToString());
                    }
                    break;
                default:

                    break;
            }
        }

        if (finDePartida)
        {
            GameOver();
        }

        //Pulsar la <k> para empezar rondas/partida
        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach(Spawner s in spawners)
            {
                StartCoroutine(s.Aparicion());
                StartCoroutine(s.AparicionBombas());
                textoEstado.GetComponent<UnityEngine.UI.Text>().text = "";
            }
            
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public void Restart()
    {
        //reiniciar la partida
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        if (!CamaraSecundaria.isActiveAndEnabled)
        {
            //la camara se situa en el cielo.
            CamaraSecundaria.gameObject.SetActive(!CamaraSecundaria.gameObject.activeSelf);
            Cursor.lockState = CursorLockMode.None;
        }

        //borrar todas las toretas
        GameObject[] Torreta = (GameObject[])GameObject.FindGameObjectsWithTag("Torreta");
        foreach (GameObject t in Torreta)
        {
            Destroy(t);
        }
        //Texto de la ui
        textoEstado.GetComponent<UnityEngine.UI.Text>().text = "Has perdido, pulsa la tecla <R> para volver a empezar";
    }
}
