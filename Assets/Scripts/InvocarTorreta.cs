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
// 
// AUTHOR: Luis Belloch
// FEATURES ADDED: Arreglos de energía, EliminarTorreta(), menu radial, cambios en la rotacion de invocacion
// ---------------------------------------------------


public class InvocarTorreta : MonoBehaviour
{
    public GameObject[] torretas;
    public GameObject[] previews;
    public float alcance = 50.0f;
    public float alturaSpawn = 50.0f;

    private GameObject torreta;
    private Rigidbody rb;
    private bool colocada = true;

    public GameObject menuRadial;

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
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit punto;

        // Si existe torreta y está en el rango de colocación
        if (torreta != null && (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out punto, alcance, LayerMask.GetMask("Terreno"))))
        {
            // Posicion y rotacion de la preview
            torreta.transform.position = new Vector3(punto.point.x, punto.point.y, punto.point.z);
            torreta.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);

            // Si pulsa clic izquierdo se "destruye" la torreta de previsualización y spawnea la otra más arriba
            if (Input.GetMouseButtonDown(0))
            {
                if (PosicionLegal())
                {
                    for(int i = 0; i < previews.Length; i++)
                    {
                        if(previews[i].gameObject.name + "(Clone)" == torreta.gameObject.name)
                        {
                            // Posicion de la nueva torreta invocada
                            Transform torretaSpawn = torretas[i].transform;
                            torretaSpawn.position = torreta.transform.position;
                            torretaSpawn.rotation = torreta.transform.rotation;
                            Destroy(torreta.gameObject);
                            // Datos para la nueva torreta invocada
                            torreta = null;
                            rb = torretaSpawn.GetComponent<Rigidbody>();
                            rb.mass = 1f;
                            // Solo permite la rotacion en Y
                            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                            rb.constraints = RigidbodyConstraints.FreezePosition;
                            SpawnTorreta(torretaSpawn);
                        }
                    }
                }
            }

            // Si pulsa Esc se destruye la previsualización
            if (Input.GetKeyDown(KeyCode.C))
            {
                Destroy(torreta.gameObject);
                torreta = null;
                SetColocada(true);
                return;
            }
        }

        // Comportamiento del menu radial solo si esta activo
        if(menuRadial.activeSelf)
        {
            RaycastHit hit;
            
            if(Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // Comprueba que este apuntando a una opcion en el Layer UI
                if (Physics.Raycast(ray, out hit, LayerMask.GetMask("UI")))
                {
                    // Debug.Log("Torreta seleccionada: ");
                    // Las opciones del menu son numeros
                    bool hecho = int.TryParse(hit.transform.name, out int index);
                    if (hecho)
                    {
                        // Se previsualiza la torreta
                        SetColocada(false);
                        PreviewTorreta(index);

                        AlternarMenuRadial();
                    }
                }
            }
        }
    }

    private bool PosicionLegal()
    {
        return sitio.colliders.Count <= 0;
    }

    public void PreviewTorreta(int index)
    {
        torreta = ((GameObject)Instantiate(previews[index]));
        sitio = torreta.GetComponent<ComprobarSitio>();
        rb = torreta.GetComponent<Rigidbody>();
        rb.mass = 0f;
    }

    public void SpawnTorreta(Transform torreta)
    {
        SetColocada(true);
        GameObject aux;
        aux = Instantiate(torreta.gameObject, new Vector3(torreta.position.x, torreta.position.y + alturaSpawn, torreta.position.z), Quaternion.identity);
        aux.GetComponent<Torreta>().enabled = true;
        /* Ahora se comprueba desde TorretaBasica Start()
        Torreta torretaCreada = aux.GetComponent<Torreta>();
        if(personaje.Energia - torretaCreada.torretaBasica.energia < 0)
        {
            Destroy(aux);
            return;
        }
        personaje.Energia -= torretaCreada.torretaBasica.gastoEnergia;
        */
    }

    // Llamado desde MecanicasPersonaje.cs
    public void EliminarTorreta()
    {
        RaycastHit punto;
        // Comprueba que este apuntando a una torreta en el Layer Torreta
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out punto, alcance, LayerMask.GetMask("Torreta"))) {
            // Toma la torreta del RaycastHit
            GameObject torretaMarcada = punto.transform.gameObject;
            // Destruye la torreta
            torretaMarcada.GetComponent<Torreta>().DestruirTorreta();
        }
    }

    public void AlternarMenuRadial()
    {
        // Si esta desactivado se activa y viceversa
        menuRadial.SetActive(!menuRadial.activeSelf);

        // Desbloquea el cursor para poder seleccionar
        if(menuRadial.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
        } else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
