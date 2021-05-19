using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: nombre
// STATUS: estado
// GAMEOBJECT: objeto
// DESCRIPTION: descripcion
//
// AUTHOR: autor
// FEATURES ADDED: cosas hechas
// ---------------------------------------------------


public class CameraController : MonoBehaviour
{
    // Velocidad de movimiento del rat�n
	public float sensibilidadMaxima = 1.0f;
    float sensibilidad;

    // Limites de la c�mara
	public float xMin = -60f;
	public float xMax = 60f;

    public Transform camara;

	private Quaternion rotacion;
    private RaycastHit impacto;

    private Vector3 offset;

    private void Start()
    {
        // Hacer que el cursor no aparezca
        Cursor.lockState = CursorLockMode.Locked;

        rotacion = transform.localRotation;
        offset = camara.localPosition;

        sensibilidad = sensibilidadMaxima;
    }

    private void Update()
    {
        // Coger los movimientos del rat�n (con la X invertida)
        rotacion.x += Input.GetAxis("Mouse Y") * sensibilidad * (-1);
        rotacion.y += Input.GetAxis("Mouse X") * sensibilidad;

        // Compara el movimiento del rat�n con los valores m�ximo y m�nimo dados arriba
        rotacion.x = Mathf.Clamp(rotacion.x, xMin, xMax);

        transform.localRotation = Quaternion.Euler(rotacion.x, rotacion.y, rotacion.z);

        // Detecta colisiones de la c�mara y la acerca al personaje
        if(Physics.Linecast(transform.position, transform.position + transform.localRotation * offset, out impacto))
        {
            camara.localPosition = new Vector3(0, 0, -Vector3.Distance(transform.position, impacto.point));
        }
        else
        {
            camara.localPosition = Vector3.Lerp(camara.localPosition, offset, Time.deltaTime);
        }
    }

    public void BloquearCamara(bool estado){
        if(estado)
        {
            Cursor.lockState = CursorLockMode.None;
            sensibilidad = 0;
        } else
        {
            Cursor.lockState = CursorLockMode.Locked;
            sensibilidad = sensibilidadMaxima;
        }
    }
}
