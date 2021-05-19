using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ---------------------------------------------------
// NAME: Ataque.cs
// STATUS: DONE
// GAMEOBJECT: Ninguno
// DESCRIPTION: Objeto de ataque
//
// AUTHOR: Luis
// FEATURES ADDED: propiedades base
// ---------------------------------------------------
public class Ataque : ScriptableObject
{
    public float fuerza;
    public GameObject origen;
    public Tipo tipo;
    public enum Tipo
    {
        laser = 1,
        balas = 2
    }
    public float radioExplosion;
    public float fuerzaExplosion;
}
