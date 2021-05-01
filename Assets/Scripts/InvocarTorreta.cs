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
// FEATURES ADDED: Arreglos de energía, EliminarTorreta(), menu radial, cambios en la rotacion de invocacion, mejoras de torreta
// ---------------------------------------------------


public class InvocarTorreta : MonoBehaviour
{
    public GameObject[] torretas;
    public GameObject[] previews;
    public float alcance = 50.0f;
    public float alturaSpawn = 50.0f;

    public GameObject torreta;
    private Rigidbody rb;
    private bool colocada = true;

    public GameObject menuRadial;
    private ComprobarSitio sitio;
    public int torretaPreviewIndex = 0;

    [Header("Areas Menu Radial")]
    public float[] areasMenuRadial;

    public bool GetColocada()
    {
        return colocada;
    }

    public void SetColocada(bool value)
    {
        colocada = value;
    }

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
            if (Input.GetAxisRaw("Fire1") > 0)
            {
                if (PosicionLegal())
                {
                    // Posicion de la nueva torreta invocada
                    Transform torretaSpawn = torretas[torretaPreviewIndex].transform;
                    torretaSpawn.position = torreta.transform.position;
                    torretaSpawn.rotation = torreta.transform.rotation;
                    Destroy(torreta);
                    // Datos para la nueva torreta invocada
                    torreta = null;
                    SpawnTorreta(torretaSpawn);
                }
            }

            // Si pulsa C se destruye la previsualizacion
            if (Input.GetAxisRaw("Cancelar") > 0)
            {
                Destroy(torreta);
                torreta = null;
                SetColocada(true);
                return;
            }
        }
    }

    private bool PosicionLegal()
    {
        return sitio.colliders.Count <= 0;
    }

    public void PreviewTorreta()
    {
        torreta = ((GameObject)Instantiate(previews[torretaPreviewIndex]));
        sitio = torreta.GetComponent<ComprobarSitio>();
        rb = torreta.GetComponent<Rigidbody>();
        rb.mass = 0f;
    }

    public void SpawnTorreta(Transform torreta)
    {
        SetColocada(true);
        GameObject aux;
        aux = Instantiate(torreta.gameObject, new Vector3(torreta.position.x, torreta.position.y + alturaSpawn, torreta.position.z), torreta.rotation);
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

    public void AlternarMenuRadial(bool activar)
    {
        // Si esta desactivado se activa y viceversa
        menuRadial.SetActive(activar);

        // Desbloquea el cursor para poder seleccionar
        if(menuRadial.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        // Se cierra el menu
        else
        {
            // Donde esta el raton en pantalla se selecciona esa torreta del area
            // Devuelve la distancia del raton del centro de la pantalla
            Vector2 centroPantalla = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // Debug.Log(Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0));
            // angulo = atan2(Y - CenterY, X - CenterX)
            float angulo = Mathf.Atan2(mousePosition.y - centroPantalla.y, mousePosition.x - centroPantalla.x);
            //Debug.Log(angulo);

            torretaPreviewIndex = ComprobarCasillaMenu(angulo);

            if(torretaPreviewIndex >= 0)
            {
                //Debug.Log("Torreta invocada: " + torretaPreviewIndex);
                // Se previsualiza la torreta 
                SetColocada(false);
                PreviewTorreta();
            }

            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Comprueba si el raton esta dentro de una casilla del menu y devuelve la casilla
    public int ComprobarCasillaMenu(float angulo)
    {
        for (int i = 0; i < areasMenuRadial.Length; i++)
        {
            // Solo si ha llegado al final, toma el 0 como referencia siguiente
            if(i + 1 == areasMenuRadial.Length)
            {
                return i;
            }
            else
            {
                if (areasMenuRadial[i] < areasMenuRadial[i + 1])
                {
                    if (angulo > areasMenuRadial[i] && angulo < areasMenuRadial[i + 1])
                    {
                        return i;
                    }
                }
            }
        }
        return -1;
    }
}
