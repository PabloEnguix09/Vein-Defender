using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: Spawner.cs
    // STATUS: WIP
    // GAMEOBJECT: Spawner
    // DESCRIPTION: El spawner genera Enemigo en la escena
    //
    // AUTHOR: Jorge
    // FEATURES ADDED: Configuración y funcionalidad del spawner al completo, añadidos los SO
    // ---------------------------------------------------

    [Header("Configuracion")]
    public RondaSO ronda;

    // El radio de aparicion alrededor del spawner
    private float radioDeAparicion;

    // Aqui se guarda la posicion del spawner
    private int xPos;
    private int zPos;


    [Header("Enemigos")]
    // Aqui van los GameObject de los Enemigo, pueden ser distintos o iguales y el codigo se puede expandir hasta tener todo tipo de Enemigo diferentes
    public GameObject primerTipoDeEnemigo;
    public GameObject segundoTipoDeEnemigo;
    public GameObject tercerTipoDeEnemigo;

    [Header("Contadores")]
    // Conteo se usa para saber la cantidad de Enemigo spawneados
    public int contador;
    public int contador2;
    public int contador3;

    [Header("Limites")]
    // El limite de aparicion de Enemigo.
    public int limitePrimerEnemigo;
    public int limiteSegundoEnemigo;
    public int limiteTercerEnemigo;

    [Header("Tiempo de aparicion")]
    // El tiempo de aparcicion entre Enemigo
    public float tiempoAparicionPrimerEnemigo;
    public float tiempoAparicionSegundoEnemigo;
    public float tiempoAparicionTercerEnemigo;

    [Header("Bases")]
    // Las bases a las que apuntaran los Enemigo generados
    public Base primeraBase;
    public Base segundaBase;
    public Base terceraBase;

    void Start()
    {

        primerTipoDeEnemigo = ronda.primerTipoDeEnemigo;
        segundoTipoDeEnemigo = ronda.segundoTipoDeEnemigo;
        tercerTipoDeEnemigo = ronda.tercerTipoDeEnemigo;

        limitePrimerEnemigo = ronda.limitePrimerEnemigo;
        limiteSegundoEnemigo = ronda.limiteSegundoEnemigo;
        limiteTercerEnemigo = ronda.limiteTercerEnemigo;

        tiempoAparicionPrimerEnemigo = ronda.tiempoAparicionPrimerEnemigo;
        tiempoAparicionSegundoEnemigo = ronda.tiempoAparicionSegundoEnemigo;
        tiempoAparicionTercerEnemigo = ronda.tiempoAparicionTercerEnemigo;

        radioDeAparicion = ronda.radioSpawn;

        int xPos1 = (int)(this.transform.position.x - radioDeAparicion);
        int xPos2 = (int)(this.transform.position.x + radioDeAparicion);
        xPos = Random.Range(xPos1, xPos2);
        int zPos1 = (int)(this.transform.position.z - radioDeAparicion);
        int zPos2 = (int)(this.transform.position.z + radioDeAparicion);
        zPos = Random.Range(zPos1, zPos2);

    }

    //creacion de la funcion que genera drones y les asigna sus bases objetivo
    public IEnumerator Aparicion()
    {
        while (contador < limitePrimerEnemigo)
        {
            yield return new WaitForSeconds(tiempoAparicionPrimerEnemigo);
            
            // Se crea el enemigo y se le asignan las bases
            GameObject entidad =  Instantiate(primerTipoDeEnemigo, new Vector3(xPos, 1, zPos), Quaternion.identity);
            ControladorEntidad enemigo = entidad.GetComponent<ControladorEntidad>();
            enemigo.AsignarBases(primeraBase, segundaBase, terceraBase);
            // Se agrupa en el gameObject vacio
            entidad.transform.parent = GameObject.Find(enemigo.nombre).transform;
            contador += 1;
        }
    }
    //creacion de la funcion que genera Enemigo bomba y les asigna sus bases objetivo
    public IEnumerator Aparicion2()
    {
        while (contador2 < limiteSegundoEnemigo)
        {
            yield return new WaitForSeconds(tiempoAparicionSegundoEnemigo);

            // Se crea el enemigo y se le asignan las bases
            GameObject entidad = Instantiate(segundoTipoDeEnemigo, new Vector3(xPos, 1, zPos), Quaternion.identity);
            ControladorEntidad enemigo = entidad.GetComponent<ControladorEntidad>();
            enemigo.AsignarBases(primeraBase, segundaBase, terceraBase);

            entidad.transform.parent = GameObject.Find(enemigo.nombre).transform;
            contador2 += 1;
        }
    }

    public IEnumerator Aparicion3()
    {
        while (contador3 < limiteTercerEnemigo)
        {
            yield return new WaitForSeconds(tiempoAparicionTercerEnemigo);

            // Se crea el enemigo y se le asignan las bases
            GameObject entidad = Instantiate(tercerTipoDeEnemigo, new Vector3(xPos, 1, zPos), Quaternion.identity);
            ControladorEntidad enemigo = entidad.GetComponent<ControladorEntidad>();
            enemigo.AsignarBases(primeraBase, segundaBase, terceraBase);

            entidad.transform.parent = GameObject.Find(enemigo.nombre).transform;
            contador3 += 1;
        }
    }

    public void SetRonda(int ronda)
    {
        switch (ronda)
        {
            case 1:
                StartCoroutine(Aparicion());
                break;
            case 2:
                StartCoroutine(Aparicion());
                StartCoroutine(Aparicion2());
                break;
            case 3:
                StartCoroutine(Aparicion());
                StartCoroutine(Aparicion2());
                StartCoroutine(Aparicion3());
                break;
            case 4:
                StartCoroutine(Aparicion());
                StartCoroutine(Aparicion2());
                StartCoroutine(Aparicion3());
                break;
            default:

                break;
        }
    }

}
