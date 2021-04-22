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
    // FEATURES ADDED: TorretaDestruida, ScriptableObjects
    // ---------------------------------------------------

    public TorretaBasica torretaBasica;

    [Header("Stats")]
    public string nombre;
    public int energia;
    public int energiaAlt;
    public float vidaActual;
    public float vidaMaxima;
    public float escudoMaximo;
    public float escudoRegen;
    public float ataque;
    public float cadenciaDisparo;
    public enum tipoDisparo
    {
        laser, balas
    }
    public Quaternion anguloDisparo;
    public float velocidadRotacion;
    public float distanciaDisparo;
    public bool antiaerea;

    private GameObject enemigoApuntando;
    private float timer;

    public Transform parteQueRota;
    private Disparo disparo;

    Personaje personaje;

    SistemaMejoras sistemaMejoras;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        // Asigna los valores de SO TorretaBasica
        nombre = torretaBasica.nombre;
        vidaMaxima = torretaBasica.vidaMaxima;
        energia = torretaBasica.energia;
        energiaAlt = torretaBasica.energiaAlt;
        ataque = torretaBasica.ataque;
        anguloDisparo = torretaBasica.anguloDisparo;
        velocidadRotacion = torretaBasica.velocidadRotacion;
        cadenciaDisparo = torretaBasica.cadenciaDisparo;

        // Busca el jugador
        personaje = FindObjectOfType<Personaje>();

        // busca el rigidbody
        rb = gameObject.GetComponent<Rigidbody>();

        // Asigna el sistema de mejoras 
        sistemaMejoras = FindObjectOfType<SistemaMejoras>();
        // LLama al sistema de mejoras
        sistemaMejoras.MejorasTorreta(this);

        // Ajusta la vida actual a la maxima
        vidaActual = vidaMaxima;
        
        // Reduce la energia del jugador
        if (personaje.Energia - energia < 0)
        {
            Destroy(gameObject);
        } else
        {
            personaje.Energia -= energia;
        }

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
            Vector3 rotacion = Quaternion.Lerp(parteQueRota.rotation, VisionRotacion, Time.deltaTime * velocidadRotacion).eulerAngles;
            parteQueRota.rotation = Quaternion.Euler(rotacion.x, rotacion.y, rotacion.z);

            //si ha pasado el tiempo de recarga
            if (timer >= cadenciaDisparo)
            {
                timer = 0;
                //disparar
                disparo.Disparar(ataque);
            }
        }

        // Torreta destruida
        if(vidaActual <= 0)
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
        Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, torretaBasica.distanciaDisparo);
        for (int i = 0; i < colliders.Length; i++)
        {

            if (colliders[i].CompareTag("Enemigos"))
            {
                //si el collider pertenece a un enemigo
                encontrado = true;
                RaycastHit hit;
                // no existe un collider entre el enemigo y la torreta
                if (!Physics.Linecast(transform.position, colliders[i].transform.position, out hit, 1, QueryTriggerInteraction.Ignore))
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
        personaje.Energia += energia;
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Terreno")
        {
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
    }
}
