using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bomba : MonoBehaviour
{
    public GameObject explosion;

    [Range(0, 1)]
    private float fuerza = .3f;

    [Range(0, 1)]
    private float vida = .5f;

    private float velocidad = 5f;

    private float rango = 5;

    public float radioExplosion = 2f;

    private Enemigo enemigo;

    // Start is called before the first frame update
    void Start()
    {
        enemigo = gameObject.GetComponent<Enemigo>();
        enemigo.fuerza = fuerza;
        enemigo.vida = vida;
        enemigo.velocidad = velocidad;
        enemigo.vision = rango;


    }

    void Update()
    {
        if (enemigo.vida <= 0)
        {
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, radioExplosion);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].CompareTag("Bases"))
                {
                    Base estructura = colliders[i].gameObject.GetComponent<Base>();
                    estructura.Salud -= enemigo.fuerza;
                    Debug.Log("Golpeo una base");
                }

                else if (colliders[i].CompareTag("Torretas"))
                {
                    Torreta estructura = colliders[i].gameObject.GetComponent<Torreta>();
                    estructura.Vida -= enemigo.fuerza;
                    Debug.Log("Golpeo una torrerta");
                }
                
                else if (colliders[i].CompareTag("Enemigos"))
                {
                    Enemigo otroEnemigo = colliders[i].gameObject.GetComponent<Enemigo>();
                    otroEnemigo.Vida -= enemigo.fuerza;
                    Debug.Log("Golpeo un enemigo");
                }
            }
            Debug.Log("Explota");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Base>() != null || other.gameObject.GetComponent<Torreta>() != null)
        {
            Debug.Log("Choque");
            enemigo.vida = 0;
        }
    }
}
