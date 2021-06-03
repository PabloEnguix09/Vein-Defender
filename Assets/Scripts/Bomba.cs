// ---------------------------------------------------
// NAME: Bomba.cs
// STATUS: WIP
// GAMEOBJECT: Enemigo
// DESCRIPTION: Aqui se reunen las capacidades especiales del enemigo bomba y sus estadisticas
//
// AUTHOR: Jorge Grau
// FEATURES ADDED: A�adidas las estadisticas, la explosion al chocar y el da�o de explosion en area.
// ---------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bomba : MonoBehaviour
{

    public GameObject explosion;

    public float destroyOfftime = 0.5f;
    public float vida;
    public float radioExplosion = 2f;

    public EnemigoBasico enemigoBasico;

    private Enemigo enemigo;

    AudioHandler audioHandler;

    // Start is called before the first frame update
    void Start()
    {
        enemigo = gameObject.GetComponent<Enemigo>();

        audioHandler = gameObject.GetComponent<AudioHandler>();
    }

    void Update()
    {
        if (enemigo.vidaActual <= 0)
        {
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        // Cuando choca contra una base, torreta o personaje se autodestruye
        if (other.gameObject.GetComponent<Base>() != null || other.gameObject.GetComponent<Torreta>() != null || other.gameObject.GetComponent<Personaje>())
        {
            //Debug.Log("Choque");
            enemigo.vidaActual = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }
}
