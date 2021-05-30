using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scutum : MonoBehaviour
{
    public int conexiones = 0;
    public GameObject muro;
    public List<GameObject> scutumConectadas;
    public List<GameObject> murosConectados;
    public TorretaBasica scutum;
    public float timerRegenerativo;
    Rigidbody rb;
    // Update is called once per frame

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        timerRegenerativo = 0;
    }
    void Update()
    {
        for (int i = 0; i < murosConectados.Count; i++)
        {
            if (murosConectados[i].gameObject.GetComponent<Torreta>().vidaActual <= 0)
            {
                murosConectados[i].SetActive(false);
                timerRegenerativo += Time.deltaTime;
                if (timerRegenerativo > 2)
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
        GameObject[] scutumDisponibles = GameObject.FindGameObjectsWithTag("Scutum");
        List<GameObject> scutumEnRango = new List<GameObject>();

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

        if (scutumEnRango.Count >= 1 && conexiones <= 1)
        {
            for (int i = 0; i < scutumEnRango.Count; i++)
            {

                if (conexiones == 2)
                {
                    break;
                }

                if (scutumEnRango[i].GetComponent<Scutum>().scutumConectadas.Contains(gameObject) && !scutumConectadas.Contains(scutumEnRango[i]) && !(transform.position == scutumDisponibles[i].transform.position))
                {
                    Vector3 rotacion = gameObject.transform.position + scutumEnRango[i].transform.position;
                    GameObject muroObjeto = Instantiate(muro, gameObject.transform.position, gameObject.transform.rotation);
                    LineRenderer lRend = muroObjeto.GetComponent<LineRenderer>();
                    BoxCollider boxCollider = muroObjeto.GetComponent<BoxCollider>();

                    Vector3 pos1 = gameObject.transform.position;
                    pos1.y += 1;

                    Vector3 pos2 = scutumEnRango[i].transform.position;
                    pos2.y += 1;

                    lRend.SetPosition(0, pos1);
                    lRend.SetPosition(1, pos2);

                    Vector3 size = scutumEnRango[i].transform.position - gameObject.transform.position + Vector3.one;


                    Vector3 center = (pos1 + pos2) / 2;

                    size.y += 2;

                    center.x = pos1.x - pos2.x;
                    Debug.Log(pos1.x + " " + pos2.x + " " + center.x);
                    Debug.Log(center.x);
                    center.y = 1.5f;
                    center.z = 0f;

                    size.x = center.x * 2;
                    size.y = 2f;
                    size.z = 0.5f;

                    boxCollider.center = center;
                    boxCollider.size = size;

                    conexiones++;
                    scutumConectadas.Add(scutumEnRango[i]);
                    murosConectados.Add(muroObjeto);
                    scutumEnRango[i].GetComponent<Scutum>().conexiones++;
                    scutumEnRango[i].GetComponent<Scutum>().scutumConectadas.Add(gameObject);
                    murosConectados.Add(muroObjeto);
                }

                if (scutumEnRango[i].GetComponent<Scutum>().conexiones < 2 && !scutumConectadas.Contains(scutumEnRango[i]) && !(transform.position == scutumDisponibles[i].transform.position))
                {
                    Vector3 dir = gameObject.transform.position - scutumEnRango[i].transform.position;
                    Quaternion VisionRotacion = Quaternion.LookRotation(dir);

                    GameObject muroObjeto = Instantiate(muro, gameObject.transform.position, VisionRotacion);
                    muroObjeto.transform.Rotate(Vector3.up * 90);

                    LineRenderer lRend = muroObjeto.GetComponent<LineRenderer>();
                    BoxCollider boxCollider = muroObjeto.GetComponent<BoxCollider>();

                    Vector3 pos1 = gameObject.transform.position;
                    pos1.y += 1;

                    Vector3 pos2 = scutumEnRango[i].transform.position;
                    pos2.y += 1;

                    lRend.SetPosition(0, pos1);
                    lRend.SetPosition(1, pos2);

                    Vector3 size = scutumEnRango[i].transform.position - gameObject.transform.position + Vector3.one;


                    Vector3 center = (pos1 + pos2) / 2;

                    size.y += 2;

                    center.x = Vector3.Distance(gameObject.transform.position, scutumEnRango[i].transform.position)/2;
                    Debug.Log(pos1.x + " " + pos2.x + " " + center.x);
                    Debug.Log(center.x);
                    center.y = 1.5f;
                    center.z = 0f;

                    size.x = center.x * 2;
                    size.y = 2f;
                    size.z = 0.5f;

                    boxCollider.center = center;
                    boxCollider.size = size;

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
