using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scutum : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: Scutum.cs
    // STATUS: WIP
    // GAMEOBJECT: Scutum
    // DESCRIPTION: La scutum crea barreras de energia
    //
    // AUTHOR: Jorge
    // FEATURES ADDED: Todas las funcionalidades de la scutum
    // ---------------------------------------------------
    public int conexiones = 0;
    public GameObject muro;
    public List<GameObject> scutumConectadas;
    public List<GameObject> murosConectados;
    public TorretaBasica scutum;
    public int regeneracion;
    public float timerRegenerativo;
    Rigidbody rb;
    // Update is called once per frame

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        timerRegenerativo = 0;
        regeneracion = 2;
    }
    void Update()
    {
        for (int i = 0; i < murosConectados.Count; i++)
        {
            // Si la vida de uno de los muros es < 0 desactivamos el objeto
            if (murosConectados[i].gameObject.GetComponent<Torreta>().vidaActual <= 0)
            {
                murosConectados[i].SetActive(false);
                timerRegenerativo += Time.deltaTime;
                // Cuando ha pasado el tiempo de regeneración lo activamos
                if (timerRegenerativo > regeneracion)
                {
                    murosConectados[i].gameObject.GetComponent<Torreta>().vidaActual = murosConectados[i].gameObject.GetComponent<Torreta>().vidaMaxima;
                    murosConectados[i].SetActive(true);
                    timerRegenerativo = 0;
                }
            }
        }


    }

    void BuscarScutum()
    {
        // Buscamos las demas scutum
        GameObject[] scutumDisponibles = GameObject.FindGameObjectsWithTag("Scutum");
        List<GameObject> scutumEnRango = new List<GameObject>();

        // Buscamos las scutum a rango
        if (scutumDisponibles.Length >= 1)
        {
            for (int i = 0; i < scutumDisponibles.Length; i++)
            {
                if (Vector3.Distance(transform.position, scutumDisponibles[i].transform.position) < scutum.distanciaDisparo && ComprobarVision(scutumDisponibles[i]))
                {
                    scutumEnRango.Add(scutumDisponibles[i]);
                }

            }
        }

        // Si tenemos scutum en rango y no hemos llegado a las 2 conexiones
        if (scutumEnRango.Count >= 1 && conexiones <= 1)
        {
            for (int i = 0; i < scutumEnRango.Count; i++)
            {
                // Cuando lleguemos a dos rompemos el bucle
                if (conexiones == 2)
                {
                    break;
                }
                
                // Si las conexiones de esa scutum son menores que 2 y no la he conectado ya
                if (scutumEnRango[i].GetComponent<Scutum>().conexiones < 2 && !scutumConectadas.Contains(scutumEnRango[i]) && !(transform.position == scutumDisponibles[i].transform.position))
                {
                    // Buscamos las dirección de la scutum objetivo
                    Vector3 dir = gameObject.transform.position - scutumEnRango[i].transform.position;
                    Quaternion VisionRotacion = Quaternion.LookRotation(dir);

                    // Si es 0 es que somos nosotros
                    if(!(VisionRotacion.eulerAngles == Vector3.zero))
                    {
                        // Creamos el muroObjeto
                        GameObject muroObjeto = Instantiate(muro, gameObject.transform.position, VisionRotacion);
                        muroObjeto.transform.Rotate(Vector3.up * 90);

                        // Referenciamos el lineRenderer y el boxCollider
                        LineRenderer lRend = muroObjeto.GetComponent<LineRenderer>();
                        BoxCollider boxCollider = muroObjeto.GetComponent<BoxCollider>();

                        // Buscamos las posiciones y las subimos en y 1
                        Vector3 pos1 = gameObject.transform.position;
                        pos1.y += 1;

                        Vector3 pos2 = scutumEnRango[i].transform.position;
                        pos2.y += 1;

                        // Las ponemos en la linea
                        lRend.SetPosition(0, pos1);
                        lRend.SetPosition(1, pos2);

                        
                        // Añadimos el tamaño y el centro para los boxCollider
                        Vector3 size = scutumEnRango[i].transform.position - gameObject.transform.position + Vector3.one;

                        Vector3 center = (pos1 + pos2) / 2;
                        
                        center.x = Vector3.Distance(gameObject.transform.position, scutumEnRango[i].transform.position) / 2;
                        center.y = 1.5f;
                        center.z = 0f;

                        size.x = center.x * 2;
                        size.y = 2f;
                        size.z = 0.5f;

                        // Los asignamos
                        boxCollider.center = center;
                        boxCollider.size = size;

                        // Añadimos las conexiones y variables necesarias a el scutum objetivo y al nuestro
                        conexiones++;
                        scutumConectadas.Add(scutumEnRango[i]);
                        murosConectados.Add(muroObjeto);
                        scutumEnRango[i].GetComponent<Scutum>().conexiones++;
                        scutumEnRango[i].GetComponent<Scutum>().scutumConectadas.Add(gameObject);
                        murosConectados.Add(muroObjeto);
                    }
                }
            }
        }
    }

    // Comprueba que el enemigo apuntado no tenga ninguna pared entre la torreta y el
    bool ComprobarVision(GameObject objetivo)
    {
        // Comprobamos que no tenga terreno entre la torreta y el enemigo
        RaycastHit hit;
        // no existe un collider entre el enemigo y la torreta
        Physics.Raycast(transform.position, Vector3.Normalize(objetivo.transform.position - transform.position), out hit, Vector3.Distance(transform.position, objetivo.transform.position), LayerMask.GetMask("Terreno"));

        if (hit.collider == null)
        {
            return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terreno")
        {
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            BuscarScutum();
        }

    }

}
