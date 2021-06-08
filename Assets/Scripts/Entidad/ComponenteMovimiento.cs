using UnityEngine;
using UnityEngine.AI;

// ---------------------------------------------------
// NAME: EnemigoMovimiento.cs
// STATUS: WIP
// GAMEOBJECT: Enemigo
// DESCRIPTION: Este escript reune todas las capacidades basicas de un enemigo. Movimiento y estadisticas.
//
// AUTHOR: Jorge Grau
// FEATURES ADDED: Los Enemigo tienen unas estadisticas que heredan de su tipo y siguen una ruta establecida por su spawner, si algo aparece en su radio de vision van a por el. Tambien el recibir da�o por disparos. Cuando los enemigos disparan a un objetivo se quedan quietos, ademas los que son subterraneos salen del suelo(vuelven al subsuelo al moverse). Los enemigos buscan al jugador o a las torretas si "pueden". Variables de ataque temporal y final
//
// AUTHOR: Adrian Maldonado
// FEATURES ADDED: Comprobacion de tener una torreta delante
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: Optimizado en el nuevo Script
// ---------------------------------------------------

public class ComponenteMovimiento : MonoBehaviour
{
    #region Variables
    // Controlador enemigo
    ControladorEntidad controlador;

    private Base base1;
    private Base base2;
    private Base base3;

    public static Transform final;
    NavMeshAgent agente;

    private float timerRalentizado = 0;

    public bool pausado;

    float velocidad;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region Componentes y variables
        agente = GetComponent<NavMeshAgent>();
        controlador = GetComponent<ControladorEntidad>();
        velocidad = controlador.stats.velocidadMaxima;
        #endregion

        Caminar();
    }

    // Update is called once per frame
    void Update()
    {
        if(base1 != null)
        {
            ComprobarBases();
        }
        #region Ralentizado y su timer
        // reduce el tiempo del timer ralentizado
        if (timerRalentizado > 0)
        {
            timerRalentizado -= Time.deltaTime;
        } else
        {
            // Si no esta ralentizado la velocidad es normal
            velocidad = controlador.stats.velocidadMaxima;
        }
        #endregion
    }

    public void ComprobarBases()
    {
        #region Comprueba vida de las bases
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
        #endregion
    }

    public void Parar()
    {
        agente.speed = 0f;
    }

    public void Ralentizado(float cantidad, float tiempo)
    {
        tiempo = timerRalentizado;
        velocidad *= cantidad;
    }

    public void Caminar()
    {
        agente.speed = velocidad;
    }

    public void AsignarBases(Base base1, Base base2, Base base3)
    {
        this.base1 = base1;
        this.base2 = base2;
        this.base3 = base3;
    }
}
