using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torreta : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: nombre
    // STATUS: estado
    // GAMEOBJECT: objeto
    // DESCRIPTION: descripcion
    //
    // AUTHOR: autor
    // FEATURES ADDED: Añadido gasto energetico
    // ---------------------------------------------------

    [Range(0, 1)]
    [SerializeField]
    public float vida;

    public float Vida
    {
        get { return vida; }

        set
        {
            value = Mathf.Clamp01(value);
            vida = value;
        }
    }

    [Range(0, 1)]
    [SerializeField]
    public float fuerza;

    public float Fuerza
    {
        get { return fuerza; }

        set
        {
            value = Mathf.Clamp01(value);
            fuerza = value;
        }
    }

    public float velocidadAtaque;

    public float VelocidadAtaque
    {
        get { return velocidadAtaque; }

        set
        {
            value = Mathf.Clamp01(value);
            velocidadAtaque = value;
        }
    }

    public float rango;

    public float Rango
    {
        get { return rango; }

        set
        {
            value = Mathf.Clamp01(value);
            rango = value;
        }
    }

    public float gastoEnergia;

    private GameObject enemigoApuntando;
    private float timer;

    public Transform parteQueRota;
    public float velocidadGiro = 10f;
    private Disparo disparo;

    // Start is called before the first frame update
    void Start()
    {
        enemigoApuntando = null;
        disparo = GetComponentInChildren<Disparo>();
    }

    // Update is called once per frame
    void Update()
    {

        enemigoApuntando = BuscarEnemigo();
        timer += Time.deltaTime;
        if (enemigoApuntando != null)
        {
            Vector3 dir = parteQueRota.position - enemigoApuntando.transform.position ;
            Quaternion VisionRotacion = Quaternion.LookRotation(dir);
            Vector3 rotacion = Quaternion.Lerp(parteQueRota.rotation, VisionRotacion, Time.deltaTime * velocidadGiro).eulerAngles;
            parteQueRota.rotation = Quaternion.Euler(rotacion.x, rotacion.y, rotacion.z);

            //Quaternion rotacion = Quaternion.LookRotation(enemigoApuntando.transform.position - transform.position);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, 10f * Time.deltaTime);

            if (timer >= velocidadAtaque)
            {

                timer = 0;
                Enemigo e = enemigoApuntando.GetComponent<Enemigo>();
                disparo.Disparar();
                //e.vida -= fuerza;
            }
        }
    }

    GameObject BuscarEnemigo()
    {
        float menorDistancia = Mathf.Infinity;
        bool encontrado = false;
        GameObject enemigoMasCercano = null;

        Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, rango);
        for (int i = 0; i < colliders.Length; i++)
        {

            if (colliders[i].CompareTag("Enemigos"))
            {
                encontrado = true;
                RaycastHit hit;
                if (Physics.Linecast(transform.position, colliders[i].transform.position, out hit, 1, QueryTriggerInteraction.Ignore))
                {

                }
                else
                {
                    if (enemigoMasCercano == null)
                    {
                        enemigoMasCercano = colliders[i].gameObject;
                        menorDistancia = Vector3.Distance(colliders[i].gameObject.transform.position, transform.position);
                    }
                    else
                    {
                        float distancia = Vector3.Distance(colliders[i].gameObject.transform.position, transform.position);
                        if (distancia < menorDistancia)
                        {
                            menorDistancia = distancia;
                            enemigoMasCercano = colliders[i].gameObject;
                        }
                    }
                }


            }
        }
        if (encontrado) return enemigoMasCercano;
        else return null;
    }
}
