using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: Base.cs
    // STATUS: WIP
    // GAMEOBJECT: base
    // DESCRIPTION: Este script recoge las caracteristicas de la base. Por ahora, la salud.
    //
    // AUTHOR: Jorge Grau
    // FEATURES ADDED: Salud de la base
    // ---------------------------------------------------

    [SerializeField]
    public float salud = 1;

    public GameObject modelo;
    public Image romboActual;
    public GameObject marcador;
    public Sprite romboAtaque;
    Sprite romboSeguro;
    private float timer;
    private bool atacado;

    public float Salud
    {
        get { return salud; }

        set
        {
            value = Mathf.Clamp01(value);
            salud = value;
            if (salud <= 0)
            {
                // Debug.Log("Destruido");
                modelo.SetActive(false);
            }
        }
    }

    private void Start()
    {
        modelo.SetActive(true);
        romboSeguro = romboActual.sprite;
        timer = 0;
    }

    private void Update()
    {
        if(atacado)
        {
            timer += Time.deltaTime;
        }
        if(timer >= 2)
        {
            atacado = false;
            romboActual.sprite = romboSeguro;
            timer = 0;
        }
    }

    public void RecibirAtaque(Ataque ataque)
    {
        salud -= ataque.fuerza;
        romboActual.sprite = romboAtaque;
        atacado = true;
        if(salud <= 0)
        {
            Destroy(gameObject);
            Destroy(romboActual.gameObject);
            Destroy(marcador);
        }
    }
}
