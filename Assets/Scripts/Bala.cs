using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: Bala.cs
    // STATUS: WIP
    // GAMEOBJECT: Bala
    // DESCRIPTION: Control de las balas disparadas por las Torreta
    //
    // AUTHOR: Adri�n
    // FEATURES ADDED: La bala con impulso y se destruye al golpear algo.
    //
    // AUTHOR: Pau
    // FEATURES ADDED: Inflingir daño a player, Torreta y base
    // ---------------------------------------------------


    public float velocidad;
    public GameObject explosion;
    public Ataque ataque;
    public int enemigosPerforados;
    public Transform perseguir;

    Rigidbody rb;

    void Start()
    {
        velocidad = 100f;
        rb = GetComponent<Rigidbody>();

        if(ataque.origen.CompareTag("Torreta"))
        {
            if (ataque.tipo == Ataque.Tipo.laser && ataque.origen.GetComponent<Torreta>().perforante)
            {
                enemigosPerforados = 3;
            }
            else
            {
                enemigosPerforados = 0;
            }
            if(ataque.tipo == Ataque.Tipo.laser && ataque.origen.GetComponent<Torreta>().perseguidor)
            {
                perseguir = ataque.origen.GetComponent<Torreta>().GetEnemigoApuntado().transform;
            }
        }

        //Darle impulso de la bala
        rb.AddForce(transform.forward * velocidad, ForceMode.Impulse);
        
    }
    private void Update(){
        // Disparos perseguidores
        if(perseguir != null)
        {
            if (ataque.tipo == Ataque.Tipo.laser)
            {
                if (ataque.origen.tag == "Torreta")
                {
                    if (ataque.origen.GetComponent<Torreta>().perseguidor)
                    {
                        rb.MovePosition(perseguir.position);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // compruebo si golpeo una base y aplico daño
        
        if (collision.gameObject.CompareTag("Base"))
        {

            if (!ataque.origen.CompareTag("Torreta"))
            {
                Base estructura = collision.gameObject.gameObject.GetComponent<Base>();
                estructura.RecibirAtaque(ataque);
                
                ExplosionAtaque(ataque);
            }
        }
        // compruebo si golpeo una torreta y aplico daño
        if (collision.gameObject.CompareTag("Torreta"))
        {
            if (!ataque.origen.CompareTag("Torreta"))
            {
                // Recoje el script torreta
                Torreta torreta = collision.gameObject.gameObject.GetComponent<Torreta>();
                // Inflige danyo
                
                torreta.RecibirAtaque(ataque);
                ExplosionAtaque(ataque);
            }
        }
        // compruebo si golpeo un jugador y aplico daño
        if (collision.gameObject.CompareTag("Player"))
        {
            if(!ataque.origen.CompareTag("Torreta"))
            {
                Personaje personaje = collision.gameObject.gameObject.GetComponent<Personaje>();
                personaje.RecibirAtaque(ataque);

                ExplosionAtaque(ataque);
            }

        }
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            if (!ataque.origen.CompareTag("Enemigo"))
            {
                ControladorEntidad enemigo = collision.gameObject.GetComponent<ControladorEntidad>();
                if (!enemigo.stats.subterraneo)
                {
                    if (ataque.origen.GetComponent<Torreta>().balaObjeto.name == "Bala" && ataque.origen.GetComponent<Torreta>().disparoPEM)
                    {                        // TODO: debe recibir un ataque que le ralentice
                        //enemigo.RecibeAtaque();
                    }
                    if(ataque.origen.GetComponent<Torreta>().disparoMarcado && enemigo.marcado)
                    {
                        ataque.fuerza *= 0.2f;
                    }
                    enemigo.RecibeAtaque(ataque);
                }
                ExplosionAtaque(ataque);
            }
        } 
        if(collision.gameObject.CompareTag("Terreno"))
        {
            ExplosionAtaque(ataque);
        }
    }
    // Busca objetivos cerca del punto de impacto
    private void ExplosionAtaque(Ataque ataque){
        if (ataque.origen.CompareTag("Torreta"))        {
            if (ataque.tipo == Ataque.Tipo.laser && ataque.origen.GetComponent<Torreta>().perforante)
            {
                enemigosPerforados--;
            }
        }

        if (ataque.radioExplosion > 0)
        {
            // Recoje todos los colliders dentro del rango y les aplica el ataque
            Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, ataque.radioExplosion);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].CompareTag("Base"))
                {
                    Base estructura = colliders[i].gameObject.GetComponent<Base>();
                    estructura.RecibirAtaque(ataque);
                }

                if (colliders[i].CompareTag("Torreta"))
                {
                    Torreta estructura = colliders[i].gameObject.GetComponent<Torreta>();

                    estructura.RecibirAtaque(ataque);
                }

                if (colliders[i].CompareTag("Enemigo"))
                {
                    ControladorEntidad otroEnemigo = colliders[i].gameObject.GetComponent<ControladorEntidad>();
                    otroEnemigo.RecibeAtaque(ataque);
                }

                if (colliders[i].CompareTag("Player"))
                {
                    Personaje personaje = colliders[i].gameObject.GetComponent<Personaje>();
                    personaje.RecibirAtaque(ataque);
                }
                if(colliders[i].CompareTag("Terreno"))
                {
                    Destroy(gameObject);
                }
            }
        }

        if(enemigosPerforados == 0)
        {
            Destroy(gameObject);
        }
    }
}
