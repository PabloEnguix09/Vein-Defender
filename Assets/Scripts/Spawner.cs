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
    // FEATURES ADDED: Configuración y funcionalidad del spawner al completo
    //
    // AUTHOR: Pau
    // FEATURES ADDED: funcion para crear drones
    // ---------------------------------------------------

    // Aqui van los GameObject de los Enemigo, pueden ser distintos o iguales y el codigo se puede expandir hasta tener todo tipo de Enemigo diferentes
    public GameObject primerTipoDeEnemigo;
    public GameObject segundoTipoDeEnemigo;

    // Aqui se guarda la posicion del spawner
    private int xPos;
    private int zPos;

    // Conteo se usa para saber la cantidad de Enemigo spawneados
    public int conteo;
    public int conteo2;

    // El limite de aparicion de Enemigo.
    public int limitePrimerEnemigo;
    public int limiteSegundoEnemigo;

    // El tiempo de aparcicion entre Enemigo
    public float tiempoAparicionPrimerEnemigo;
    public float tiempoAparicionSegundoEnemigo;

    // El radio de aparicion alrededor del spawner
    public float radioDeAparicion;

    // Las bases a las que apuntaran los Enemigo generados
    public Base primeraBase;
    public Base segundaBase;
    public Base terceraBase;

    void Start()
    {
        // La posicion a la que se deben dirigir los Enemigo
        Enemigo.final = gameObject.transform;

        //StartCoroutine(Aparicion());
        //StartCoroutine(AparicionBombas());
    }

    //creacion de la funcion que genera drones y les asigna sus bases objetivo
    public IEnumerator Aparicion()
    {
        while (conteo < limitePrimerEnemigo)
        {

            int xPos1 = (int)(this.transform.position.x - radioDeAparicion);
            int xPos2 = (int)(this.transform.position.x + radioDeAparicion);
            xPos = Random.Range(xPos1, xPos2);
            int zPos1 = (int)(this.transform.position.z - radioDeAparicion);
            int zPos2 = (int)(this.transform.position.z + radioDeAparicion);
            zPos = Random.Range(zPos1, zPos2);
            
            // Se crea el enemigo y se le asignan las bases
            GameObject dron =  Instantiate(primerTipoDeEnemigo, new Vector3(xPos, 1, zPos), Quaternion.identity);
            Enemigo enemigo = dron.GetComponent<Enemigo>();
            enemigo.AsignarBases(primeraBase, segundaBase, terceraBase);

            yield return new WaitForSeconds(tiempoAparicionPrimerEnemigo);
            conteo += 1;
        }
    }
    //creacion de la funcion que genera Enemigo bomba y les asigna sus bases objetivo
    public IEnumerator AparicionBombas()
    {
        while (conteo2 < limiteSegundoEnemigo)
        {
            int xPos1 = (int)(this.transform.position.x - radioDeAparicion);
            int xPos2 = (int)(this.transform.position.x + radioDeAparicion);
            xPos = Random.Range(xPos1, xPos2);
            int zPos1 = (int)(this.transform.position.z - radioDeAparicion);
            int zPos2 = (int)(this.transform.position.z + radioDeAparicion);
            zPos = Random.Range(zPos1, zPos2);

            // Se crea el enemigo y se le asignan las bases
            GameObject bomba  = Instantiate(segundoTipoDeEnemigo, new Vector3(xPos, 1, zPos), Quaternion.identity);
            Enemigo enemigo = bomba.GetComponent<Enemigo>();
            enemigo.AsignarBases(primeraBase, segundaBase, terceraBase);

            yield return new WaitForSeconds(tiempoAparicionSegundoEnemigo);
            conteo2 += 1;
        }
    }
}
