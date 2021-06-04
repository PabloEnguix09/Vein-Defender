using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: EnemigoAtaque.cs
// STATUS: WIP
// GAMEOBJECT: Enemigo
// DESCRIPTION: En este script se determinan los objetivos que busca un enemigo capaz de disparar
//
// AUTHOR: Pau
// FEATURES ADDED: Primera version del codigo(dronAtaque): estadisticas, la seleccion del objetivo a disparar y la funcion de disparar
// 
// AUTHOR: Jorge Grau
// FEATURES ADDED: Todo el codigo actualizado para que funcione para todos los enemigos independientemente de su tipo, el codigo ahora usa las variables del SO de enemigo y busca objetivos de una forma m�s eficiente y limpia. A�adida la comprobaci�n de objetiivo invisible. A�adido el da�o y el rango de la explosion en la creaci�n del objeto ataque. Los enemigos atacan al jugador o a las torretas si "pueden". Sabemos la direcci�n del ataque.
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: optimizado en el nuevo script, unificacion con el bomba, tanque y todos los enemigos
//
// AUTHOR: Adrian Maldonado
// FEATURES ADDED: Comprobacion de tener una torreta delante
// ---------------------------------------------------

public class EnemigoAtaque : MonoBehaviour
{
    #region Variables
    EnemigoControlador controlador;
    // Cooldown de disparo
    float timerDisparo = 0;
    // Fuerza de ataque total
    [HideInInspector]
    public float fuerza;
    List<GameObject> potenciadores;
    private Vector3 objetivoADisparar;

    [Header("Con Disparo")]
    public Transform parteQueRota;
    public GameObject balaObjeto;
    public GameObject spawnerBalas;
    #endregion

    private void Start()
    {
        controlador = GetComponent<EnemigoControlador>();
        // Fuerza base
        fuerza = controlador.stats.ataque;
    }
    // Update is called once per frame
    void Update()
    {

        if(controlador.stats.tipoAtaque == EnemigoBasico.Tipo.potenciador)
        {
            // Recogemos todos los objetivos de la zona
            GameObject[] enemigo = GameObject.FindGameObjectsWithTag("Enemigo");

            for (int i = 0; i < enemigo.Length; i++)
            {
                // Su tiene un enemigo en rango le da un buff
                if (Vector3.Distance(this.transform.position, enemigo[i].transform.position) < controlador.stats.rangoDisparo)
                {
                    enemigo[i].GetComponent<EnemigoControlador>().Potenciado(true, this.gameObject);
                }
                // Su tiene un enemigo fuera de rango se lo quita
                else if (Vector3.Distance(this.transform.position, enemigo[i].transform.position) > controlador.stats.rangoDisparo)
                {
                    enemigo[i].GetComponent<EnemigoControlador>().Potenciado(false, this.gameObject);
                }
            }
        }
        // Si el enemigo dispara
        if(controlador.stats.tipoAtaque == EnemigoBasico.Tipo.disparo)
        {

            //busca al objetivo mas cercano
            objetivoADisparar = BuscarObjetivo();
            //tiempo para la velocidad de ataque
            timerDisparo += Time.deltaTime;
            //si apunta a alguien
            if (objetivoADisparar != Vector3.zero)
            {
                // Tiene una torreta en rango
                controlador.TorretaEnRango();
                // rotar en direccion al objetivo apuntado
                Vector3 dir = objetivoADisparar - parteQueRota.position;
                Quaternion VisionRotacion = Quaternion.LookRotation(dir);
                //rotacion suave
                Vector3 rotacion = Quaternion.Lerp(parteQueRota.rotation, VisionRotacion, Time.deltaTime * controlador.stats.velocidadDeRotacion).eulerAngles;
                parteQueRota.rotation = Quaternion.Euler(rotacion.x, rotacion.y, rotacion.z);

                //si ha pasado el tiempo de recarga
                if (timerDisparo >= controlador.stats.velocidadDisparo)
                {
                    timerDisparo = 0;
                    Disparar();
                }
            }
            else
            {
                // Si no apunta a nadie vuelve a la animacion de caminar
                controlador.Caminar();
            }
        }
    }

