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
    //
    // AUTHOR: Adrian Maldonado
    // FEATURES ADDED: Comprobacion de tener una torreta delante
    //
    // AUTHOR: Luis Belloch
    // FEATURES ADDED: Animaciones
    // ---------------------------------------------------

    private Base base1;
    private Base base2;
    private Base base3;
    public static Transform final;
    public NavMeshAgent agente;
    private Vector3 objetivo;

    public float vidaActual;

    public EnemigoBasico enemigo;
    public bool marcado = false;
    public bool invisibilidad;
    public bool subterraneo;
    public bool vuela;
    private int ataqueTemporal;

    public bool ralentizado;
    private float timerRalentizado;

    AnimEnemigo animEnemigo;

    private List<GameObject> tanques = new List<GameObject>();

    void Start()
    {

        agente = GetComponent<NavMeshAgent>();        // Script de control de las animaciones        animEnemigo = GetComponent<AnimEnemigo>();

        agente.speed = enemigo.velocidadInicial;
        enemigo.velocidadActual = enemigo.velocidadInicial;

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
        timerRalentizado = 0;
    }

    // Update is called once per frame
    void Update()
    {
        enemigo.ataqueFinal = enemigo.ataque + ataqueTemporal;
        // Busca un objetivo y se dirige hacia el
        objetivo = BuscarObjetivo();

        agente.destination = objetivo;
        // Si el objetivo esta en nuestro rango de disparo, nos quedamos quietos y perdemos la invisibilidad
        if (Vector3.Distance(this.transform.position, objetivo) <= enemigo.rangoDisparo)
        {
            if (enemigo.subterraneo)
            {
                subterraneo = false;
            }
            agente.speed = 0f;
            // Entra en la animacion bloqueado
            animEnemigo.Bloqueado(true);
        }
        // Si el objetivo no esta en el rango de disparo mantenemos la velocidad y la invisibilidad
        else
        {
            agente.speed = enemigo.velocidadActual;

            if (enemigo.subterraneo)
            {
                subterraneo = true;
            }
        }

        // Se mueve en la animacion idle
        animEnemigo.Bloqueado(false);

        agente.speed = enemigo.velocidadActual;
        if (ralentizado)
        {
            timerRalentizado += Time.deltaTime;
        }
        if(timerRalentizado >= 2)
        {
            ralentizado = false;
            enemigo.velocidadActual *= 2;
            timerRalentizado = 0;
        }

        // Muerte
        if(enemigo.vidaActual <= 0)
        {
            Destruido();
        }
    }
    
    public void Destruido()
    {
        animEnemigo.Destruido();

        Destroy(gameObject, 0.5f);
    }

    public void Marcar()
    {
        marcado = true;
    }

    public void Ralentizar()
    {
        if (!ralentizado)
        {
            ralentizado = true;
            enemigo.velocidadActual /= 2;
        }
        if (ralentizado && timerRalentizado > 0)
        {
            timerRalentizado = 0;
        }
    }

    public void AsignarBases(Base base1, Base base2, Base base3)
    {
        this.base1 = base1;
        this.base2 = base2;
        this.base3 = base3;
    }

    Vector3 BuscarObjetivo()
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
                    // Comprueba que delante tenga una torreta
                    RaycastHit hit;
                    Physics.Raycast(this.gameObject.transform.position, this.gameObject.transform.forward, out hit, this.enemigo.rango, LayerMask.GetMask("Torreta"));

                    if (hit.collider != null)
                    {
                        //En caso de tener delante una torreta
                        return objetivo = hit.point;

                    }

                    if (Vector3.Distance(this.transform.position, objetivosEnRango[i].transform.position) <= enemigo.rango)
                    {
                        return objetivo = objetivosEnRango[i].transform.position;
                    }
                }
            }
            // Si es el jugador y esta en rango
            else
            {
                if (Vector3.Distance(this.transform.position, objetivosEnRango[i].transform.position) <= enemigo.rango)
                {
                    return objetivo = objetivosEnRango[i].transform.position;
                }
            }
        }
        if (base1.Salud > 0)
        {
            return objetivo = base1.transform.position;
        }
        else if (base2.Salud > 0 && base1.Salud <= 0)
        {
            return objetivo = base2.transform.position;
        }
        else if (base3.Salud > 0 && base1.Salud <= 0 && base2.Salud <= 0)
        {
            return objetivo = base3.transform.position;
        }
        else
        {
            return objetivo = final.transform.position;
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
