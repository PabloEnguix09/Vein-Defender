using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    private float adelante;
    private float derecha;
    private Vector3 velocidad;

    public float alcance = 12.5f;

    public CameraController camara;
    public MovimientoPersonaje personaje;

    public void Mover(float adelante, float derecha)
    {
        this.adelante = adelante;
        this.derecha = derecha;

        Vector3 objetivo = adelante * camara.transform.forward;
        objetivo += derecha * camara.transform.right;
        objetivo.y = 0;

        if (objetivo.magnitude > 0)
        {
            velocidad = objetivo;
        }
        else
        {
            velocidad = Vector3.zero;
        }
        personaje.Velocidad = objetivo;
    }

    public void Correr()
    {
        if(personaje.GetMovimientos() != MovimientoPersonaje.Movimientos.Correr)
        {
            personaje.SetMovimientos(MovimientoPersonaje.Movimientos.Correr);
        }
        else
        {
            personaje.SetMovimientos(MovimientoPersonaje.Movimientos.Caminar);
        }
    }

    // SOLO PARA LAS ESCENAS: Muestra el rayo de apuntado
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * alcance);
    }
}
