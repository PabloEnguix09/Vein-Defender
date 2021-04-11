using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  AUTHOR: Pablo Enguix Llopis
 *  STATUS: WIP
 *  NAME: CameraController.cs
 *  GAMEOBJECT: Camara (inside Jugador prefab)
 *  DESCRIPTION: This script is used to rotate the camera with the mouse and detect collisions with other objects to make the camera closer to the player
 */

public class CameraController : MonoBehaviour
{
    // Velocidad de movimiento del rat�n
	public float sensibilidad = 1.0f;

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
}
