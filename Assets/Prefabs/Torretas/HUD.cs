using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public List<int> indices;//Torretas disponibles los valores son iguales al de las imagenes es decir el indice 0 corresponde a la camara, la imagen de la camara es 0
    public List<int> indicesElegidos;//Torretas disponibles los valores son iguales al de las imagenes es decir el indice 0 corresponde a la camara, la imagen de la camara es 0
    public List<GameObject> imagenes;
    public Personaje personaje;
    public TorretasDisponibles torretasDisponibles;
    private int contador = 0;

    private void Update()
    {
        /*
        if(personaje.transform.position.y < 120f && contador == 0)
        {
            contador = 1;
            
            
            for (int i = 0; i < indices.Count; i++)
            {
                Debug.Log("Posicion " + i + " idTorreta " + indices[i].ToString());
            }

            torretasDisponibles.recibirInd(indicesElegidos);
        }
        */
    }
    public void recibirIndices(List<int> index)
    {
        indices = index;

        Image imagen = imagenes[0].GetComponent<Image>();

        imagenes[0].GetComponent<Image>().color = new Color(imagen.color.r, imagen.color.g, imagen.color.b, 0.5f);

        // 0,2,3,4


        for (int i = 0; i < 8; i++)
        {
            indicesElegidos.Add(0);
        }

    }

    public void recibirMovimientos(string slot, string torretaIndice)
    {
        Debug.Log(slot + " " + torretaIndice);

        //0,0,0,5,0,0,0,0,0,0,0
        indicesElegidos[int.Parse(slot)] = int.Parse(torretaIndice);
        /*
        for(int i = 0; i < indicesElegidos.Count; i++)
        {
            Debug.Log("Posicion " + i + " idTorreta " + indicesElegidos[i].ToString());
        }
        */
    }
}
