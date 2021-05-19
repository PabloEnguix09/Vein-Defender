using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
public class Enemigo : MonoBehaviour
{

    // ---------------------------------------------------
    // NAME: Enemigo.cs
    // STATUS: WIP
    // GAMEOBJECT: Enemigo
    // DESCRIPTION: Este escript reune todas las capacidades basicas de un enemigo. Movimiento y estadisticas.
    //
    // AUTHOR: Jorge Grau
    // FEATURES ADDED: Los Enemigo tienen unas estadisticas que eredan de su tipo y siguen una ruta establecida por su spawner, si algo aparece en su radio de vision van a por el. Tambien el recibir da�o por disparos. Cuando los enemigos disparan a un objetivo se quedan quietos, ademas los que son invisibles pierden la invisibilidad(que es recuperada la moverse de nuevo).
    // ---------------------------------------------------

    private Base base1;
    private Base base2;
    private Base base3;
    public static Transform final;
    public NavMeshAgent agente;
    private Transform objetivo;

    public GameObject explosion;

    public float vidaActual;

    public EnemigoBasico enemigo;
    public bool marcado = false;
    public bool invisibilidad;
    public bool subterraneo;
    public bool vuela;

    private LineRenderer linea;
    private List<Vector3> puntos;

    void Start()
    {

        agente = GetComponent<NavMeshAgent>();
        agente.speed = enemigo.velocidad;
        linea = GetComponent<LineRenderer>();

        // Pone la vida al maximo
        vidaActual = enemigo.vidaMaxima;

        invisibilidad = enemigo.invisibilidad;
        subterraneo = enemigo.subterraneo;
        vuela = enemigo.vuela;


        // El enemigo busca a que base dirigirse, si todas estan destruidas va donde a aparecido
        if (base1.Salud > 0)
        {
            agente.destination = base1.transform.position;
        }

        else if (base2.Salud > 0)
        {
            agente.destination = base2.transform.position;
        }

        else if (base3.Salud > 0)
        {
            agente.destination = base3.transform.position;
        }

        else
        {
            agente.destination = final.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Busca un objetivo y se dirige hacia el
        objetivo = BuscarObjetivo();

        agente.destination = objetivo.position;
        // Si el objetivo esta en nuestro rango de disparo, nos quedamos quietos y perdemos la invisibilidad
        if (Vector3.Distance(this.transform.position, objetivo.position) <= enemigo.rangoDisparo)
        {
            if (enemigo.subterraneo)
            {
                subterraneo = false;
            }
            agente.speed = 0f;
        }
        // Si el objetivo no esta en el rango de disparo mantenemos la velocidad y la invisibilidad
        else
        {
            agente.speed = enemigo.velocidad;
            if (enemigo.subterraneo)
            {
                subterraneo = true;
            }
        }

        if(agente.hasPath)
        {
            linea.positionCount = agente.path.corners.Length;
            linea.SetPositions(agente.path.corners);
            linea.enabled = true;
        }
    }
    
    public void Marcar()
    {
        marcado = true;
    }

    public void AsignarBases(Base base1, Base base2, Base base3)
    {
        this.base1 = base1;
        this.base2 = base2;
        this.base3 = base3;

    }

    Transform BuscarObjetivo()
    {
        // Recogemos todos los objetivos de la zona
        GameObject[] torretas = GameObject.FindGameObjectsWithTag("Torreta");
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> objetivosEnRango = new List<GameObject>();

        for (int i = 0; i < torretas.Length; i++)
        {
            objetivosEnRango.Add(torretas[i]);
        }

        for (int i = 0; i < player.Length; i++)
        {
            objetivosEnRango.Add(player[i]);
        }

        for(int i = 0; i < objetivosEnRango.Count; i++)
        {
            // Si el enemigo es visible, es una torreta y esta en rango
            if (objetivosEnRango[i].TryGetComponent(out Torreta torreta))
            {
                if (!torreta.invisibilidad)
                {
                    if (Vector3.Distance(this.transform.position, objetivosEnRango[i].transform.position) <= enemigo.rango)
                    {
                        return objetivo = objetivosEnRango[i].transform;
                    }
                }

            }
            // Si es el jugador y esta en rango
            else
            {
                if (Vector3.Distance(this.transform.position, objetivosEnRango[i].transform.position) <= enemigo.rango)
                {
                    return objetivo = objetivosEnRango[i].transform;
                }
            }
        }

        if (base1.Salud > 0)
        {
            return objetivo = base1.transform;
        }

        else if (base2.Salud > 0 && base1.Salud <= 0)
        {
            return objetivo = base2.transform;
        }

        else if (base3.Salud > 0 && base1.Salud <= 0 && base2.Salud <= 0)
        {
            return objetivo = base3.transform;
        }

        else
        {
            return objetivo = final.transform;
        }

    }

    public void RecibirAtaque(Ataque ataque)
    {
        vidaActual -= ataque.fuerza;
    }
}
