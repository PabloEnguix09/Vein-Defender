using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public Text textoEstado;
    public GameObject textoRonda;
    public GameObject textoMenu;

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

    AudioHandler audioHandler;

    public GameObject victoryCanvas;

    public GameObject HudCanvas;

    void Start()
    {
        finMision = false;
        timerFinMision = 0;
        bases = (Base[])GameObject.FindObjectsOfType(typeof(Base));
        spawners = (Spawner[])GameObject.FindObjectsOfType(typeof(Spawner));
        personaje = jugador.GetComponent<Personaje>();
        CamaraSecundaria.SetActive(false);
        mejora.SetActive(false);
        ronda = 0;
        gameManager = FindObjectOfType<GameManager>();
        textoEstado.text = "";
        ultimoTexto = textoEstado.text;
        victoryCanvas.SetActive(false);
        // Audio
        audioHandler = GetComponent<AudioHandler>();
        // Cancion de la nave
        audioHandler.Play(0);

        mejoras.activarMejoras();
    }

    void Update()
    {
        if (finMision)
        {
            timerFinMision += Time.deltaTime;
            if (!victoryCanvas.activeSelf)
            {
                //la camara se situa en el cielo.
                victoryCanvas.SetActive(!victoryCanvas.activeSelf);
                textoEstado.text = "Volviendo a la nave en 10 segundos";
                ultimoTexto = textoEstado.text;
            }
            if (timerFinMision > tiempoEntreRondas)
            {
                // Carga del nivel 1
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

        // Muere el jugador
        if(personaje.Salud <= 0)
        {
            finDePartida = true;
        }

        // Jugador cae al suelo
        if(!personaje.movimientoPersonaje.volando && ronda == 0)
        {
            textoEstado.text = "Pulsa <k> para empezar";
            
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
            textoEstado.GetComponent<UnityEngine.UI.Text>().text = "Nuevos enemigos se aproximan";
            timer += Time.deltaTime;
            if(timer > tiempoEntreRondas)
            {
                textoEstado.GetComponent<UnityEngine.UI.Text>().text = "";
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
                            s.limitePrimerEnemigo += 2;                    
                            ultimoTexto = textoEstado.text;
                            s.SetRonda(ronda);
                            textoRonda.GetComponent<UnityEngine.UI.Text>().text = "Ronda 2";

                            // Cancion de la segunda ronda
                            audioHandler.Play(1);
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
                            s.limiteSegundoEnemigo += 3;
                            ultimoTexto = textoEstado.text;
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
                            s.limitePrimerEnemigo += 5;
                            s.limiteSegundoEnemigo = s.limiteSegundoEnemigo * 2;
                            s.limiteTercerEnemigo += 2;
                            ultimoTexto = textoEstado.text;
                            s.SetRonda(ronda);
                            textoRonda.GetComponent<UnityEngine.UI.Text>().text = "Ronda 4";

                            // Cancion de la ronda final
                            audioHandler.Play(2);
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
            HudCanvas.SetActive(false);
            GameOver();  
        }

        //Pulsar la <k> para empezar rondas/partida
        if (Input.GetKeyDown(KeyCode.K) && ronda == 0)
        {
            ronda = 1;
            activarExcavadoras();
            foreach(Spawner s in spawners)
            {
                s.SetRonda(ronda);
                textoEstado.text = "";
                ultimoTexto = textoEstado.text;
            }

            // termina el audio de la nave
            audioHandler.PauseAll();
            
        }

        //Salir al menu
        if (finDePartida && Input.GetKeyDown(KeyCode.Space))
        {
            Cursor.lockState = CursorLockMode.Confined;
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
        textoEstado.text = "Pulsa <Barra espaciadora> para volver al menú o <R> para reiniciar";
        ultimoTexto = textoEstado.text;

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
        if (interactuar)
        {
            textoEstado.text = "Pulsa <Click derecho> para interactuar";
        }
        else if (textoEstado.text.Equals("Pulsa <Click derecho> para interactuar"))
        {
            textoEstado.text = ultimoTexto;
        }
    }

    public void TextoPreview(bool preview)
    {
        if (preview)
        {
            textoEstado.text = "Pulsa <Click izquierdo> para colocar, <C> para cancelar";
        }
        else if(textoEstado.text.Equals("Pulsa <Click izquierdo> para colocar, <C> para cancelar"))
        {
            textoEstado.text = ultimoTexto;
        }
    }
}
