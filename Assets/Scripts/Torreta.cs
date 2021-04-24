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
    //
    // AUTHOR: Pau Blanes
    //FEATURES ADDED: regenreacion de escudo y rango de vision
    // ---------------------------------------------------

    public TorretaBasica torretaBasica;

    [Header("Stats")]
    public string nombre;
    public int energia;
    public int energiaAlt;
    public float vidaActual;
    public float vidaMaxima;
    public float escudoMaximo;
    public float escudoActual;
    public float escudoRegen;
    public float ataque;
    public float cadenciaDisparo;
    public float velocidadRotacion;
    public float distanciaDisparo;
    public bool antiaerea;

    public float radioExplosion;
    public float danyoExplosion;

    private float timerDisparo;
    private float timerEscudo;


    private GameObject enemigoApuntando;
    public Transform parteQueRota;
    private Disparo disparo;
    public Quaternion anguloDisparo;
    Personaje personaje;

    SistemaMejoras sistemaMejoras;
    Rigidbody rb;
    public enum tipoDisparo
    {
        laser, balas
    }


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
        escudoActual = torretaBasica.escudoActual;
        escudoMaximo = torretaBasica.escudoMaximo;
        escudoRegen = torretaBasica.escudoRegen;
        radioExplosion = torretaBasica.radioExplosion;
        danyoExplosion = torretaBasica.danyoExplosion;

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
        }
        else
        {
            personaje.Energia -= energia;
        }

        //No apuntar a nadie
        enemigoApuntando = null;

        disparo = GetComponentInChildren<Disparo>();

        timerDisparo = 0;
        timerEscudo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //busca al enemigo mas cercano
        enemigoApuntando = BuscarEnemigo();

        //tiempo para la velocidad de ataque
        timerDisparo += Time.deltaTime;

        //si apunta a alguien
        if (enemigoApuntando != null)
        {
            //rota la torreta en dirección al enemigo apuntado
            Vector3 dir = parteQueRota.position - enemigoApuntando.transform.position;
            Quaternion VisionRotacion = Quaternion.LookRotation(dir);
            //rotacion suave
            Vector3 rotacion = Quaternion.Lerp(parteQueRota.rotation, VisionRotacion, Time.deltaTime * velocidadRotacion).eulerAngles;
            parteQueRota.rotation = Quaternion.Euler(rotacion.x, rotacion.y, rotacion.z);

            //si ha pasado el tiempo de recarga
            if (timerDisparo >= cadenciaDisparo)
            {
                timerDisparo = 0;
                //disparar
                disparo.Disparar(ataque,radioExplosion,danyoExplosion);
            }
        }

        // Torreta destruida
        if (vidaActual <= 0)
        {
            DestruirTorreta();
        }

        if (escudoActual <= escudoMaximo)
        {
            //tiempo para la recarga de escudo
            timerEscudo += Time.deltaTime;
            if (timerEscudo >= 1)
            {
                if ((escudoActual + escudoRegen) > escudoMaximo)
                {
                    escudoActual = escudoMaximo;
                }
                else
                {
                    escudoActual += escudoRegen;
                }
                timerEscudo = 0;
            }

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

                        Vector3 dir2 = parteQueRota.position - colliders[i].gameObject.transform.position;
                        Quaternion VisionRotacion = Quaternion.LookRotation(dir2);
                        float aux = VisionRotacion.eulerAngles.y - this.transform.rotation.eulerAngles.y;
                        if (aux < 0)
                        {
                            aux += 360;
                        }
                        if (anguloDisparo.y >= aux || 360 - anguloDisparo.y <= aux)
                        {
                            enemigoMasCercano = colliders[i].gameObject;
                            menorDistancia = Vector3.Distance(colliders[i].gameObject.transform.position, transform.position);

                        }
                    }
                    else
                    {
                        Vector3 dir2 = parteQueRota.position - colliders[i].gameObject.transform.position;
                        Quaternion VisionRotacion = Quaternion.LookRotation(dir2);
                        float aux = VisionRotacion.eulerAngles.y - this.transform.rotation.eulerAngles.y;
                        if (aux < 0)
                        {
                            aux += 360;
                        }
                        if (anguloDisparo.y >= aux || 360 - anguloDisparo.y <= aux)
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
        if (collision.gameObject.tag == "Terreno")
        {
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
    }
}