using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Madre : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: Madre.cs
    // STATUS: WIP
    // GAMEOBJECT: Madre
    // DESCRIPTION: El enemigo Madre genera enemigos
    //
    // AUTHOR: Jorge
    // FEATURES ADDED: Configuración y funcionalidad de madre al completo
    // ---------------------------------------------------

    // Aqui ponemos todos los enemigos del juego
    public List<GameObject> enemigos;

    // Aqui se guarda la posicion del spawner
    private int xPos;
    private int zPos;

    // El radio de aparicion alrededor del spawner
    public float radioDeAparicion;

    // Las bases a las que apuntaran los Enemigo generados
    public Base primeraBase;
    public Base segundaBase;
    public Base terceraBase;

    public EnemigoBasico enemigoBasico;

    private float timerCreacion = 0;

    void Start()
    {
        // Buscamos las bases y las añadimos
        GameObject[] bases = GameObject.FindGameObjectsWithTag("Base");
        List<Base> objetivos = new List<Base>();
        objetivos.Add(bases[0].GetComponent<Base>());
        objetivos.Add(bases[1].GetComponent<Base>());
        objetivos.Add(bases[2].GetComponent<Base>());
        primeraBase = objetivos[0];
        segundaBase = objetivos[1];
        terceraBase = objetivos[2];
    }

    private void Update()
    {
        timerCreacion += Time.deltaTime;
        if (timerCreacion >= enemigoBasico.velocidadDisparo)
        {
            int xPos1 = (int)(this.transform.position.x - radioDeAparicion);
            int xPos2 = (int)(this.transform.position.x + radioDeAparicion);
            xPos = buscarPunto(xPos1, xPos2);
            int zPos1 = (int)(this.transform.position.z - radioDeAparicion);
            int zPos2 = (int)(this.transform.position.z + radioDeAparicion);
            zPos = buscarPunto(zPos1, zPos2);

            // Escoge un enemigo de la lista
            int numeroEnemigo = Random.Range(0, enemigos.Count);

            // Se crea el enemigo y se le asignan las bases
            GameObject creado = Instantiate(enemigos[numeroEnemigo], new Vector3(xPos, 1, zPos), Quaternion.identity);
            Enemigo enemigoCreado = creado.GetComponent<Enemigo>();
            enemigoCreado.AsignarBases(primeraBase, segundaBase, terceraBase);
            timerCreacion = 0;
        }
    }

    // Busca un punto lo suficientemente alejado de ella para que no choquen los colliders
    int buscarPunto(int Pos1, int Pos2)
    {
        int Pos = 0;
        while (Pos < 17 && Pos > -17)
        {
            Pos = Random.Range(Pos1, Pos2);
        }
        return Pos;
    }
}
