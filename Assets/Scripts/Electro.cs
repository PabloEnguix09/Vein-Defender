using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electro : MonoBehaviour
{
    public EnemigoBasico enemigo;
    private float vidaActual;
    public GameObject explosion;
    void Update()
    {
        vidaActual = enemigo.vidaActual;
        if (vidaActual <= 0)
        {
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);

            Electrocutar();

            Destroy(gameObject);
        }
    }

    private void Electrocutar()
    {
        List<GameObject> objetivosEnRango = new List<GameObject>();
        GameObject[] torretas = GameObject.FindGameObjectsWithTag("Torreta");
        objetivosEnRango.AddRange(torretas);

        if (objetivosEnRango.Count >= 1)
        {
            for (int i = 0; i < objetivosEnRango.Count; i++)
            {
                if(Vector3.Distance(gameObject.transform.position, objetivosEnRango[i].transform.position) < enemigo.rangoExplosion)
                {
                    torretas[i].GetComponent<Torreta>().InhabilitarTorreta();
                }
            }
        }
    }

}
