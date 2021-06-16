// ---------------------------------------------------
// NAME: Interaccion.cs
// STATUS: WIP
// GAMEOBJECT: objetos interactuables en el Layer = 
// DESCRIPTION: Este escript se implementa la posibilidad de interactuar con diversos elementos
//
// AUTHOR: Juan Ferrera Sala
// FEATURES ADDED: Activar el HUD y layer interactuables.
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: Interaccion con el HUD
// ---------------------------------------------------
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Interaccion : MonoBehaviour
{

    public GameObject hud;

    public TipoItem tipoItem;
    //esta variable guardará el animator
    Animator animatorNave;
    public GameObject nave;

    GameObject camaraTByte;
    public GameObject camaraSelector;
    public GameObject camaraPuerta;

    CameraController cameraController;

    public GameObject gui;
    public Image mira;
    public enum TipoItem
    {
        pcTorretas = 1
    }

    // Start is called before the first frame update
    void Start()
    {
        // Busca el camera controller
        cameraController = FindObjectOfType<CameraController>();

        // Lo desactiva al principio por si acaso
        hud.SetActive(false);

        animatorNave = nave.GetComponent<Animator>();

        camaraTByte = GameObject.Find("CamaraTByte");
        gui = GameObject.Find("HUD_Canvas");

    }

    public Interaccion Interactuar()
    {
        // Si es un PC de torretas interactua con el HUD de las torretas
        if(tipoItem == TipoItem.pcTorretas)
        {
            camaraSelector.SetActive(true);
            camaraTByte.SetActive(false);
            
            // Abre el menu
            hud.SetActive(true);
            gui.SetActive(false);

            // Cuando el HUD esta activado queremos tener la camara bloqueada
            cameraController.BloquearCamara(true);
            
            return this;
        }
        
        return null;
    }

    public void Cerrar()
    {
        if (tipoItem == TipoItem.pcTorretas)
        {
            // Actualiza las torretas en uso
            hud.GetComponent<HUD>().actualizarTorretasUso();
            // Cierra el menu
            hud.SetActive(false);

            
            //en caso de que la puerta ya haya sido abierta
            if (animatorNave.GetBool("tieneTorretas"))
            {
                //cambio de cámara
                camaraTByte.SetActive(true);
                camaraSelector.SetActive(false);

                //activar la gui y permitir el movimiento
                gui.SetActive(true);
                cameraController.BloquearCamara(false);
            }
            else
            {
                StartCoroutine("CamaraPuerta");
                abrirPuerta();
            }
            
            
        }
    }

    public void abrirPuerta()
    {
        animatorNave.SetBool("tieneTorretas", true);
    }


    IEnumerator CamaraPuerta()
    {
        //cambio de cámara a la puerta
        camaraPuerta.SetActive(true);
        camaraTByte.SetActive(false);
        camaraSelector.SetActive(false);

        //esperar a la animación
        yield return new WaitForSeconds(4);

        //cambio de cámara a t-byte
        camaraTByte.SetActive(true);
        camaraSelector.SetActive(false);
        camaraPuerta.SetActive(false);

        //esperar al cambio de cámara
        yield return new WaitForSeconds(2);

        //regresar el control al usuario
        gui.SetActive(true);
        cameraController.BloquearCamara(false);

    }
}
