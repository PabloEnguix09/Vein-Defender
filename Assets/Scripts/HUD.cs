using System.Collections.Generic;
using UnityEngine;
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

    // Imagenes de preview de cada torreta para controlar su aspecto en el HUD
    public List<GameObject> imagenes;

    ItemSlot[] itemSlots;
    TorretasDisponibles torretasDisponibles;

    public Personaje personaje;

    private void Start()
    {
        // Encuentra todos los ItemSlot
        itemSlots = FindObjectsOfType<ItemSlot>();

        torretasDisponibles = FindObjectOfType<TorretasDisponibles>();

        // se llama la primera vez para no tener un array vacio en InvocarTorreta.cs
        actualizarTorretasUso();
    }

    public void actualizarTorretasUso()
    {
        // Se asigna el indice de cada slot
        for (int i = 0; i < itemSlots.Length; i++)
        {
            indicesElegidos[i] = itemSlots[i].indiceTorretaActual;
        }
        // Se envian los indices del menu a las torretas disponibles
        torretasDisponibles.actualizarTorretasElegidas(indicesElegidos);
    }

    public void OnClickCerrar()
    {
        Debug.Log("Cierra");
        personaje.CerrarInteraccion();
    }
}
