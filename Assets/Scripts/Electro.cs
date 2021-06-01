using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: Electro.cs
    // STATUS: WIP
    // GAMEOBJECT: Electro
    // DESCRIPTION: Este escript contiene la capacidad especial del electro
    //
    // AUTHOR: Jorge Grau
    // FEATURES ADDED: El electro muere y electrocuta torretas a rango.
    // ---------------------------------------------------
    public EnemigoBasico enemigo;
    private float vidaActual;
    public GameObject explosion;

    void Update()
    {
        // El electro comprueba que tiene vida
        vidaActual = enemigo.vidaActual;
        // Cuando su vida es menor que 0 explota y inhabilita las torretas
        if (vidaActual <= 0)
        {
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Electrocutar();
        }
    }

    private void Electrocutar()
    {
        // Recogemos los objetivos
        List<GameObject> objetivosEnRango = new List<GameObject>();
        GameObject[] torretas = GameObject.FindGameObjectsWithTag("Torreta");
        objetivosEnRango.AddRange(torretas);

        if (objetivosEnRango.Count >= 1)
        {
            for (int i = 0; i < objetivosEnRango.Count; i++)
            {
                // Si estan dentro de nuestro rango de explosion le electrocutamos llamando a InhabilitarTorreta()
                if (Vector3.Distance(gameObject.transform.position, objetivosEnRango[i].transform.position) < enemigo.rangoExplosion)
                {
                    torretas[i].GetComponent<Torreta>().InhabilitarTorreta();
                }
            }
        }
    }

}
