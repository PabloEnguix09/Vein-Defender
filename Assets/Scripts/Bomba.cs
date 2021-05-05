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

    Animator animator;
    public float destroyOfftime = 0.5f;
    public float vida;
    public float radioExplosion = 2f;

    public EnemigoBasico enemigoBasico;

    private Enemigo enemigo;

    // Start is called before the first frame update
    void Start()
    {
        enemigo = gameObject.GetComponent<Enemigo>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (enemigo.vidaActual <= 0)
        {
            // Inicia la animacion de explotar comentado porque peta
            //animator.SetBool("Explode", true);

            // Cuenta regresiva hasta terminar la animacion de explotar
            destroyOfftime -= Time.deltaTime;
            
            // Destruye el gameobject y crea las particulas
            if (destroyOfftime <= 0)
            {
                Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
                Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, radioExplosion);

                Ataque ataqueObjeto = ScriptableObject.CreateInstance<Ataque>();

                ataqueObjeto.fuerza = enemigoBasico.ataque;
                ataqueObjeto.tipo = Ataque.Tipo.laser;
                ataqueObjeto.origen = gameObject;

                // Inflinge da�o a todos los objetivos dentro del rango
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].CompareTag("Base"))
                    {
                        Base estructura = colliders[i].gameObject.GetComponent<Base>();
                        estructura.RecibirAtaque(ataqueObjeto);
                    }

                    else if (colliders[i].CompareTag("Torreta"))
                    {
                        Torreta estructura = colliders[i].gameObject.GetComponent<Torreta>();

                        estructura.RecibirAtaque(ataqueObjeto);
                    }

                    else if (colliders[i].CompareTag("Enemigo"))
                    {
                        Enemigo otroEnemigo = colliders[i].gameObject.GetComponent<Enemigo>();
                        otroEnemigo.RecibirAtaque(ataqueObjeto);
                    }

                    else if (colliders[i].CompareTag("Player"))
                    {
                        Personaje personaje = colliders[i].gameObject.GetComponent<Personaje>();
                        personaje.RecibirAtaque(ataqueObjeto);
                    }
                }
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
            enemigo.vidaActual = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radioExplosion);
    }
}
