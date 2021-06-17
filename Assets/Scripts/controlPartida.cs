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
    public GameObject CamaraSecundaria;

    public GameObject textoEstado;
    public GameObject textoRonda;

    public GameObject mejora;

    public float tiempoEntreRondas;
    private float timer;

    private bool finDePartida;
    private bool finDeRonda;
    private bool todosSpawneados;
    private int ronda;

    ControladorEntidad[] enemigos;

    GameManager gameManager;

    private Camino camino;

    private string ultimoTexto;
    void Start()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
        bases = (Base[])GameObject.FindObjectsOfType(typeof(Base));
        spawners = (Spawner[])GameObject.FindObjectsOfType(typeof(Spawner));
        personaje = jugador.GetComponent<Personaje>();
        CamaraSecundaria.SetActive(false);
        mejora.SetActive(false);
        ronda = 1;

        gameManager = FindObjectOfType<GameManager>();
        ultimoTexto = textoEstado.GetComponent<UnityEngine.UI.Text>().text;
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
            if(s.contador == s.limitePrimerEnemigo && s.contador2 == s.limiteSegundoEnemigo && s.contador3 == s.limiteTercerEnemigo)
            {
                todosSpawneados = true;
            }
        }

        //si han aparecido todos los Enemigo y est�n todos muertos se acaba la ronda
        if (todosSpawneados && enemigos.Length == 0)
        {
            finDeRonda = true;
        }

        // Empieza la nueva ronda
        if (finDeRonda)
        {
            timer += Time.deltaTime;
            if(timer > tiempoEntreRondas)
            {
                ronda++;
                timer = 0;

                switch (ronda)
                {
                    case 2:
                        foreach (Spawner s in spawners)
                        {
                            //inicio ronda 2
                            s.contador = 0;
                            s.contador2 = 0;
                            s.contador3 = 0;
                            s.limitePrimerEnemigo += 5;
                            textoEstado.GetComponent<UnityEngine.UI.Text>().text = "Pulsa la tecla <K> para empezar la ronda 2";
                            ultimoTexto = textoEstado.GetComponent<UnityEngine.UI.Text>().text;
                            s.SetRonda(ronda);
                            textoRonda.GetComponent<UnityEngine.UI.Text>().text = "Ronda 2";
                        }
                        break;
                    case 3:
                        foreach (Spawner s in spawners)
                        {
                            //inicio ronda 3
                            s.contador = 0;
                            s.contador2 = 0;
                            s.contador3 = 0;
                            s.limitePrimerEnemigo = s.limitePrimerEnemigo * 2;
                            s.limiteSegundoEnemigo += 5;
                            textoEstado.GetComponent<UnityEngine.UI.Text>().text = "Pulsa la tecla <K> para empezar la ronda 3";
                            ultimoTexto = textoEstado.GetComponent<UnityEngine.UI.Text>().text;
                            s.SetRonda(ronda);
                            textoRonda.GetComponent<UnityEngine.UI.Text>().text = "Ronda 3";
                        }
                        break;
                    case 4:
                        foreach (Spawner s in spawners)
                        {
                            //inicio ronda 4
                            s.contador = 0;
                            s.contador2 = 0;
                            s.contador3 = 0;
                            s.limitePrimerEnemigo += 10;
                            s.limiteSegundoEnemigo = s.limiteSegundoEnemigo * 2;
                            s.limiteTercerEnemigo += 5;
                            textoEstado.GetComponent<UnityEngine.UI.Text>().text = "Pulsa la tecla <K> para empezar la ronda 3";
                            ultimoTexto = textoEstado.GetComponent<UnityEngine.UI.Text>().text;
                            s.SetRonda(ronda);
                            textoRonda.GetComponent<UnityEngine.UI.Text>().text = "Ronda 4";
                        }
                        break;
                    case 5:
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


        }

        if (finDePartida)
        {
            GameOver();
        }

        //Pulsar la <k> para empezar rondas/partida
        if (Input.GetKeyDown(KeyCode.K) && ronda == 1)
        {
            foreach(Spawner s in spawners)
            {
                s.SetRonda(ronda);
                textoEstado.GetComponent<UnityEngine.UI.Text>().text = "";
                ultimoTexto = textoEstado.GetComponent<UnityEngine.UI.Text>().text;
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
        if (!CamaraSecundaria.activeSelf)
        {
            //la camara se situa en el cielo.
            CamaraSecundaria.SetActive(!CamaraSecundaria.activeSelf);
            Cursor.lockState = CursorLockMode.None;
        }

        //borrar todas las toretas
        GameObject[] Torreta = (GameObject[])GameObject.FindGameObjectsWithTag("Torreta");
        foreach (GameObject t in Torreta)
        {
            Destroy(t);
        }
        //Texto de la ui
        textoEstado.GetComponent<UnityEngine.UI.Text>().text = "Pulsa cualquier tecla para volver al menu";
        ultimoTexto = textoEstado.GetComponent<UnityEngine.UI.Text>().text;

    }

    public void Interactuar(bool interactuar)
    {
        if(interactuar)
        {
            textoEstado.GetComponent<UnityEngine.UI.Text>().text = "Pulsa <RMB> para interactuar";
        }
        else
        {
            textoEstado.GetComponent<UnityEngine.UI.Text>().text = ultimoTexto;
        }
    }
}
