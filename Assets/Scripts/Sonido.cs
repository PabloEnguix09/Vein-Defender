using UnityEngine.Audio;
using UnityEngine;

// ---------------------------------------------------
// NAME: Sonido.cs
// STATUS: WIP
// GAMEOBJECT: None
// DESCRIPTION: Objeto para guardar los datos de un sonido
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: 
// ---------------------------------------------------
[System.Serializable]
public class Sonido
{
    public AudioClip clip;

    public string nombre;
    public int indice;

    [Range(0f,1f)]
    public float volumen;
    [Range(.1f, 3)]
    public float tono;

    [HideInInspector]
    public AudioSource origen;
}
