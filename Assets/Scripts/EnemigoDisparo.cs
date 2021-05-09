// ---------------------------------------------------
// NAME: EnemigoDisparo.cs
// STATUS: WIP
// GAMEOBJECT: Enemigo
// DESCRIPTION: En este script se determinan los objetivos que busca un enemigo capaz de disparar
//
// AUTHOR: Pau
// FEATURES ADDED: Primera versi�n del codigo(dronAtaque): A�adidas las estadisticas, la seleccion del objetivo a disparar y la funcion de disparar
// 
// AUTHOR: Jorge Grau
// FEATURES ADDED: Todo el codigo actualizado para que funcione para todos los enemigos independientemente de su tipo, el codigo ahora usa las variables del SO de enemigo y busca objetivos de una forma m�s eficiente y limpia. A�adida la comprobaci�n de objetiivo invisible.
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: sonidos
// ---------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemigoDisparo : MonoBehaviour
{
    public EnemigoBasico enemigoBasico;

    private Enemigo enemigo;

    private float timerDisparo;

    public GameObject explosion;

    public float velocidadDeRotacion;

    [Header("Partes")]
    private GameObject objetivoADisparar;
    public Transform parteQueRota;
    public GameObject balaObjeto;
    public GameObject spawnerBalas;
    AudioHandler audioHandler;

    // Start is called before the first frame update
    void Start()
    {
        enemigo = gameObject.GetComponent<Enemigo>();
        timerDisparo = 0;
        velocidadDeRotacion = enemigoBasico.velocidadDeRotacion;

        audioHandler = gameObject.GetComponent<AudioHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemigo.vidaActual <= 0)
        {
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        //busca al objetivo mas cercano
        objetivoADisparar = BuscarObjetivo();
        //tiempo para la velocidad de ataque
        timerDisparo += Time.deltaTime;
        //si apunta a alguien
        if (objetivoADisparar != null)
        {
            // rotar en direccion al objetivo apuntado
            Vector3 dir = parteQueRota.position - objetivoADisparar.transform.position;
            Quaternion VisionRotacion = Quaternion.LookRotation(dir);
            //rotacion suave
            Vector3 rotacion = Quaternion.Lerp(parteQueRota.rotation, VisionRotacion, Time.deltaTime * velocidadDeRotacion).eulerAngles;
            parteQueRota.rotation = Quaternion.Euler(rotacion.x, rotacion.y, rotacion.z);

            //si ha pasado el tiempo de recarga
            if (timerDisparo >= enemigoBasico.velocidadDisparo)
            {
                timerDisparo = 0;

                // Sonido de disparo
                audioHandler.PlaySound(0, false);
                //disparar
                Ataque ataqueObjeto = ScriptableObject.CreateInstance<Ataque>();

                ataqueObjeto.fuerza = enemigoBasico.ataque;
                ataqueObjeto.tipo = Ataque.Tipo.laser;
                ataqueObjeto.origen = gameObject;

                Bala bala = Instantiate(balaObjeto, spawnerBalas.transform.position, spawnerBalas.transform.rotation).GetComponent<Bala>();

                bala.ataque = ataqueObjeto;
            }

        }
    }
    //Funcion de busqueda de objetivo
    GameObject BuscarObjetivo()
    {
        // Recogemos todos los objetivos de la zona
        GameObject[] torretas = GameObject.FindGameObjectsWithTag("Torreta");
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] bases = GameObject.FindGameObjectsWithTag("Base");
        List<GameObject> objetivosEnRango = new List<GameObject>();

        objetivosEnRango.AddRange(torretas);
        objetivosEnRango.AddRange(player);
        objetivosEnRango.AddRange(bases);

        GameObject masCercano = null;

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
                                masCercano = objetivosEnRango[i];
                            }
                        }
                        // Si ya tiene un objetivo asignado
                        else if (masCercano != null)
                        {
                            // Si la distancia del actual es menor que la asignada, se asigna el actual como masCercano
                            if (Vector3.Distance(parteQueRota.position, masCercano.transform.position) > Vector3.Distance(parteQueRota.position, objetivosEnRango[i].transform.position))
                            {
                                // Comprueba que tenga vision del objetivo
                                if (ComprobarVision(objetivosEnRango[i]))
                                {
                                    masCercano = objetivosEnRango[i];
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
                            masCercano = objetivosEnRango[i];
                        }
                    }
                    // Si ya tiene un objetivo asignado
                    else if (masCercano != null)
                    {
                        // Si la distancia del actual es menor que la asignada, se asigna el actual como masCercano
                        if (Vector3.Distance(parteQueRota.position, masCercano.transform.position) > Vector3.Distance(parteQueRota.position, objetivosEnRango[i].transform.position))
                        {
                            // Comprueba que tenga vision del objetivo
                            if (ComprobarVision(objetivosEnRango[i]))
                            {
                                masCercano = objetivosEnRango[i];
                            }

                        }
                    }
                }
            }
        }
        // Comprueba que existe un objetivo visible
        if (masCercano != null)
        {
            // Ahora que tenemos el objetivo mas cercano devolvemos el GameObject si esta dentro del rango de disparo
            if (Vector3.Distance(parteQueRota.position, masCercano.transform.position) < enemigoBasico.rangoDisparo)
            {
                return masCercano;
            }
        }
        // Si no, devuelve un null
        return null;
    }

    bool ComprobarVision(GameObject objetivo)
    {
        // Comprobamos que no hayan obstaculos desde nuestra posici�n a la del objetivo
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
