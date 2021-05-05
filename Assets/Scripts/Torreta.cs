using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: Torreta.cs
    // STATUS: WIP
    // GAMEOBJECT: Torreta
    // DESCRIPTION: Modelo base para las Torreta
    //
    // AUTHOR: Adrian
    // FEATURES ADDED: La torreta apunta al enemigo mas cercano en un rango
    //
    // AUTHOR: Luis Belloch
    // FEATURES ADDED: TorretaDestruida, ScriptableObjects, apuntado de Enemigo rehecho
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
    public Quaternion anguloDisparo;

    public float radioExplosion;
    public float danyoExplosion;

    private float timerDisparo;

    [Header("Partes")]
    private GameObject enemigoApuntando;
    public Transform parteQueRota;
    public GameObject balaObjeto;
    public GameObject spawnerBalas;

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
        distanciaDisparo = torretaBasica.distanciaDisparo;

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

        timerDisparo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Solo si la torreta tiene ataque
        if(ataque > 0)
        {
            //busca al enemigo mas cercano
            enemigoApuntando = BuscarEnemigo();

            //tiempo para la velocidad de ataque
            timerDisparo += Time.deltaTime;

            //si apunta a alguien
            if (enemigoApuntando != null)
            {
                //rota la torreta en direccion al enemigo apuntado
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

                    Ataque ataqueObjeto = ScriptableObject.CreateInstance<Ataque>();

                    ataqueObjeto.fuerza = ataque;
                    ataqueObjeto.tipo = Ataque.Tipo.laser;
                    ataqueObjeto.origen = gameObject;
                    ataqueObjeto.fuerzaExplosion = danyoExplosion;
                    ataqueObjeto.radioExplosion = radioExplosion;

                    Bala bala = Instantiate(balaObjeto, spawnerBalas.transform.position, spawnerBalas.transform.rotation).GetComponent<Bala>();

                    bala.ataque = ataqueObjeto;
                }
            }
        }

        // Torreta destruida
        if (vidaActual <= 0)
        {
            DestruirTorreta();
        }

        // regeneracion de escudo
        if (escudoActual < escudoMaximo)
        {
            escudoActual += escudoRegen * Time.deltaTime;
        }

    }

    //Funcion de busqueda de enemigo
    GameObject BuscarEnemigo()
    {
        // Recogemos todos los Enemigo de la zona
        GameObject[] enemigosEnRango = GameObject.FindGameObjectsWithTag("Enemigo");

        GameObject masCercano = null;

        // Encontramos el enemigo mas cercano
        if(enemigosEnRango.Length >= 1)
        {
            for (int i = 0; i < enemigosEnRango.Length; i++)
            {
                // Si el enemigo es visible
                if (!enemigosEnRango[i].GetComponent<Enemigo>().invisibilidad){
                    // Si aun no ha encontrado ningun enemigo
                    if (masCercano == null)
                    {
                        // Comprueba que tenga vision del enemigo
                        if (ComprobarVision(enemigosEnRango[i]))
                        {
                            masCercano = enemigosEnRango[i];
                        }
                    }
                    // Si ya tiene un enemigo asignado
                    else if (masCercano != null)
                    {
                        // Si la distancia del actual es menor que la asignada, se asigna el actual como masCercano
                        if (Vector3.Distance(parteQueRota.position, masCercano.transform.position) > Vector3.Distance(parteQueRota.position, enemigosEnRango[i].transform.position))
                        {
                            //Comprobamos que no este marcado
                            if (masCercano.GetComponent<Enemigo>().marcado == false)
                            {
                                // Comprueba que tenga vision del enemigo
                                if (ComprobarVision(enemigosEnRango[i]))
                                {
                                    masCercano = enemigosEnRango[i];
                                }
                            }

                        }
                    }
                }
                
            }
        }
        // Comprueba que existe un enemigo visible
        if(masCercano != null)
        {
            // Ahora que tenemos la torreta mas cercana devolvemos el GameObject si esta dentro del rango de disparo
            if (Vector3.Distance(parteQueRota.position, masCercano.transform.position) < distanciaDisparo)
            {
                return masCercano;
            }
        }
        // Si no, devuelve un null
        return null;
    }

    // Comprueba que el enemigo apuntado no tenga ninguna pared entre la torreta y el
    bool ComprobarVision(GameObject objetivo)
    {
        // Comprobamos que no tenga terreno entre la torreta y el enemigo
        RaycastHit hit;
        // no existe un collider entre el enemigo y la torreta
        Physics.Raycast(parteQueRota.position, Vector3.Normalize(objetivo.transform.position - parteQueRota.position), out hit, Vector3.Distance(parteQueRota.position, objetivo.transform.position), LayerMask.GetMask("Terreno"));

        if(hit.collider == null)
        {
            return true;
        }
        return false;
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

    // Recibe un ataque
    public void RecibirAtaque(Ataque ataque)
    {

        // El escudo reduce el ataque 
        if (escudoActual > 0)
        {
            if (escudoActual < ataque.fuerza)
            {
                float aux = ataque.fuerza - escudoActual;
                escudoActual = 0;
                vidaActual -= aux;
            }
            else
            {
                // El resto del escudo golpea en la vida
                escudoActual -= ataque.fuerza;
            }
        }
        else
        {
            vidaActual -= ataque.fuerza;
        }
       
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if(enemigoApuntando != null)
        {
            Gizmos.DrawRay(parteQueRota.position, Vector3.Normalize(enemigoApuntando.transform.position - parteQueRota.position) * Vector3.Distance(parteQueRota.position, enemigoApuntando.transform.position));
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(parteQueRota.position, distanciaDisparo);
    }
}