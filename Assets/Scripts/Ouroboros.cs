// ---------------------------------------------------
// NAME: Ouroboros.cs
// STATUS: WIP
// GAMEOBJECT: Enemigo
// DESCRIPTION: Aqui se reunen las capacidades especiales del enemigo ouroboros y sus estadisticas
//
// AUTHOR: Jorge Grau
// FEATURES ADDED: 
// ---------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ouroboros : MonoBehaviour
{

    public EnemigoBasico enemigoBasico;

    private Enemigo enemigo;

    private float timerDisparo;

    [Header("Partes")]
    private GameObject enemigoApuntando;
    public Transform parteQueRota;
    private Disparo disparo;

    // Start is called before the first frame update
    void Start()
    {
        enemigo = gameObject.GetComponent<Enemigo>();
        timerDisparo = 0;
        disparo = GetComponentInChildren<Disparo>();
    }

    // Update is called once per frame
    void Update()
    {
        // Targetea a un enemigo, es invisible
        // Puede disparar a un enemigo se vuelve visible

        //busca al enemigo mas cercano
        enemigoApuntando = BuscarEnemigo();
        //tiempo para la velocidad de ataque
        timerDisparo += Time.deltaTime;

        //si apunta a alguien
        if (enemigoApuntando != null)
        {
            comprobarInvisibilidad(enemigoApuntando);
            //rota la torreta en direccion al enemigo apuntado
            Vector3 dir = parteQueRota.position - enemigoApuntando.transform.position;
            Quaternion VisionRotacion = Quaternion.LookRotation(dir);
            //rotacion suave
            // rotacion = Quaternion.Lerp(parteQueRota.rotation, VisionRotacion, Time.deltaTime * velocidadRotacion).eulerAngles;
            //parteQueRota.rotation = Quaternion.Euler(rotacion.x, rotacion.y, rotacion.z);

            //si ha pasado el tiempo de recarga
            if (timerDisparo >= enemigoBasico.velocidadDisparo)
            {
                timerDisparo = 0;
                //disparar
                disparo.Disparar(enemigoBasico.ataque, enemigoBasico.ataqueExplosion, enemigoBasico.rangoExplosion);
            }
        }
    }
    //Funcion de busqueda de enemigo
    GameObject BuscarEnemigo()
    {
        // Recogemos todos los enemigos de la zona
        GameObject[] enemigosEnRango = GameObject.FindGameObjectsWithTag("Torretas");

        GameObject masCercano = null;

        // Encontramos el enemigo mas cercano
        if (enemigosEnRango.Length >= 1)
        {
            for (int i = 0; i < enemigosEnRango.Length; i++)
            {
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
                            // Comprueba que tenga vision del enemigo
                            if (ComprobarVision(enemigosEnRango[i]))
                            {
                                masCercano = enemigosEnRango[i];
                            }

                    }
                }
            }
        }
        // Comprueba que existe un enemigo visible
        if (masCercano != null)
        {
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
    void comprobarInvisibilidad(GameObject enemigoApuntando)
    {
        if (Vector3.Distance(parteQueRota.position, enemigoApuntando.transform.position) <= enemigoBasico.rangoDisparo){
            enemigo.invisibilidad = false;
        }
        else { enemigo.invisibilidad = true; }

        Debug.Log("Distancia " + Vector3.Distance(parteQueRota.position, enemigoApuntando.transform.position).ToString());
        Debug.Log("Rango " + enemigoBasico.rangoDisparo);
        Debug.Log("Invisibilidad " + enemigo.invisibilidad.ToString());

    }
}
