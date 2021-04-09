using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorretaBasica : MonoBehaviour
{

    [Range(0, 1)]
    [SerializeField]
    public float vida = 0.3f;

    [Range(0, 1)]
    [SerializeField]
    public float fuerza = 0.4f;

    public float velocidadAtaque = 1f;

    public float rango = 40f;

    private Torreta torreta;

    // Start is called before the first frame update
    void Start()
    {
        torreta = gameObject.GetComponent<Torreta>();
        torreta.fuerza = fuerza;
        torreta.vida = vida;
        torreta.velocidadAtaque = velocidadAtaque;
        torreta.rango = rango;
    }

    // Update is called once per frame
    void Update()
    {
        if (torreta.vida <= 0)
        {
            Debug.Log("Torreta muere");
            Destroy(gameObject);
        }
    }
}
