using System.Collections;
using System.Collections.Generic;
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
    // FEATURES ADDED: Los enemigos tienen unas estadisticas que eredan de su tipo y siguen una ruta establecida por su spawner, si algo aparece en su radio de vision van a por el. Tambien el recibir daño por disparos.
    // ---------------------------------------------------

    private Base base1;
    private Base base2;
    private Base base3;
    public static Transform final;
    public NavMeshAgent agente;
    private Transform objetivo;

    public float vidaActual;

    public EnemigoBasico enemigo;

    void Start()
    {

        agente = GetComponent<NavMeshAgent>();

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

    }

    public void AsignarBases(Base base1, Base base2, Base base3)
    {
        this.base1 = base1;
        this.base2 = base2;
        this.base3 = base3;

    }

    Transform BuscarObjetivo()
    {
        // Se crea una esfera buscando todos los colliders en el rango de vision, si encuentra una torreta o a un enemigo se dirige hacia el.
        Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, enemigo.rango);

        for (int i = 0; i < colliders.Length; i++)
        {

            if (colliders[i].CompareTag("Player"))
            {
                return objetivo = colliders[i].transform;
            }

            else if (colliders[i].CompareTag("Torretas"))
            {
                return objetivo = colliders[i].transform;
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

    private void OnTriggerEnter(Collider other)
    {
        // Si es golpeado por una bala recibe daño y la bala se destruye.
        if (other.gameObject.GetComponent<Bala>() != null)
        {
            Bala bala = other.gameObject.GetComponent<Bala>();
            vidaActual -= bala.fuerza;
            Destroy(other.gameObject);
        }
    }

}
