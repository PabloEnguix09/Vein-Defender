using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camino : MonoBehaviour
{
    public Color color;
    private List<Transform> nodos = new List<Transform>();
    public LineRenderer linea;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;

        Transform[] camino = GetComponentsInChildren<Transform>();
        nodos = new List<Transform>();

        for (int i = 0; i < camino.Length; i++)
        {
            if(camino[i] != transform)
            {
                nodos.Add(camino[i]);
            }
        }

        for(int i = 1; i < nodos.Count; i++) 
        {
            Vector3 nodoActual = nodos[i].position;
            Vector3 nodoAnterior = nodos[0].position;
            if(i>0) 
            {
                nodoAnterior = nodos[i - 1].position;
            }
            //SOLO PARA ESCENAS
            Gizmos.DrawLine(nodoAnterior, nodoActual);
        }
        linea.positionCount = nodos.Count;
        for(int i = 0; i < nodos.Count; i++)
        {
            linea.SetPosition(i, nodos[i].position);
        }
        linea.startWidth = 0.5f;
        linea.endWidth = 0.5f;
    }
}
