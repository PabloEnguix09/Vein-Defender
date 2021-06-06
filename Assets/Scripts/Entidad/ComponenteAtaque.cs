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

public class ComponenteAtaque : MonoBehaviour
{
    #region Variables
    ControladorEntidad controlador;
    // Cooldown de disparo
    float timerDisparo = 0;
    // Fuerza de ataque total
    [HideInInspector]
    public float fuerza;
    List<GameObject> potenciadores;
    private Vector3 objetivoADisparar;
    float timerDebilitacion = 0;
    float cantidadDebilitacion = 1;

    [Header("Componentes de ataque ¡Tocar solo si sabes lo que haces!")]
    public DisparadorSO disparadorSO;
    public ExplosivoSO explosivoSO;
    public PotenciadorSO potenciadorSO;
    #endregion

    private void Start()
    {
        controlador = GetComponent<ControladorEntidad>();
        // Fuerza base
        fuerza = controlador.stats.ataqueDisparo;

        // Crea un el controlador de ataque de la entidad si no tiene uno creado ya
        if (controlador.stats.tipoAtaque == EntidadSO.Tipo.laser && !disparadorSO) disparadorSO = ScriptableObject.CreateInstance<DisparadorSO>();
        if (controlador.stats.tipoAtaque == EntidadSO.Tipo.balas && !disparadorSO) disparadorSO = ScriptableObject.CreateInstance<DisparadorSO>();
        if (controlador.stats.explosivo && !explosivoSO) explosivoSO = ScriptableObject.CreateInstance<ExplosivoSO>();
        if (controlador.stats.tipoAtaque == EntidadSO.Tipo.potenciador && !potenciadorSO) potenciadorSO = ScriptableObject.CreateInstance<PotenciadorSO>();
    }
    // Update is called once per frame
    void Update()
    {

        if(potenciadorSO)
        {
            potenciadorSO.Potenciar(controlador, this.gameObject);
        }
        // Si la entidad dispara
        if(disparadorSO)
        {

            //busca al objetivo mas cercano
            objetivoADisparar = disparadorSO.BuscarObjetivo(controlador);
            //tiempo para la velocidad de ataque
            timerDisparo += Time.deltaTime;
            //si apunta a alguien
            if (objetivoADisparar != Vector3.zero)
            {
                // Tiene una torreta en rango
                controlador.ObjetivoEnRango();
                // rotar en direccion al objetivo apuntado
                Vector3 dir = objetivoADisparar - controlador.parteQueRota.position;
                Quaternion VisionRotacion = Quaternion.LookRotation(dir);
                //rotacion suave
                Vector3 rotacion = Quaternion.Lerp(controlador.parteQueRota.rotation, VisionRotacion, Time.deltaTime * controlador.stats.velocidadRotacion).eulerAngles;
                controlador.parteQueRota.rotation = Quaternion.Euler(rotacion.x, rotacion.y, rotacion.z);

                //si ha pasado el tiempo de recarga
                if (timerDisparo >= controlador.stats.velocidadDisparo)
                {
                    timerDisparo = 0;
                    disparadorSO.Disparar(controlador, fuerza, this.gameObject, objetivoADisparar);
                }
            }
            else
            {
                // Si no apunta a nadie vuelve a la animacion de caminar
                controlador.Caminar();
            }
        }
        #region Debilitacion y timer
        // Debilitacion
        if(timerDebilitacion > 0)
        {
            timerDebilitacion -= Time.deltaTime;
        } else
        {
            cantidadDebilitacion = 1;
        }
        #endregion
    }

    // Para el bomba cuando toca a un objetivo explota
    private void OnTriggerEnter(Collider other)
    {
        if(explosivoSO)
        {
            // Cuando choca contra una base, torreta o personaje se autodestruye
            if (other.gameObject.GetComponent<Base>() != null || other.gameObject.GetComponent<Torreta>() != null || other.gameObject.GetComponent<Personaje>())
            {
                explosivoSO.Explotar(controlador, controlador.stats.ataqueExplosion, this.gameObject);
            }
        }
    }

    // Controla el ataque cuando esta siendo potenciado
    public void Potenciado(bool estado, GameObject potenciador)
    {
        // Poner un potenciador nuevo
        if(estado)
        {
            potenciadores.Add(potenciador);
            CalcularFuerzaTotal();
        }
        // Quita un ptenciador existente en la lista
        if (!estado)
        {
            if (potenciadores.Contains(potenciador))
            {
                potenciadores.Remove(potenciador);
                CalcularFuerzaTotal();
            }
        }
    }

    void CalcularFuerzaTotal()
    {
        // Ponemos la fuerza a base
        fuerza = controlador.stats.ataqueDisparo;
        // Sumamos cada uno de los potenciadores
        for (int i = 0; i < potenciadores.Count; i++)
        {
            fuerza += potenciadores[i].GetComponent<ControladorEntidad>().stats.ataqueDisparo;
        }

        fuerza *= cantidadDebilitacion;
    }
    // La debilitacion es un factor entre 0 y 1
    public void Debilitado(float cantidad, float tiempo)
    {
        cantidadDebilitacion = cantidad;
    }

    // Se llama desde el controlador cuando el objeto tiene 0 de vida
    public void Explotar()
    {
        if(explosivoSO)
        {
            explosivoSO.Explotar(controlador, fuerza, this.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Muestra el rango de explosion para los enemigos explosivos
        if(explosivoSO)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, controlador.stats.rangoExplosion);
        }
        // Muestra el rango de disparo para los enemigos de rango
        if(disparadorSO ||
           potenciadorSO)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, controlador.stats.rangoDisparo);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, controlador.stats.rangoDeteccion);

    }
}