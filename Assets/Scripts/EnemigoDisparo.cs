// ---------------------------------------------------
// NAME: EnemigoDisparo.cs
// STATUS: WIP
// GAMEOBJECT: Enemigo
// DESCRIPTION: En este script se determinan los objetivos que busca un enemigo capaz de disparar
//
// AUTHOR: Pau
// FEATURES ADDED: Primera versión del codigo(dronAtaque): Añadidas las estadisticas, la seleccion del objetivo a disparar y la funcion de disparar
// 
// AUTHOR: Jorge Grau
// FEATURES ADDED: Todo el codigo actualizado para que funcione para todos los enemigos independientemente de su tipo, el codigo ahora usa las variables del SO de enemigo y busca objetivos de una forma más eficiente y limpia. Añadida la comprobación de objetiivo invisible. Añadido el daño y el rango de la explosion en la creación del objeto ataque. Los enemigos atacan al jugador o a las torretas si "pueden". Sabemos la dirección del ataque.
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: sonidos, animaciones
//
// AUTHOR: Adrian Maldonado
// FEATURES ADDED: Comprobacion de tener una torreta delante
// ---------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoDisparo : MonoBehaviour
{
    public EnemigoBasico enemigoBasico;

    private float timerDisparo;

    public GameObject explosion;

    public float velocidadDeRotacion;

    [Header("Partes")]
    private Vector3 objetivoADisparar;
    public Transform parteQueRota;
    public GameObject balaObjeto;
    public GameObject spawnerBalas;
    AudioHandler audioHandler;
    AnimEnemigo animEnemigo;

    // Start is called before the first frame update
    void Start()
    {
        timerDisparo = 0;
        velocidadDeRotacion = enemigoBasico.velocidadDeRotacion;

        audioHandler = gameObject.GetComponent<AudioHandler>();
        animEnemigo = GetComponent<AnimEnemigo>();
    }

    // Update is called once per frame
    void Update()
    {

        //busca al objetivo mas cercano
        objetivoADisparar = BuscarObjetivo();
        //tiempo para la velocidad de ataque
        timerDisparo += Time.deltaTime;
        //si apunta a alguien
        if (objetivoADisparar != Vector3.zero)
        {
            // Animacion de bloqueo para disparar
            animEnemigo.Bloqueado(true);
            // rotar en direccion al objetivo apuntado
            Vector3 dir = objetivoADisparar - parteQueRota.position;
            Quaternion VisionRotacion = Quaternion.LookRotation(dir);
            //rotacion suave
            Vector3 rotacion = Quaternion.Lerp(parteQueRota.rotation, VisionRotacion, Time.deltaTime * velocidadDeRotacion).eulerAngles;
            parteQueRota.rotation = Quaternion.Euler(rotacion.x, rotacion.y, rotacion.z);

            //si ha pasado el tiempo de recarga
            if (timerDisparo >= enemigoBasico.velocidadDisparo)
            {
                timerDisparo = 0;

                Disparar();
            }
        } else
        {
            // Si no apunta a nadie vuelve a la animacion de caminar
            animEnemigo.Bloqueado(false);
        }
    }
    //Funcion de busqueda de objetivo
    Vector3 BuscarObjetivo()
    {
        List<GameObject> objetivosEnRango = new List<GameObject>();
        // Recogemos todos los objetivos de la zona
        if (enemigoBasico.atacaTorretas)
        {
            GameObject[] torretas = GameObject.FindGameObjectsWithTag("Torreta");
            objetivosEnRango.AddRange(torretas);
        }

        if (enemigoBasico.atacaJugador)
        {
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
            objetivosEnRango.AddRange(player);
        }

        GameObject[] bases = GameObject.FindGameObjectsWithTag("Base");
        objetivosEnRango.AddRange(bases);

        Vector3 masCercano = Vector3.zero;

        // Encontramos el objetivo mas cercano
        if (objetivosEnRango.Count >= 1)
        {
            for (int i = 0; i < objetivosEnRango.Count; i++)
            {

                // Si el enemigo es visible y es una torreta
                if (objetivosEnRango[i].TryGetComponent(out Torreta torreta))
                {
                    if (!torreta.invisibilidad)
                    {
                        // Si aun no ha encontrado ningun objetivo
                        if (masCercano == null)
                        {
                            // Comprueba que tenga vision del objetivo
                            if (ComprobarVision(objetivosEnRango[i]))
                            {
                                masCercano = objetivosEnRango[i].transform.position;
                            }
                        }
                        // Si ya tiene un objetivo asignado
                        else if (masCercano != null)
                        {
                            // Si la distancia del actual es menor que la asignada, se asigna el actual como masCercano
                            if (Vector3.Distance(parteQueRota.position, masCercano) > Vector3.Distance(parteQueRota.position, objetivosEnRango[i].transform.position))
                            {
                                // Comprueba que tenga vision del objetivo
                                if (ComprobarVision(objetivosEnRango[i]))
                                {
                                    masCercano = objetivosEnRango[i].transform.position;
                                }

                            }
                        }
                    }
                }
                else
                {
                    // Si aun no ha encontrado ningun objetivo
                    if (masCercano == null)
                    {
                        // Comprueba que tenga vision del objetivo
                        if (ComprobarVision(objetivosEnRango[i]))
                        {
                            masCercano = objetivosEnRango[i].transform.position;
                        }
                    }
                    // Si ya tiene un objetivo asignado
                    else if (masCercano != null)
                    {
                        // Si la distancia del actual es menor que la asignada, se asigna el actual como masCercano
                        if (Vector3.Distance(parteQueRota.position, masCercano) > Vector3.Distance(parteQueRota.position, objetivosEnRango[i].transform.position))
                        {
                            // Comprueba que tenga vision del objetivo
                            if (ComprobarVision(objetivosEnRango[i]))
                            {
                                masCercano = objetivosEnRango[i].transform.position;
                            }

                        }
                    }
                }
            }
        }
        // Comprueba que existe un objetivo visible
        if (masCercano != null)
        {
            // Comprueba que delante tenga una torreta
            RaycastHit hit;
            Physics.Raycast(parteQueRota.position, parteQueRota.forward, out hit, enemigoBasico.rango, LayerMask.GetMask("Torreta"));

            if (hit.collider != null)
            {
                //En caso de tener delante una torreta
                return hit.point;

            }

            // Ahora que tenemos el objetivo mas cercano devolvemos el GameObject si esta dentro del rango de disparo
            if (Vector3.Distance(parteQueRota.position, masCercano) < enemigoBasico.rangoDisparo)
            {
                return masCercano;
            }
        }
        // Si no, devuelve un null
        return Vector3.zero;
    }

    public void Disparar()
    {
        // Animacion Disparo
        animEnemigo.Dispara();
        // Sonido de disparo
        audioHandler.PlaySound(0, false);
        //disparar
        Ataque ataqueObjeto = ScriptableObject.CreateInstance<Ataque>();

        ataqueObjeto.fuerza = enemigoBasico.ataqueFinal;
        ataqueObjeto.fuerzaExplosion = enemigoBasico.ataqueExplosion;
        ataqueObjeto.radioExplosion = enemigoBasico.rangoExplosion;
        ataqueObjeto.tipo = Ataque.Tipo.laser;
        ataqueObjeto.origen = gameObject;

        // Se busca la direccion desde donde esta atacando al objetivo
        Vector3 direccion = transform.position - objetivoADisparar;
        ataqueObjeto.direccion = Vector3.Dot(transform.forward, direccion);

        Bala bala = Instantiate(balaObjeto, spawnerBalas.transform.position, spawnerBalas.transform.rotation).GetComponent<Bala>();

        bala.ataque = ataqueObjeto;
    }

    bool ComprobarVision(GameObject objetivo)
    {
        // Comprobamos que no hayan obstaculos desde nuestra posición a la del objetivo
        RaycastHit hit;
        // no existe un collider entre nosotros y el objetivo
        Physics.Raycast(parteQueRota.position, Vector3.Normalize(objetivo.transform.position - parteQueRota.position), out hit, Vector3.Distance(parteQueRota.position, objetivo.transform.position), LayerMask.GetMask("Terreno"));

        if (hit.collider == null)
        {
            return true;
        }
        return false;
    }
}
