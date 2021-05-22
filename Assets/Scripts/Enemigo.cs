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
    // FEATURES ADDED: Los Enemigo tienen unas estadisticas que heredan de su tipo y siguen una ruta establecida por su spawner, si algo aparece en su radio de vision van a por el. Tambien el recibir da�o por disparos. Cuando los enemigos disparan a un objetivo se quedan quietos, ademas los que son subterraneos salen del suelo(vuelven al subsuelo al moverse). Los enemigos buscan al jugador o a las torretas si "pueden". Variables de ataque temporal y final
    // ---------------------------------------------------

    private Base base1;
    private Base base2;
    private Base base3;
    public static Transform final;
    public NavMeshAgent agente;
    private Transform objetivo;

    public float vidaActual;

    public EnemigoBasico enemigo;
    public bool marcado = false;
    public bool invisibilidad;
    public bool subterraneo;
    public bool vuela;
    private int ataqueTemporal;

    private List<GameObject> tanques = new List<GameObject>();

    //private LineRenderer linea;
    //private List<Vector3> puntos;

    void Start()
    {

        agente = GetComponent<NavMeshAgent>();
        agente.speed = enemigo.velocidad;
        //linea = GetComponent<LineRenderer>();

        // Pone la vida al maximo
        enemigo.vidaActual = enemigo.vidaMaxima;
        vidaActual = enemigo.vidaMaxima;

        invisibilidad = enemigo.invisibilidad;
        subterraneo = enemigo.subterraneo;
        vuela = enemigo.vuela;
        ataqueTemporal = 0;

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
        enemigo.ataqueFinal = enemigo.ataque + ataqueTemporal;
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
            //linea.positionCount = agente.path.corners.Length;
            //linea.SetPositions(agente.path.corners);
            //linea.enabled = true;
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

        List<GameObject> objetivosEnRango = new List<GameObject>();

        // Recogemos todos los objetivos de la zona
        if (enemigo.atacaTorretas)
        {
            GameObject[] torretas = GameObject.FindGameObjectsWithTag("Torreta");
            for (int i = 0; i < torretas.Length; i++)
            {
                objetivosEnRango.Add(torretas[i]);
            }
        }

        if (enemigo.atacaJugador)
        {
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < player.Length; i++)
            {
                objetivosEnRango.Add(player[i]);
            }
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
        enemigo.vidaActual = vidaActual;
    }

    
    public void RecibirBuff(GameObject tanque)
    {
        // Tenemos una lista de tanques, para saber si un tanque ya a buffado a un enemigo, en caso de que sea asi no se le aplica el buff de nuevo, en caso de no estar buffado aumentamos su ataque temporal.
        for(int i = 0; i < tanques.Count; i++)
        {
            if(tanques[i] == tanque)
            {
                return;
            }
        }
        tanques.Add(tanque);
        ataqueTemporal += 2;
        return;
    }
    public void EliminarBuff(GameObject tanque)
    {
        // En caso de haber buffado al enemigo le quitamos el buff si se va de rango
        for (int i = 0; i < tanques.Count; i++)
        {
            if (tanques[i] == tanque)
            {
                tanques.Remove(tanque);
                ataqueTemporal -= 2;
                return;
            }
        }
    }
}
