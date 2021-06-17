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
    public GameObject[] rayos;
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

    public bool finMision;
    public float timerFinMision;

    ControladorEntidad[] enemigos;

    GameManager gameManager;

    private Camino camino;

    private string ultimoTexto;

    public SistemaMejoras mejoras;

    void Start()
    {
        finMision = false;
        timerFinMision = 0;
        bases = (Base[])GameObject.FindObjectsOfType(typeof(Base));
        spawners = (Spawner[])GameObject.FindObjectsOfType(typeof(Spawner));
        personaje = jugador.GetComponent<Personaje>();
        CamaraSecundaria.SetActive(false);
        mejora.SetActive(false);
        ronda = 1;
        gameManager = FindObjectOfType<GameManager>();
        ultimoTexto = textoEstado.GetComponent<UnityEngine.UI.Text>().text;

        mejoras.activarMejoras();
    }

    void Update()
    {
        if (finMision)
        {
            timerFinMision += Time.deltaTime;
            if(timerFinMision > 2)
            {
                SceneManager.LoadScene(2);
                SceneManager.LoadScene(3, LoadSceneMode.Additive);
                SceneManager.LoadScene(4, LoadSceneMode.Additive);
            }
        }

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

        if(personaje.Salud <= 0)
        {
            finDePartida = true;
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
                            //s.limitePrimerEnemigo += 5;
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
                            //s.limitePrimerEnemigo = s.limitePrimerEnemigo * 2;
                            //s.limiteSegundoEnemigo += 5;
                            s.limiteSegundoEnemigo = 0;
                            s.limiteTercerEnemigo = 0;
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
                            //s.limitePrimerEnemigo += 10;
                            //s.limiteSegundoEnemigo = s.limiteSegundoEnemigo * 2;
                            //s.limiteTercerEnemigo += 5;
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

            activarExcavadoras();
            foreach(Spawner s in spawners)
            {
                s.SetRonda(ronda);
                textoEstado.GetComponent<UnityEngine.UI.Text>().text = "";
                ultimoTexto = textoEstado.GetComponent<UnityEngine.UI.Text>().text;
            }
            
        }

        //Salir al menu
        if (finDePartida && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }

        // Reinicio rapido
        if ( finDePartida && Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public void Restart()
    {
        limpiarPrefs();
        //al pulsar cualquier tecla carga la siguiente escena
        SceneManager.LoadScene(2);
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
        SceneManager.LoadScene(4, LoadSceneMode.Additive);
    }

    public void GameOver()
    {
        limpiarPrefs();
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

    public void activarExcavadoras()
    {
        for(int i = 0; i < rayos.Length; i++)
        {
            rayos[i].SetActive(true);
        }
    }

    public void limpiarPrefs()
    {
        PlayerPrefs.SetInt("vidaTbyte", 0);
        PlayerPrefs.SetInt("escudoTbyte", 0);
        PlayerPrefs.SetInt("mejoraMinimapa", 0);
        PlayerPrefs.SetInt("mejoraMagnetismo", 0);
        PlayerPrefs.SetInt("mejoraImpulso", 0);
        PlayerPrefs.SetInt("mejoraEnergia", 0);
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
