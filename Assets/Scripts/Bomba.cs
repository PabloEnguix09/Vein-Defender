// ---------------------------------------------------
// NAME: Bomba.cs
// STATUS: WIP
// GAMEOBJECT: Enemigo
// DESCRIPTION: Aqui se reunen las capacidades especiales del enemigo bomba y sus estadisticas
//
// AUTHOR: Jorge Grau
// FEATURES ADDED: Añadidas las estadisticas, la explosion al chocar y el daño de explosion en area.
// ---------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bomba : MonoBehaviour
{

    public GameObject explosion;

    Animator animator;
    public float destroyOfftime = 0.5f;

    [Range(0, 1)]
    public float fuerza = .3f;

    [Range(0, 1)]
    public float vida = .5f;

    public float velocidad = 10;

    public float rango = 4;

    public float radioExplosion = 2f;

    private Enemigo enemigo;

    // Start is called before the first frame update
    void Start()
    {
        enemigo = gameObject.GetComponent<Enemigo>();
        enemigo.fuerza = fuerza;
        enemigo.vida = vida;
        enemigo.vision = rango;
        enemigo.agente.speed = velocidad;
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (enemigo.vida <= 0)
        {
            // Inicia la animacion de explotar
            animator.SetBool("Explode", true);

            // Cuenta regresiva hasta terminar la animacion de explotar
            destroyOfftime -= Time.deltaTime;
            
            // Destruye el gameobject y crea las particulas
            if (destroyOfftime <= 0)
            {
                Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
                Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, radioExplosion);

                // Inflinge daño a todos los objetivos dentro del rango
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].CompareTag("Bases"))
                    {
                        Base estructura = colliders[i].gameObject.GetComponent<Base>();
                        estructura.Salud -= enemigo.fuerza;
                        //Debug.Log("Golpeo una base");
                    }

                    else if (colliders[i].CompareTag("Torretas"))
                    {
                        Torreta estructura = colliders[i].gameObject.GetComponent<Torreta>();
                        estructura.Vida -= enemigo.fuerza;
                        //Debug.Log("Golpeo una torrerta");
                    }

                    else if (colliders[i].CompareTag("Enemigos"))
                    {
                        Enemigo otroEnemigo = colliders[i].gameObject.GetComponent<Enemigo>();
                        otroEnemigo.Vida -= enemigo.fuerza;
                        //Debug.Log("Golpeo un enemigo");
                    }

                    else if (colliders[i].CompareTag("Player"))
                    {
                        Personaje personaje = colliders[i].gameObject.GetComponent<Personaje>();
                        personaje.Salud -= enemigo.fuerza;
                        //Debug.Log("Golpeo al jugador");
                    }
                }
                //Debug.Log("Explota");
                Destroy(gameObject);
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        // Cuando choca contra una base, torreta o personaje se autodestruye
        if (other.gameObject.GetComponent<Base>() != null || other.gameObject.GetComponent<Torreta>() != null || other.gameObject.GetComponent<Personaje>())
        {
            //Debug.Log("Choque");
            enemigo.vida = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }
}
