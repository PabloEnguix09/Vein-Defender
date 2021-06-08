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
    // FEATURES ADDED: La torreta apunta al EnemigoControlador.stats mas cercano en un rango
    //
    // AUTHOR: Luis Belloch
    // FEATURES ADDED: TorretaDestruida, ScriptableObjects, apuntado de EnemigoControlador.stats rehecho
    //
    // AUTHOR: Pau Blanes
    //FEATURES ADDED: regenreacion de escudo y rango de vision
    //
    // AUTHOR: Jorge Grau
    //FEATURES ADDED: comprobaci�n de que la torreta es antiaerea (puede atacar enemigos voladores), EnemigoControlador.stats subterraneo y estado de invisibilidad(los enemigos invisibles no pueden ser atacados a menos que pierdan la ivisibilidad), a�adido tambien la variable energiaEnUso, que guarda el total de energia que consume una torreta en el momento que esta siendo usada, ej: la fantasma gasta 3 si esta invisible gasta 6. Las torretas pueden reducir el da�o y sabemos la direccion del ataque.
    //
    //AUTHOR: Juan Ferrera Sala
    //FEATURES ADDED:Si el EnemigoControlador.stats es visible esta marcado le esta disparndo una Mohawk
    // ---------------------------------------------------

    public TorretaSO torretaBasica;

    [Header("Stats")]
    public string nombre;
    public int energia;
    public int energiaAlt;
    public int energiaEnUso;
    public float vidaActual;
    public float vidaMaxima;
    public float escudoMaximo;
    public float escudoActual;
    public float escudoRegen;
    public float timerEscudo;
    float actualTimerEscudo;
    public float ataque;
    public float cadenciaDisparo;
    public float velocidadRotacion;
    public float distanciaDisparo;
    public bool antiaerea;
    public bool invisibilidad;
    public bool escudo;
    public bool perforante;
    public bool perseguidor;
    public bool disparoPEM;
    public bool disparoMarcado;
    public Quaternion anguloDisparo;

    public float radioExplosion;
    public float danyoExplosion;

    private float timerDisparo;
    private float timerActivacion;
    public bool torretaInhabilitada;


    [Header("Partes")]
    private GameObject enemigoApuntando;
    public Transform parteQueRota;
    public GameObject balaObjeto;
    public GameObject spawnerBalas;
    AudioHandler audioHandler;
    public GameObject escudoObjeto;

    Personaje personaje;

    SistemaMejoras sistemaMejoras;
    Rigidbody rb;

    public GameObject explosionElectrica;
    public ParticleSystem explosionBala;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Asigna los valores de SO TorretaBasica
        nombre = torretaBasica.name;
        energia = torretaBasica.energia;
        energiaAlt = torretaBasica.energiaAlt;
        energiaEnUso = torretaBasica.energia;
        vidaActual = torretaBasica.vidaMaxima;
        vidaMaxima = torretaBasica.vidaMaxima;
        escudoMaximo = torretaBasica.escudo.escudoMaximo;
        escudoActual = escudoMaximo;
        escudoRegen = torretaBasica.escudo.escudoRegen;
        ataque = torretaBasica.ataque;
        cadenciaDisparo = torretaBasica.cadenciaDisparo;
        velocidadRotacion = torretaBasica.velocidadRotacion;
        distanciaDisparo = torretaBasica.distanciaDisparo;
        antiaerea = torretaBasica.variantes.antiaerea;
        invisibilidad = torretaBasica.variantes.invisibilidad;
        anguloDisparo = torretaBasica.anguloDisparo;
        radioExplosion = torretaBasica.radioExplosion;
        danyoExplosion = torretaBasica.danyoExplosion;
        // Empieza estando inhabilitada
        torretaInhabilitada = true;

        // Busca el jugador
        personaje = FindObjectOfType<Personaje>();

        // busca el rigidbody
        rb = gameObject.GetComponent<Rigidbody>();

        // Asigna el sistema de mejoras 
        sistemaMejoras = FindObjectOfType<SistemaMejoras>();
        // LLama al sistema de mejoras
        sistemaMejoras.MejorasTorreta(this);

        // Busca el animador
        animator = gameObject.GetComponent<Animator>();

        // Busca el AudioHandler
        audioHandler = gameObject.GetComponent<AudioHandler>();

        // Reduce la energia del jugador
        if (personaje.Energia - energiaEnUso < 0)
        {
            Destroy(gameObject);
        }
        else
        {
            personaje.Energia -= energiaEnUso;
        }

        //No apuntar a nadie
        enemigoApuntando = null;

        timerDisparo = 0;
        timerActivacion = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetBool("Inactiva", torretaInhabilitada);

        if (!torretaInhabilitada)
        {
            // Solo si la torreta tiene ataque
            if (ataque > 0)
            {
                //busca al EnemigoControlador.stats mas cercano
                enemigoApuntando = BuscarEnemigo();

                //tiempo para la velocidad de ataque
                timerDisparo += Time.deltaTime;

                //si apunta a alguien
                if (enemigoApuntando != null)
                {
                    //rota la torreta en direccion al EnemigoControlador.stats apuntado
                    Vector3 dir = parteQueRota.position - enemigoApuntando.transform.position;
                    Quaternion VisionRotacion = Quaternion.LookRotation(dir);
                    //rotacion suave
                    Vector3 rotacion = Quaternion.Lerp(parteQueRota.rotation, VisionRotacion, Time.deltaTime * velocidadRotacion).eulerAngles;
                    parteQueRota.rotation = Quaternion.Euler(rotacion.x, rotacion.y, rotacion.z);

                    timerDisparo += Time.deltaTime;
                    if (timerDisparo > cadenciaDisparo)
                    {
                        timerDisparo = 0;
                        Disparar();
                    }
                }
            }



            // Torreta destruida
            if (vidaActual <= 0)
            {
                if (!torretaBasica.variantes.regeneracion)
                {
                    DestruirTorreta();
                }
            }

            // Regeneracion de escudo
            if (escudoActual < escudoMaximo)
            {
                actualTimerEscudo -= Time.deltaTime;
                // Cuando el han pasado unos segundos sin recibir ataques
                if (actualTimerEscudo <= 0)
                {
                    escudoActual += escudoRegen * Time.deltaTime;
                }
            }
            // Si el escudo es mayor que el maximo, se ajusta al maximo
            if (escudoActual > escudoMaximo)
            {
                escudoActual = escudoMaximo;
            }

            // Si tiene un escudo dentro
            if (escudoObjeto != null)
            {
                // Si el escudo esta activado pero ya no tiene escudo, se desactiva
                if (escudoObjeto.activeSelf && escudoActual <= 0)
                {
                    escudoObjeto.SetActive(false);
                }
                // Si el escudo esta desactivado pero tiene escudo, se activa
                if (!escudoObjeto.activeSelf && escudoActual > 0)
                {
                    escudoObjeto.SetActive(true);
                }
            }
        }
        else
        {
            //tiempo para la velocidad de ataque
            timerActivacion += Time.deltaTime;
            if (timerActivacion >= 2)
            {
                torretaInhabilitada = false;
                timerActivacion = 0;
            }
        }
    }

    public void Disparar()
    {
        // Animacion de disparo si lanza balas
        if (torretaBasica.tipoDisparo == TorretaSO.TipoDisparo.balas)
        {
            animator.SetTrigger("Disparo");
        }
        // sonido disparar
        // audioHandler.Play(0);
        // Crea las particulas del disparo si el tipo es de balas
        if (torretaBasica.tipoDisparo == TorretaSO.TipoDisparo.balas)
        {
            Destroy(Instantiate(explosionBala, spawnerBalas.transform.position, spawnerBalas.transform.rotation), 1f);
        }

        Ataque ataqueObjeto = ScriptableObject.CreateInstance<Ataque>();

        ataqueObjeto.fuerza = ataque;
        ataqueObjeto.tipo = Ataque.Tipo.laser;
        ataqueObjeto.origen = gameObject;
        ataqueObjeto.fuerzaExplosion = danyoExplosion;
        ataqueObjeto.radioExplosion = radioExplosion;


        Bala bala = Instantiate(balaObjeto, spawnerBalas.transform.position, spawnerBalas.transform.rotation).GetComponent<Bala>();

        bala.ataque = ataqueObjeto;
    }

    //Funcion de busqueda de EnemigoControlador.stats
    GameObject BuscarEnemigo()
    {
        // Recogemos todos los EnemigoControlador.stats de la zona
        GameObject[] enemigosEnRango = GameObject.FindGameObjectsWithTag("Enemigo");

        GameObject masCercano = null;

        // Encontramos el EnemigoControlador.stats mas cercano
        if (enemigosEnRango.Length >= 1)
        {
            for (int i = 0; i < enemigosEnRango.Length; i++)
            {
                // Comprobamos si el EnemigoControlador.stats vuela
                if (enemigosEnRango[i].GetComponent<ControladorEntidad>().stats.vuela)
                {
                    // Si vuela y soy una torreta antiaerea
                    if (antiaerea)
                    {
                        // Si el EnemigoControlador.stats es visible esta marcado le esta disparndo una Mohawk
                        if (!enemigosEnRango[i].GetComponent<ControladorEntidad>().stats.invisibilidad || torretaBasica.visual.nombre.Equals("Mohawk") || enemigosEnRango[i].GetComponent<ControladorEntidad>().marcado)
                        {
                            // Si aun no ha encontrado ningun EnemigoControlador.stats
                            if (masCercano == null)
                            {
                                // Comprueba que tenga vision del EnemigoControlador.stats
                                if (ComprobarVision(enemigosEnRango[i]))
                                {
                                    masCercano = enemigosEnRango[i];
                                }
                            }
                            // Si ya tiene un EnemigoControlador.stats asignado
                            else if (masCercano != null)
                            {
                                // Si la distancia del actual es menor que la asignada, se asigna el actual como masCercano
                                if (Vector3.Distance(parteQueRota.position, masCercano.transform.position) > Vector3.Distance(parteQueRota.position, enemigosEnRango[i].transform.position))
                                {
                                    //Comprobamos que no este marcado
                                    if (masCercano.GetComponent<ControladorEntidad>().marcado == false)
                                    {
                                        // Comprueba que tenga vision del EnemigoControlador.stats
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
                // Si el EnemigoControlador.stats no vuela
                else
                {
                    // Si el EnemigoControlador.stats no es subterraneo
                    if (!enemigosEnRango[i].GetComponent<ControladorEntidad>().stats.subterraneo)
                    {
                        //Si el EnemigoControlador.stats es visible esta marcado le esta disparndo una Mohawk
                        if (!enemigosEnRango[i].GetComponent<ControladorEntidad>().stats.invisibilidad || torretaBasica.visual.nombre.Equals("Mohawk") || enemigosEnRango[i].GetComponent<ControladorEntidad>().marcado)
                            // Si aun no ha encontrado ningun EnemigoControlador.stats
                            if (masCercano == null)
                            {
                                // Comprueba que tenga vision del EnemigoControlador.stats
                                if (ComprobarVision(enemigosEnRango[i]))
                                {

                                    masCercano = enemigosEnRango[i];
                                    /*
                                    if (ComprobarAngulo(enemigosEnRango[i]))
                                    {
                                        masCercano = enemigosEnRango[i];
                                    }
                                    */
                                }
                            }
                            // Si ya tiene un EnemigoControlador.stats asignado
                            else if (masCercano != null)
                            {
                                // Si la distancia del actual es menor que la asignada, se asigna el actual como masCercano
                                if (Vector3.Distance(parteQueRota.position, masCercano.transform.position) > Vector3.Distance(parteQueRota.position, enemigosEnRango[i].transform.position))
                                {
                                    //Comprobamos que no este marcado
                                    if (masCercano.GetComponent<ControladorEntidad>().marcado == false)
                                    {
                                        // Comprueba que tenga vision del EnemigoControlador.stats
                                        if (ComprobarVision(enemigosEnRango[i]))
                                        {
                                            masCercano = enemigosEnRango[i];
                                            /*
                                            if (ComprobarAngulo(enemigosEnRango[i]))
                                            {
                                                masCercano = enemigosEnRango[i];
                                            }
                                            */
                                        }
                                    }
                                }
                            }
                    }
                }
            }
        }

        // Comprueba que existe un EnemigoControlador.stats visible
        if (masCercano != null)
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

    // Comprueba que el EnemigoControlador.stats apuntado no tenga ninguna pared entre la torreta y el
    bool ComprobarVision(GameObject objetivo)
    {
        // Comprobamos que no tenga terreno entre la torreta y el EnemigoControlador.stats
        RaycastHit hit;
        // no existe un collider entre el EnemigoControlador.stats y la torreta
        Physics.Raycast(parteQueRota.position, Vector3.Normalize(objetivo.transform.position - parteQueRota.position), out hit, Vector3.Distance(parteQueRota.position, objetivo.transform.position), LayerMask.GetMask("Terreno"));

        if (hit.collider == null)
        {
            return true;
        }
        return false;
    }

    bool ComprobarAngulo(GameObject objetivo)
    {
        //en caso de ser 0 es decir 360
        if (anguloDisparo.eulerAngles.y != 0)
        {
            //Calculo cual es el vector necesario para apuntar al EnemigoControlador.stats
            Vector3 vistaRelativa = objetivo.transform.position - parteQueRota.position;

            //guardo el quaternion que necesita para apuntar
            Quaternion VisionRotacion = Quaternion.LookRotation(vistaRelativa);

            //divido el angulo que puede apuntar entre 2 porque apunta en +0 y -0
            float rangoEnY = anguloDisparo.eulerAngles.y / 2;

            //calculo el angulo para apuntar hacia el EnemigoControlador.stats
            float rotacionNecesaria = VisionRotacion.eulerAngles.y - transform.rotation.eulerAngles.y;

            //si es negativo lo devulevo a positivo
            if (rotacionNecesaria < 0)
            {
                rotacionNecesaria += 360;
            }

            if (rangoEnY >= rotacionNecesaria || 360 - rangoEnY <= rotacionNecesaria)
            {
                //si esta en angulo
                return true;
            }
            else
            {
                //no esta en angulo
                return false;
            }
        }
        else
        {
            return true;
        }



    }

    // Llamado desde InvocarTorretas.cs EliminarTorreta()
    public void DestruirTorreta()
    {
            // Ajusta el animador a destruir
            animator.SetBool("Destruir", true);
            // Destruye la torreta y devuelve la energia al jugador
            personaje.Energia += energiaEnUso;
            Destroy(gameObject, 0.5f);
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
        // Comprobamos si puede reducir da�o
        if (torretaBasica.reduceDanyo.reducirDanyo)
        {
            // Comprobamos desde donde golpea y si reduce el da�o
            if (ataque.direccion > 0 && torretaBasica.reduceDanyo.frente)
            {
                ataque.fuerza = ataque.fuerza * torretaBasica.reduceDanyo.reduccion;
            }
            if (ataque.direccion < 0 && torretaBasica.reduceDanyo.espalda)
            {
                ataque.fuerza = ataque.fuerza * torretaBasica.reduceDanyo.reduccion;
            }
            if (ataque.direccion == 0 && torretaBasica.reduceDanyo.lados)
            {
                ataque.fuerza = ataque.fuerza * torretaBasica.reduceDanyo.reduccion;
            }
        }
        // Se reinicia el timer de regeneracion del escudo
        actualTimerEscudo = timerEscudo;
        // El escudo reduce el ataque 
        if (escudoActual > 0)
        {
            if (escudoActual < ataque.fuerza)
            {
                float aux = ataque.fuerza - escudoActual;
                escudoActual = 0;
                vidaActual -= aux;
                ParticleSystem ps = explosionElectrica.GetComponentInChildren<ParticleSystem>();
                Instantiate(ps, transform.position, transform.rotation);
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
            ParticleSystem ps = explosionElectrica.GetComponentInChildren<ParticleSystem>();
            Instantiate(ps, transform.position, transform.rotation);
        }

    }

    public void InhabilitarTorreta()
    {
        torretaInhabilitada = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (enemigoApuntando != null)
        {
            Gizmos.DrawRay(parteQueRota.position, Vector3.Normalize(enemigoApuntando.transform.position - parteQueRota.position) * Vector3.Distance(parteQueRota.position, enemigoApuntando.transform.position));
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(parteQueRota.position, distanciaDisparo);
    }

    public GameObject GetEnemigoApuntado()
    {
        return enemigoApuntando;
    }
}