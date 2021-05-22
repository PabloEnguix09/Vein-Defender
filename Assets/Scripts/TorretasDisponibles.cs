using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ---------------------------------------------------
// NAME: TorretaDisponibles.cs
// STATUS: DONE
// GAMEOBJECT: ControlPartida
// DESCRIPTION: controla las listas de torretas disponibles, totales y en uso
//
// AUTHOR: Juan Ferrera Sala
// FEATURES ADDED: creamos el Script y pasamos a recibirTorretasYPreviews a invocar torretas.
//
// AUTHOR: Jorge Grau
// FEATURES ADDED: 
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: selector de torretas al completo
// ---------------------------------------------------

public class TorretasDisponibles : MonoBehaviour
{
    // 1,3,2,0
    // 1,2

    //Crear lista de torretas
    public List<GameObject> torretasTotales; //Camara,Inmortal,Balear,Scutum

    //Crear lista de preview torretas
    public List<GameObject> previewsTotales; //CamaraPrev,InmortalPrev,BalearPrev,ScutumPrev

    // Lista de imagenes de las torretas
    public List<Sprite> imagenesTotales;

    //Crear lista de torretas
    public List<GameObject> torretasDisponibles; //Inmortal,Balear

    //Crear lista de preview torretas
    public List<GameObject> previewsDisponibles; //InmortalPrev,BalearPrev

    //Torretas torretas elegidas
    public List<GameObject> torretasUso; //Inmortal,Balear

    //Torretas preview elegidas
    public List<GameObject> previewsUso; //InmortalPrev,BalearPrev,

    // Imagenes de las torretas elegidas
    public List<Sprite> imagenesUso;

    public InvocarTorreta invocarTorreta;
    public HUD hud;

    private void Start()
    {
        List<int> listaVacia = new List<int>();
        actualizarTorretasElegidas(listaVacia);
    }

    // Recibe una lista de indices que representan los indices de las torretas
    public void asignarTorretasDisponibles(List<int> listaIndices)
    {
        torretasDisponibles.Clear();
        previewsDisponibles.Clear();
        for (int i = 0; i < listaIndices.Count; i++)
        {
            // Asigna cada torreta segun su indice
            torretasDisponibles.Add(torretasTotales[listaIndices[i]]);
            previewsDisponibles.Add(previewsTotales[listaIndices[i]]);
        }
        // Ponemos la sparky siempre desbloqueada
        torretasDisponibles.Add(torretasTotales[0]);
        previewsDisponibles.Add(previewsTotales[0]);
    }

    // Se llama desde el HUD cuando se terminan de poner las torretas
    public void actualizarTorretasElegidas(List<int> listaIndices)
    {
        torretasUso.Clear();
        previewsUso.Clear();
        imagenesUso.Clear();

        // Se llena de torretas basicas
        for (int i = 0; i < 8; i++)
        {
            torretasUso.Add(torretasTotales[0]);
            previewsUso.Add(previewsTotales[0]);
            imagenesUso.Add(imagenesTotales[0]);
        }
        // Se sustituyen por las nuevas
        for (int i = 0; i < listaIndices.Count; i++)
        {
            // Asigna cada torreta, preview e imagen en uso segun su indice en las totales
            torretasUso[i] = torretasTotales[listaIndices[i]];
            previewsUso[i] = previewsTotales[listaIndices[i]];
            imagenesUso[i] = imagenesTotales[listaIndices[i]];
        }

        // Se envian al menu radial
        invocarTorreta.asignarTorretasActuales(torretasUso, previewsUso, imagenesUso);
    }
}