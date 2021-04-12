using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: nombre
// STATUS: estado
// GAMEOBJECT: objeto
// DESCRIPTION: descripcion
//
// AUTHOR: Pablo Enguix Llopis
// FEATURES ADDED: cosas hecha
//
// AUTHOR: Jorge Grau
// FEATURES ADDED: Comprobación de energia y gasto de energia.
// ---------------------------------------------------


public class InvocarTorreta : MonoBehaviour
{
    public GameObject[] torretas;
    public float alcance = 50.0f;

    private GameObject torreta;
    private Rigidbody rb;
    private bool colocada = true;

    private Personaje personaje;

    public bool GetColocada()
    {
        return colocada;
    }

    public void SetColocada(bool value)
    {
        colocada = value;
    }

    private ComprobarSitio sitio;

    // Start is called before the first frame update
    void Start()
    {
        personaje = this.GetComponent<Personaje>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit punto;

        // Si existe torreta y está en el rango de colocación
        if (torreta != null && (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out punto, alcance, LayerMask.GetMask("Terreno"))))
        {
            torreta.transform.position = new Vector3(punto.point.x, punto.point.y, punto.point.z);
        }

        // Si pulsa clic izquierdo se "destruye" la torreta de previsualización y spawnea la otra más arriba
        if (Input.GetMouseButtonDown(0))
        {

            if (PosicionLegal())
            {
                Transform torretaSpawn = torreta.transform;
                Destroy(torreta.gameObject);
                torreta = null;
                rb = torretaSpawn.GetComponent<Rigidbody>();
                rb.mass = 1f;
                rb.constraints = RigidbodyConstraints.None;
                SpawnTorreta(torretaSpawn);
            }
        }

        // Si pulsa Esc se destruye la previsualización
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(torreta.gameObject);
            torreta = null;
            SetColocada(true);
            return;
        }
    }

    private bool PosicionLegal()
    {
        return sitio.colliders.Count <= 0;
    }

    public void PreviewTorreta(string nombre)
    {
        for (int i = 0; i < torretas.Length; i++)
        {
            if (torretas[i].name == nombre)
            {
                torreta = ((GameObject)Instantiate(torretas[i]));
                sitio = torreta.GetComponent<ComprobarSitio>();
                rb = torreta.GetComponent<Rigidbody>();
                rb.mass = 0f;
            }
        }
    }

    public void SpawnTorreta(Transform torreta)
    {
        SetColocada(true);
        GameObject aux;
        aux = Instantiate(torreta.gameObject, new Vector3(torreta.position.x, 20, torreta.position.z), Quaternion.identity);
        aux.GetComponent<Torreta>().enabled = true;
        aux.GetComponent<TorretaBasica>().enabled = true;
        Torreta torretaCreada = aux.GetComponent<Torreta>();
        if(personaje.Energia - torretaCreada.gastoEnergia < 0)
        {
            Destroy(aux);
            return;
        }
        personaje.Energia -= torretaCreada.gastoEnergia;
    }
}
