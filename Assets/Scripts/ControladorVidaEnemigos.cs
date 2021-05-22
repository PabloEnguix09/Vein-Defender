using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorVidaEnemigos : MonoBehaviour
{
    public EnemigoBasico enemigo;
    private float vidaActual;
    public GameObject explosion;
    void Update()
    {
        vidaActual = enemigo.vidaActual;
        if (vidaActual <= 0)
        {
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}