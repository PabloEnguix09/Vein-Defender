using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject primerTipoDeEnemigo;
    public GameObject segundoTipoDeEnemigo;
    private int xPos;
    private int zPos;
    public int conteo;
    public int conteo2;
    public int limitePrimerEnemigo;
    public int limiteSegundoEnemigo;
    public float radioDeAparicion;


    public Base primeraBase;
    public Base segundaBase;
    public Base terceraBase;

    // Start is called before the first frame update
    void Start()
    {
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
    
            GameObject bomba =  Instantiate(primerTipoDeEnemigo, new Vector3(xPos, 1, zPos), Quaternion.identity);
            Enemigo enemigo = bomba.GetComponent<Enemigo>();
            enemigo.AsignarBases(primeraBase, segundaBase, terceraBase);

            yield return new WaitForSeconds(0.5f);
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
            
            GameObject bomba  = Instantiate(segundoTipoDeEnemigo, new Vector3(xPos, 1, zPos), Quaternion.identity);
            Enemigo enemigo = bomba.GetComponent<Enemigo>();
            enemigo.AsignarBases(primeraBase, segundaBase, terceraBase);

            yield return new WaitForSeconds(1f);
            conteo2 += 1;
        }
    }
}
