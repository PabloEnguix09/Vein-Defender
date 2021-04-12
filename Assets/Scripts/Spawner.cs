using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: Spawner.cs
    // STATUS: WIP
    // GAMEOBJECT: Spawner
    // DESCRIPTION: El spawner genera enemigos en la escena
    //
    // AUTHOR: Jorge
    // FEATURES ADDED: Configuración y funcionalidad del spawner al completo
    // ---------------------------------------------------

    // Aqui van los GameObject de los enemigos, pueden ser distintos o iguales y el codigo se puede expandir hasta tener todo tipo de enemigos diferentes
    public GameObject primerTipoDeEnemigo;
    public GameObject segundoTipoDeEnemigo;

    // Aqui se guarda la posicion del spawner
    private int xPos;
    private int zPos;

    // Conteo se usa para saber la cantidad de enemigos spawneados
    public int conteo;
    public int conteo2;

    // El limite de aparicion de enemigos.
    public int limitePrimerEnemigo;
    public int limiteSegundoEnemigo;

    // El tiempo de aparcicion entre enemigos
    public float tiempoAparicionPrimerEnemigo;
    public float tiempoAparicionSegundoEnemigo;

    // El radio de aparicion alrededor del spawner
    public float radioDeAparicion;

    // Las bases a las que apuntaran los enemigos generados
    public Base primeraBase;
    public Base segundaBase;
    public Base terceraBase;

    void Start()
    {
        // La posicion a la que se deben dirigir los enemigos
        Enemigo.final = gameObject.transform;

        //StartCoroutine(Aparicion());
        //StartCoroutine(AparicionBombas());
    }


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
            GameObject bomba =  Instantiate(primerTipoDeEnemigo, new Vector3(xPos, 1, zPos), Quaternion.identity);
            Enemigo enemigo = bomba.GetComponent<Enemigo>();
            enemigo.AsignarBases(primeraBase, segundaBase, terceraBase);

            yield return new WaitForSeconds(tiempoAparicionPrimerEnemigo);
            conteo += 1;
        }
    }
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
