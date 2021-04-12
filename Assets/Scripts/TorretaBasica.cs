using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorretaBasica : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: nombre
    // STATUS: estado
    // GAMEOBJECT: objeto
    // DESCRIPTION: descripcion
    //
    // AUTHOR: Jorge
    // FEATURES ADDED: Añadido gasto energetico
    // ---------------------------------------------------

    [Range(0, 1)]
    [SerializeField]
    public float vida = 0.3f;

    [Range(0, 1)]
    [SerializeField]
    public float fuerza = 0f;

    public float velocidadAtaque = 1f;

    public float rango = 40f;

    public float gastoEnergia = 1;

    private Torreta torreta;

    // Start is called before the first frame update
    void Start()
    {
        torreta = gameObject.GetComponent<Torreta>();
        torreta.fuerza = fuerza;
        torreta.vida = vida;
        torreta.velocidadAtaque = velocidadAtaque;
        torreta.rango = rango;
        torreta.gastoEnergia = gastoEnergia;
    }

    // Update is called once per frame
    void Update()
    {
        if (torreta.vida <= 0)
        {
            Debug.Log("Torreta muere");
            GameObject jugador = GameObject.FindGameObjectWithTag("Player");
            Personaje personaje = jugador.GetComponent<Personaje>();
            personaje.Energia += gastoEnergia;
            Destroy(gameObject);
        }
    }
}
