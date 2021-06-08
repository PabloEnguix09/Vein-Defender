using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ---------------------------------------------------
// NAME: InvocarTorreta.cs
// STATUS: WIP
// GAMEOBJECT: Jugador
// DESCRIPTION: maneja la invocacion de torretas y el menu radial de seleccion
//
// AUTHOR: Pablo Enguix Llopis
// FEATURES ADDED: cosas hecha
//
// AUTHOR: Jorge Grau
// FEATURES ADDED: Comprobación de energia y gasto de energia.
// 
// AUTHOR: Luis Belloch
// FEATURES ADDED: Arreglos de energía, EliminarTorreta(), menu radial, cambios en la rotacion de invocacion, mejoras de torreta
// 
// AUTHOR: Adrian Maldonado
// FEATURES ADDED: Comprobacion de que no se esta invocando una torreta sobre otra
// ---------------------------------------------------


public class InvocarTorreta : MonoBehaviour
{
    public List<GameObject> torretas;
    public List<GameObject> previews;
    List<Sprite> imagenes;
    public float alcance = 50.0f;
    public float alturaSpawn = 50.0f;

    public GameObject torreta;
    public GameObject caja;
    Rigidbody rb;
    bool colocada = true;

    public GameObject menuRadial;
    public List<Image> imagenesMenuRadial;
    ComprobarSitio sitio;
    public int torretaPreviewIndex = 0;

    [Header("Areas Menu Radial")]
    public float[] areasMenuRadial;

    AnimTByte animTByte;
    public bool GetColocada()
    {
        return colocada;
    }

    public void SetColocada(bool value)
    {
        colocada = value;
    }

    private void Start()
    {
        // Script de control de las animaciones
        animTByte = GetComponent<AnimTByte>();
    }
    // Se llama cuando se actualiza el menu HUD de seleccion de torretas antes de empezar a jugar
    // Asigna las torretas en uso al menu radial
    public void asignarTorretasActuales(List<GameObject> torretasUso, List<GameObject> previewUso, List<Sprite> imagenesUso)
    {
        torretas = torretasUso;
        previews = previewUso;
        imagenes = imagenesUso;
        // Se asignan las imagenes al menu radial
        for (int i = 0; i < torretas.Count; i++)
        {
            imagenesMenuRadial[i].sprite = imagenes[i];
        }
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
        //Comprobar que no se esta invocando una torreta en esa posicion
        RaycastHit hit;
        Physics.Raycast(torreta.transform.position, torreta.transform.up, out hit, Mathf.Infinity);
        
        if (hit.collider != null)
        {
            //En caso de haber una torreta arriba
            return false;

        }

        return sitio.colliders.Count <= 0;
    }

    public void PreviewTorreta()
    {
        torreta = ((GameObject)Instantiate(previews[torretaPreviewIndex]));
        sitio = torreta.GetComponent<ComprobarSitio>();
        rb = torreta.GetComponent<Rigidbody>();
        rb.mass = 0f;
    }

    // Crea la torreta invocada a una altura alturaSpawn
    public void SpawnTorreta(Transform torreta)
    {
        SetColocada(true);
        // Invocamos una caja con los datos de la torreta dentro
        GameObject aux;

        Vector3 nave = new Vector3(18.6f, 135, -17.4f);

        aux = Instantiate(caja.gameObject, nave, torreta.rotation);
        if (aux.GetComponent<CajaDrop>())
        {

            aux.GetComponent<CajaDrop>().inicio = nave;
            aux.GetComponent<CajaDrop>().final = torreta.position;

            animTByte.InvocarTorreta();
            aux.GetComponent<CajaDrop>().torreta = torreta.gameObject;
        }
        /*
        GameObject aux;
        aux = Instantiate(torreta.gameObject, new Vector3(torreta.position.x, torreta.position.y + alturaSpawn, torreta.position.z), torreta.rotation);
        if(aux.GetComponent<Torreta>())
        {
            aux.GetComponent<Torreta>().enabled = true;
        }
        */
    }

    // Llamado desde MecanicasPersonaje.cs
    public void EliminarTorreta()
    {
        RaycastHit punto;

        // Comprueba que este apuntando a una torreta en el Layer Scutum
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out punto, alcance, LayerMask.GetMask("Scutum")))
        {
            // Toma la torreta del RaycastHit
            GameObject torretaMarcada = punto.transform.gameObject;
            // Destruye la torreta
            torretaMarcada.GetComponent<Scutum>().DestruirScutum();
        }

        // Comprueba que este apuntando a una torreta en el Layer Torreta
        else if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out punto, alcance, LayerMask.GetMask("Torreta"))) {
            // Toma la torreta del RaycastHit
            GameObject torretaMarcada = punto.transform.gameObject;

            //Si es un muro de energia no lo destruye
            if (!torretaMarcada.GetComponent<Torreta>().nombre.Equals("Scutum"))
            {
                // Destruye la torreta
                torretaMarcada.GetComponent<Torreta>().DestruirTorreta();

            }
        }


    }

    public void AlternarMenuRadial(bool activar)
    {
        // Si esta desactivado se activa y viceversa
        menuRadial.SetActive(activar);
        animTByte.SeleccionDeTorreta(true);
        // Se cierra el menu
        if (!menuRadial.activeSelf)
        {
            // Donde esta el raton en pantalla se selecciona esa torreta del area
            // Devuelve la distancia del raton del centro de la pantalla
            Vector2 centroPantalla = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // Formula: angulo = atan2(Y - CenterY, X - CenterX)
            float angulo = Mathf.Atan2(mousePosition.y - centroPantalla.y, mousePosition.x - centroPantalla.x);
            animTByte.SeleccionDeTorreta(false);
            torretaPreviewIndex = ComprobarCasillaMenu(angulo);
            // BUG: si al cargar la escena el menu radial esta abierto torretaPreviewIndex da error
            if(torretaPreviewIndex >= 0)
            {
                // Se previsualiza la torreta 
                SetColocada(false);
                PreviewTorreta();
            }
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
                // Comprueba la casilla donde ha caido el raton y devuelve su numero
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
