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
    public float fuerza;
    List<GameObject> potenciadores = new List<GameObject>();
    private Vector3 objetivoADisparar;
    float timerDebilitacion = 0;
    float cantidadDebilitacion = 1;

    DisparadorSO disparadorSO;
    ExplosivoSO explosivoSO;
    PotenciadorSO potenciadorSO;
    AudioHandler audioHandler;
    #endregion

    private void Start()
    {
        controlador = GetComponent<ControladorEntidad>();
        audioHandler = GetComponent<AudioHandler>();
        // Fuerza base
        fuerza = controlador.stats.ataqueDisparo;

        // Crea un el controlador de ataque de la entidad si no tiene uno creado ya
        if (controlador.stats.tipoAtaque == EntidadSO.Tipo.laser) disparadorSO = gameObject.AddComponent(typeof(DisparadorSO)) as DisparadorSO;
        if (controlador.stats.tipoAtaque == EntidadSO.Tipo.balas) disparadorSO = gameObject.AddComponent(typeof(DisparadorSO)) as DisparadorSO;
        if (controlador.stats.explosivo) explosivoSO = gameObject.AddComponent(typeof(ExplosivoSO)) as ExplosivoSO;
        if (controlador.stats.tipoAtaque == EntidadSO.Tipo.potenciador) potenciadorSO = new PotenciadorSO();
    }
    // Update is called once per frame
    void Update()
    {
        CalcularFuerzaTotal();

        if(potenciadorSO != null)
        {
            potenciadorSO.Potenciar(controlador, this.gameObject);
        }
        // Si la entidad dispara
        if(disparadorSO != null)
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
                    Disparar();
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
        if(explosivoSO != null)
        {
            // Cuando choca contra una base, torreta o personaje se autodestruye
            if (other.gameObject.GetComponent<Base>() != null || other.gameObject.GetComponent<Torreta>() != null || other.gameObject.GetComponent<Personaje>())
            {
                explosivoSO.Explotar(controlador, controlador.stats.ataqueExplosion, this.gameObject);
                audioHandler.Play(0);
            }
        }
    }

    // Controla el ataque cuando esta siendo potenciado
    public void Potenciado(bool estado, GameObject potenciador)
    {
        // Poner un potenciador nuevo
        if(estado)
        {
            if(!potenciadores.Contains(potenciador))
            {
                potenciadores.Add(potenciador);
                CalcularFuerzaTotal();
            }
        }
        // Quita un potenciador existente en la lista
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
            // Comprueba que no este destruido el objeto o este demasiado lejos del objeto actual
            if(potenciadores[i] == null)
            {
                potenciadores.RemoveAt(i);
                continue;
            }
            if(Vector3.Distance(potenciadores[i].transform.position, this.transform.position) 
                > potenciadores[i].GetComponent<ControladorEntidad>().stats.rangoDisparo)
            {
                potenciadores.RemoveAt(i);
                continue;
            }
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
        if(explosivoSO != null)
        {
            explosivoSO.Explotar(controlador, fuerza, this.gameObject);
            audioHandler.Play(0);
        }
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
        ataqueObjeto.origenTag = gameObject.tag;

        // Se busca la direccion desde donde esta atacando al objetivo
        Vector3 direccion = gameObject.transform.position - objetivoADisparar;
        ataqueObjeto.direccion = Vector3.Dot(gameObject.transform.forward, direccion);

        // Instancia el disparo
        Bala bala = Instantiate(controlador.balaObjeto, controlador.spawnerBalas.transform.position, controlador.spawnerBalas.transform.rotation).GetComponent<Bala>();
        bala.ataque = ataqueObjeto;

        audioHandler.Play(0);
    }

    private void OnDrawGizmosSelected()
    {
        // Muestra el rango de explosion para los enemigos explosivos
        if(explosivoSO != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, controlador.stats.rangoExplosion);
        }
        // Muestra el rango de disparo para los enemigos de rango
        if(disparadorSO != null ||
           potenciadorSO != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, controlador.stats.rangoDisparo);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, controlador.stats.rangoDeteccion);

    }
}