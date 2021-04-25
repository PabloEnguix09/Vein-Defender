// ---------------------------------------------------
// NAME: MenuButtons
// STATUS: DONE
// GAMEOBJECT: EventManager
// DESCRIPTION: Manager for menu buttons, start and exit game
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: onClick events
// ---------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{

    public int GameScene;
    public Sprite[] mapas;
    public Image mapa;
    public int siguienteNivel = 0;
    public GameObject mejoras, botonStart;

    GameManager gameManager;
    SistemaMejoras sistemaMejoras;

    bool mapaAbierto = false;

    // Recoge el game manager
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        sistemaMejoras = FindObjectOfType<SistemaMejoras>();
        // Si ya se ha completado una mision, se activa la seleccion de mejoras
        if(gameManager.misionesCompletadas > 0)
        {
            mejoras.SetActive(true);
            botonStart.SetActive(false);
        }
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene(GameScene);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnCLickShowMap()
    {
        mapa.sprite = mapas[siguienteNivel];
        mapaAbierto = true;
        mapa.gameObject.SetActive(mapaAbierto);
    }

    private void Update()
    {
        // Solo si el mapa esta abierto, selecciona el lugar donde bajar al mundo
        if(mapaAbierto)
        {
            if(Input.GetAxisRaw("Fire1") > 0)
            {
                SceneManager.LoadScene(GameScene);
            }
        }
    }

    public void ActivarMejora01()
    {
        sistemaMejoras.mejorap01 = true;
        mejoras.SetActive(false);
        botonStart.SetActive(true);
    }
}
