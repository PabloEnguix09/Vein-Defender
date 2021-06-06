using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: DisparadorSO.cs
// STATUS: DONE
// GAMEOBJECT: ControladorEntidad
// DESCRIPTION: En este script se determinan los objetivos que busca un enemigo capaz de disparar
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: Traspaso del código y cambios básicos en la implementación
// ---------------------------------------------------

[CreateAssetMenu(fileName ="DisparadorSO", menuName = "Componentes/DisparadorSO")]
public class DisparadorSO : ScriptableObject
{
    public Vector3 BuscarObjetivo(ControladorEntidad controlador)
    {
        List<GameObject> objetivosEnRango = new List<GameObject>();
        // Recogemos todas torretas de la zona
        if (controlador.stats.atacaTorretas)
        {
            GameObject[] torretas = GameObject.FindGameObjectsWithTag("Torreta");
            objetivosEnRango.AddRange(torretas);
        }
        // Pone al jugador en la lista de posibles objetivos
        if (controlador.stats.atacaJugador)
        {
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
            objetivosEnRango.AddRange(player);
        }
        // Pone las bases 
        GameObject[] bases = GameObject.FindGameObjectsWithTag("Base");
        objetivosEnRango.AddRange(bases);

        Vector3 masCercano = Vector3.zero;

        // Encontramos el objetivo mas cercano
        if (objetivosEnRango.Count >= 1)
        {
            //Debug.Log("Objetivos en rango " + objetivosEnRango.Count.ToString());
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
                            if (ComprobarVision(objetivosEnRango[i], controlador.parteQueRota))
                            {
                                masCercano = objetivosEnRango[i].transform.position;
                            }
                        }
                        // Si ya tiene un objetivo asignado
                        else if (masCercano != null)
                        {
                            // Si la distancia del actual es menor que la asignada, se asigna el actual como masCercano
                            if (Vector3.Distance(controlador.parteQueRota.position, masCercano) > Vector3.Distance(controlador.parteQueRota.position, objetivosEnRango[i].transform.position))
                            {
                                // Comprueba que tenga vision del objetivo
                                if (ComprobarVision(objetivosEnRango[i], controlador.parteQueRota))
                                {
                                    masCercano = objetivosEnRango[i].transform.position;
                                }

                            }
                        }
                    } // if
                } // if
                else
                {
                    // Si aun no ha encontrado ningun objetivo
                    if (masCercano == null)
                    {
                        // Comprueba que tenga vision del objetivo
                        if (ComprobarVision(objetivosEnRango[i], controlador.parteQueRota))
                        {
                            masCercano = objetivosEnRango[i].transform.position;
                        }
                    }
                    // Si ya tiene un objetivo asignado
                    else if (masCercano != null)
                    {
                        // Si la distancia del actual es menor que la asignada, se asigna el actual como masCercano
                        if (Vector3.Distance(controlador.parteQueRota.position, masCercano) > Vector3.Distance(controlador.parteQueRota.position, objetivosEnRango[i].transform.position))
                        {
                            // Comprueba que tenga vision del objetivo
                            if (ComprobarVision(objetivosEnRango[i], controlador.parteQueRota))
                            {
                                masCercano = objetivosEnRango[i].transform.position;
                            }

                        }
                    } // else if
                } // else
            } // for
        } // if
        // Comprueba que existe un objetivo visible
        if (masCercano != null)
        {
            // Comprueba que delante tenga una torreta
            RaycastHit hit;
            Physics.Raycast(controlador.parteQueRota.position, controlador.parteQueRota.forward, out hit, controlador.stats.rangoDeteccion, LayerMask.GetMask("Torreta"));
            if (hit.collider != null)
            {
                //En caso de tener delante una torreta
                return hit.point;
            }

            //Debug.Log(masCercano);

            // Ahora que tenemos el objetivo mas cercano devolvemos el GameObject si esta dentro del rango de disparo
            if (Vector3.Distance(controlador.parteQueRota.position, masCercano) < controlador.stats.rangoDisparo)
            {
                return masCercano;
            }
        } // if
        // Si no, devuelve un null
        return Vector3.zero;
    }

    bool ComprobarVision(GameObject objetivo, Transform parteQueRota)
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

    public void Disparar(ControladorEntidad controlador, float fuerza, GameObject gameObject, Vector3 objetivo)
    {
        controlador.Disparar();

        Ataque ataqueObjeto = ScriptableObject.CreateInstance<Ataque>();

        // Parametros del ataque
        ataqueObjeto.fuerza = fuerza;
        ataqueObjeto.fuerzaExplosion = controlador.stats.ataqueExplosion;
        ataqueObjeto.radioExplosion = controlador.stats.rangoExplosion;
        ataqueObjeto.tipo = Ataque.Tipo.laser;
        ataqueObjeto.origen = gameObject;

        // Se busca la direccion desde donde esta atacando al objetivo
        Vector3 direccion = gameObject.transform.position - objetivo;        ataqueObjeto.direccion = Vector3.Dot(gameObject.transform.forward, direccion);
        // Instancia el disparo
        Bala bala = Instantiate(controlador.balaObjeto, controlador.spawnerBalas.transform.position, controlador.spawnerBalas.transform.rotation).GetComponent<Bala>();
        bala.ataque = ataqueObjeto;
    }
}
