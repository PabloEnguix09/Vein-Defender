using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanque : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: Tanque.cs
    // STATUS: WIP
    // GAMEOBJECT: Tanque
    // DESCRIPTION: El enemigo tanque potencia a los enemigos cercanos
    //
    // AUTHOR: Jorge
    // FEATURES ADDED: Configuración y funcionalidad del tanque al completo
    // ---------------------------------------------------

    public EnemigoBasico enemigoBasico;
    public Enemigo tanque;
    private float rango;

    void Start()
    {

        // Recogemos todos los objetivos de la zona
        GameObject[] enemigo = GameObject.FindGameObjectsWithTag("Enemigo");
        List<Enemigo> enemigos = new List<Enemigo>();
        for(int i = 0; i < enemigo.Length; i++)
        {
            enemigos.Add(enemigo[i].GetComponent<Enemigo>());
        }

        rango = enemigoBasico.rango;

    }

    private void Update()
    {
        // Recogemos todos los objetivos de la zona
        GameObject[] enemigo = GameObject.FindGameObjectsWithTag("Enemigo");

        // Si su vida es <= 0 pone su rango a 0 para eliminar los buff de los enmeigos
        if (tanque.vidaActual <= 0)
        {
            rango = 0;
            // Se destruye
            tanque.Destruido();
        }

        for (int i = 0; i < enemigo.Length; i++)
        {
            // Su tiene un enemigo en rango le da un buff
            if(Vector3.Distance(this.transform.position, enemigo[i].transform.position) < rango)
            {
                enemigo[i].GetComponent<Enemigo>().RecibirBuff(this.gameObject);
            }
            // Su tiene un enemigo fuera de rango se lo quita
            else if (Vector3.Distance(this.transform.position, enemigo[i].transform.position) > rango)
            {
                enemigo[i].GetComponent<Enemigo>().EliminarBuff(this.gameObject);

            }
        }


    }
}
