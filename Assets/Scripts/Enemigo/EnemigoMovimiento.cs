using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
// FEATURES ADDED: Animaciones
// ---------------------------------------------------

public class EnemigoMovimiento : MonoBehaviour
{
    // Controlador enemigo
    EnemigoControlador controlador;
    EnemigoBasico stats;

    private Base base1;
    private Base base2;
    private Base base3;

    public static Transform final;
    public NavMeshAgent agente;
    private Vector3 objetivo;

    private float timerRalentizado = 0;
    bool volador;

    public bool pausado;

    float velocidad;

    // Start is called before the first frame update
    void Start()
    {
        #region Componentes y variables
        agente = GetComponent<NavMeshAgent>();

        stats = controlador.stats;
        #endregion

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

    // Update is called once per frame
    void Update()
    {
        #region Ralentizado y su timer
        // reduce el tiempo del timer ralentizado
        if (timerRalentizado > 0)
        {
            timerRalentizado -= Time.deltaTime;
            // La velocidad se reduce a la mitad
            velocidad = stats.velocidadMaxima / 2;
        } else
        {
            // Si no esta ralentizado la velocidad es normal
            velocidad = stats.velocidadMaxima;
        }
        #endregion
    }

    public void Parar()
    {
        agente.speed = 0f;
    }

    public void Ralentizado(float tiempo)
    {
        tiempo = timerRalentizado;
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
