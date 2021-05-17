using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// ---------------------------------------------------
// NAME: HUD.cs
// STATUS: WIP
// GAMEOBJECT: HUD_torretas
// DESCRIPTION: Manager del selector de torretas
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: propiedades base
// ---------------------------------------------------

public class HUD : MonoBehaviour
{
    // Torretas disponibles los valores son iguales al de las imagenes es decir el indice 0 corresponde a la camara, la imagen de la camara es 0
    // Indices de las torretas
    public List<int> indicesTorretas;
    // Indices actuales en el circulo de seleccion
    public List<int> indicesElegidos;

    // Imagenes de preview de cada torreta
    public List<GameObject> imagenes;
    ItemSlot[] itemSlots;
    public Personaje personaje;
    public TorretasDisponibles torretasDisponibles;
    private int contador = 0;

    private void Start()
    {
        // Encuentra todos los ItemSlot
        itemSlots = FindObjectsOfType<ItemSlot>();
    }
}
