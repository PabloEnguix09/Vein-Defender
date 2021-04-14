using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: Torreta.cs
    // STATUS: WIP
    // GAMEOBJECT: Torreta
    // DESCRIPTION: Modelo base para las torretas
    //
    // AUTHOR: Adrian
    // FEATURES ADDED: La torreta apunta al enemigo mas cercano en un rango
    //
    // AUTHOR: Luis Belloch
    // FEATURES ADDED: TorretaDestruida
    // ---------------------------------------------------

    [Range(0, 1)]
    [SerializeField]
    public float vida;

    public float Vida
    {
        get { return vida; }

        set
        {
            value = Mathf.Clamp01(value);
            vida = value;
        }
    }

    [Range(0, 1)]
    [SerializeField]
    public float fuerza;

    public float Fuerza
    {
        get { return fuerza; }

        set
        {
            value = Mathf.Clamp01(value);
            fuerza = value;
        }
    }

    public float velocidadAtaque;

    public float VelocidadAtaque
    {
        get { return velocidadAtaque; }

        set
        {
            value = Mathf.Clamp01(value);
            velocidadAtaque = value;
        }
    }

    public float rango;

    public float Rango
    {
        get { return rango; }

        set
        {
            value = Mathf.Clamp01(value);
            rango = value;
        }
    }

    public float gastoEnergia;

    private GameObject enemigoApuntando;
    private float timer;

    public Transform parteQueRota;
    public float velocidadGiro = 10f;
    private Disparo disparo;

    Personaje personaje;

    // Start is called before the first frame update
    void Start()
    {
        // Busca el jugador
        personaje = FindObjectOfType<Personaje>();

        //No apuntar a nadie
        enemigoApuntando = null;

        disparo = GetComponentInChildren<Disparo>();
    }

    // Update is called once per frame
    void Update()
    {
        //busca al enemigo mas cercano
        enemigoApuntando = BuscarEnemigo();

        //tiempo para la velocidad de ataque
        timer += Time.deltaTime;

        //si apunta a alguien
        if (enemigoApuntando != null)
        {
            //rota la torreta en dirección al enemigo apuntado
            Vector3 dir = parteQueRota.position - enemigoApuntando.transform.position ;
            Quaternion VisionRotacion = Quaternion.LookRotation(dir);
            //rotacion suave
            Vector3 rotacion = Quaternion.Lerp(parteQueRota.rotation, VisionRotacion, Time.deltaTime * velocidadGiro).eulerAngles;
            parteQueRota.rotation = Quaternion.Euler(rotacion.x, rotacion.y, rotacion.z);

            //si ha pasado el tiempo de recarga
            if (timer >= velocidadAtaque)
            {
                timer = 0;
                //disparar
                disparo.Disparar();
            }
        }

        // Torreta destruida
        if(vida <= 0)
        {
            DestruirTorreta();
        }
    }

    //Funcion de busqueda de enemigo
    GameObject BuscarEnemigo()
    {
        float menorDistancia = Mathf.Infinity;
        bool encontrado = false;
        GameObject enemigoMasCercano = null;

        //generar una esfera de radio <rango> alrededor de la torreta y guardar todas las colisiones en una lista
        Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, rango);
        for (int i = 0; i < colliders.Length; i++)
        {

            if (colliders[i].CompareTag("Enemigos"))
            {
                //si el collider pertenece a un enemigo
                encontrado = true;
                RaycastHit hit;
                if (Physics.Linecast(transform.position, colliders[i].transform.position, out hit, 1, QueryTriggerInteraction.Ignore))
                {
                    //existe un collider entre el enemigo y la torreta
                }
                else
                {
                    //si todavía no apunta a nadie
                    if (enemigoMasCercano == null)
                    {
                        enemigoMasCercano = colliders[i].gameObject;
                        menorDistancia = Vector3.Distance(colliders[i].gameObject.transform.position, transform.position);
                    }
                    else
                    {
                        //comprueba la distancia entre la torreta y el enemigo
                        float distancia = Vector3.Distance(colliders[i].gameObject.transform.position, transform.position);
                        //si está mas cerca que el anterior enemigo mas cercano lo sustituye
                        if (distancia < menorDistancia)
                        {
                            menorDistancia = distancia;
                            enemigoMasCercano = colliders[i].gameObject;
                        }
                    }
                }


            }
        }
        if (encontrado) return enemigoMasCercano;
        else return null;
    }

    // Llamado desde InvocarTorretas.cs EliminarTorreta()
    public void DestruirTorreta()
    {
        // Destruye la torreta y devuelve la energia al jugador
        personaje.energia += gastoEnergia;
        Destroy(gameObject);
    }
}