    //Funcion de busqueda de objetivo
    Vector3 BuscarObjetivo()
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
                    } // if
                } // if
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
                    } // else if
                } // else
            } // for
        } // if
        // Comprueba que existe un objetivo visible
        if (masCercano != null)
        {
            // Comprueba que delante tenga una torreta
            RaycastHit hit;
            Physics.Raycast(parteQueRota.position, parteQueRota.forward, out hit, controlador.stats.rango, LayerMask.GetMask("Torreta"));
            if (hit.collider != null)
            {
                //En caso de tener delante una torreta
                return hit.point;
            }

            //Debug.Log(masCercano);

            // Ahora que tenemos el objetivo mas cercano devolvemos el GameObject si esta dentro del rango de disparo
            if (Vector3.Distance(parteQueRota.position, masCercano) < controlador.stats.rangoDisparo)
            {
                return masCercano;
            }
        } // if
        // Si no, devuelve un null
        return Vector3.zero;
    }

    public void Disparar()
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
        Vector3 direccion = transform.position - objetivoADisparar;        ataqueObjeto.direccion = Vector3.Dot(transform.forward, direccion);
        // Instancia el disparo
        Bala bala = Instantiate(balaObjeto, spawnerBalas.transform.position, spawnerBalas.transform.rotation).GetComponent<Bala>();
        bala.ataque = ataqueObjeto;
    }

    // Para el bomba cuando toca a un objetivo explota
    private void OnTriggerEnter(Collider other)
    {
        if(controlador.stats.tipoAtaque == EnemigoBasico.Tipo.bomba)
        {
            // Cuando choca contra una base, torreta o personaje se autodestruye
            if (other.gameObject.GetComponent<Base>() != null || other.gameObject.GetComponent<Torreta>() != null || other.gameObject.GetComponent<Personaje>())
            {
                Explotar();
            }
        }
    }
    // Para el bomba principalmente
    public void Explotar()
    {
        // Impacto de explosion y objetivos afectados
        Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, controlador.stats.rangoExplosion);

        Ataque ataqueObjeto = ScriptableObject.CreateInstance<Ataque>();

        ataqueObjeto.fuerza = fuerza;
        ataqueObjeto.tipo = Ataque.Tipo.balas;
        ataqueObjeto.origen = gameObject;

        // Hace un ataque contra todos los objetivos dentro del rango
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Base"))
            {
                Base estructura = colliders[i].gameObject.GetComponent<Base>();
                estructura.RecibirAtaque(ataqueObjeto);
            }

            else if (colliders[i].CompareTag("Torreta"))
            {
                Torreta estructura = colliders[i].gameObject.GetComponent<Torreta>();

                estructura.RecibirAtaque(ataqueObjeto);
            }

            else if (colliders[i].CompareTag("Enemigo"))
            {
                EnemigoControlador otroEnemigo = colliders[i].gameObject.GetComponent<EnemigoControlador>();
                otroEnemigo.RecibeAtaque(ataqueObjeto);
            }

            else if (colliders[i].CompareTag("Player"))
            {
                Personaje personaje = colliders[i].gameObject.GetComponent<Personaje>();
                personaje.RecibirAtaque(ataqueObjeto);
            }
        }

        controlador.Muerte();
    }
    // Devuelve si puedes ver al objetivo
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
    // Controla el ataque cuando esta siendo potenciado
    public void Potenciado(bool estado, GameObject potenciador)
    {
        // Poner un potenciador nuevo
        if(estado)
        {
            potenciadores.Add(potenciador);
            // Ponemos la fuerza a base
            fuerza = controlador.stats.ataque;
            // Sumamos cada uno de los potenciadores
            for (int i = 0; i < potenciadores.Count; i++)
            {
                fuerza += potenciadores[i].GetComponent<EnemigoControlador>().stats.ataque;
            }
        }
        // Quita un ptenciador existente en la lista
        if (!estado)
        {
            if (potenciadores.Contains(potenciador))
            {
                potenciadores.Remove(potenciador);
                // Ponemos la fuerza a base
                fuerza = controlador.stats.ataque;
                // Sumamos cada uno de los potenciadores
                for (int i = 0; i < potenciadores.Count; i++)
                {
                    fuerza += potenciadores[i].GetComponent<EnemigoControlador>().stats.ataque;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Muestra el rango de explosion para los enemigos explosivos
        if(controlador.stats.tipoAtaque == EnemigoBasico.Tipo.bomba)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, controlador.stats.rangoExplosion);
        }
        // Muestra el rango de disparo para los enemigos de rango
        if(controlador.stats.tipoAtaque == EnemigoBasico.Tipo.disparo ||
           controlador.stats.tipoAtaque == EnemigoBasico.Tipo.potenciador)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, controlador.stats.rangoDisparo);
        }

    }
}