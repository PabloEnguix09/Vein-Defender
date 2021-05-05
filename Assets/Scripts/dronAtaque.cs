// ---------------------------------------------------
// NAME: dronAtaque.cs
// STATUS: WIP
// GAMEOBJECT: Enemigo
// DESCRIPTION: Aqui se reunen las capacidades especiales del enemigo bomba y sus estadisticas
//
// AUTHOR: Pau
// FEATURES ADDED: Añadidas las estadisticas, la seleccion del objetivo a disparar y la funcion de disparar
// ---------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dronAtaque : MonoBehaviour
{
    public float ataque;
    public float vidaMaxima;
    public float velocidad;

    public float rango;
    public float rangoDisparo;

    private float timerDisparo;
    private GameObject objetivoApuntado;
    public GameObject spawnerBalas;
    public Transform parteQueRota;
    public GameObject explosion;
    public GameObject balaObjeto;
    public EnemigoBasico enemigoBasico;
    private Enemigo enemigo;
    // Start is called before the first frame update
    void Start()
    {
        enemigo = gameObject.GetComponent<Enemigo>();
        timerDisparo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemigo.vidaActual <= 0)
        {
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        //busca al enemigo mas cercano
        objetivoApuntado = buscarObjetivo();

        //tiempo para la velocidad de ataque
        timerDisparo += Time.deltaTime;

        //si apunta a alguien
        if (objetivoApuntado != null)
        {
           
            //rota la torreta en direccion al enemigo apuntado
            Vector3 dir = parteQueRota.position - objetivoApuntado.transform.position+new Vector3(0,-1,0);
            Quaternion VisionRotacion = Quaternion.LookRotation(dir);
            //rotacion suave
            Vector3 rotacion = Quaternion.Lerp(parteQueRota.rotation, VisionRotacion, Time.deltaTime * 20).eulerAngles;
            parteQueRota.rotation = Quaternion.Euler(rotacion.x, rotacion.y, rotacion.z);
           
            //si ha pasado el tiempo de recarga
            if (timerDisparo >= enemigoBasico.velocidadDisparo)
            {
                timerDisparo = 0;

                //disparar
                Ataque ataqueObjeto = ScriptableObject.CreateInstance<Ataque>();

                ataqueObjeto.fuerza = ataque;
                ataqueObjeto.tipo = Ataque.Tipo.laser;
                ataqueObjeto.origen = gameObject;

                Bala bala = Instantiate(balaObjeto, spawnerBalas.transform.position, spawnerBalas.transform.rotation).GetComponent<Bala>();

                bala.ataque = ataqueObjeto;
            }
        }

    }

    GameObject buscarObjetivo()
    {
        GameObject masCercano = null;

        // Encontramos el enemigo mas cercano
        
        Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, enemigoBasico.rango);

        // Inflinge da�o a todos los objetivos dentro del rango
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Base")||colliders[i].CompareTag("Torreta")||colliders[i].CompareTag("Player"))
            {

                // Si aun no ha encontrado ninguna torreta
                if (masCercano == null)
                {

                    // Comprueba que tenga vision del enemigo
                    if (ComprobarVision(colliders[i].gameObject))
                    {

                        masCercano = colliders[i].gameObject;
                    }
                }
                // Si ya tiene un enemigo asignado
                else if (masCercano != null)
                {

                    // Si la distancia del actual es menor que la asignada, se asigna el actual como masCercano
                    if (Vector3.Distance(parteQueRota.position, masCercano.transform.position) > Vector3.Distance(parteQueRota.position, colliders[i].gameObject.transform.position))
                    {

                        // Comprueba que tenga vision del enemigo
                        if (ComprobarVision(colliders[i].gameObject))
                        {
                            masCercano = colliders[i].gameObject;
                        }

                    }
                }
            }
        }
    
        // Comprueba que existe un enemigo visible
        if (masCercano != null)
        {
           ;
            // Ahora que tenemos la torreta mas cercana devolvemos el GameObject si esta dentro del rango de disparo
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
        // Comprobamos que no tenga terreno entre la torreta y el enemigo
        RaycastHit hit;
        // no existe un collider entre el enemigo y la torreta
        Physics.Raycast(parteQueRota.position, Vector3.Normalize(objetivo.transform.position - parteQueRota.position), out hit, Vector3.Distance(parteQueRota.position, objetivo.transform.position), LayerMask.GetMask("Terreno"));

        if (hit.collider == null)
        {
            return true;
        }
        return false;
    }



}

