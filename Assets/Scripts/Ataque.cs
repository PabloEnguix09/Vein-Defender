using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: Ataque
// STATUS: WIP
// GAMEOBJECT: sistema
// DESCRIPTION: maneja el da�o
//
// AUTHOR: Luis
// FEATURES ADDED: Creado el Scriptable Object de Ataque
//
// AUTHOR: Jorge Grau
// FEATURES ADDED: direccion
// ---------------------------------------------------
public class Ataque : ScriptableObject
{
    public float fuerza;
    public GameObject origen;
    public Tipo tipo;
    public float direccion;
    public enum Tipo
    {
        laser = 1,
        balas = 2
    }
    public float radioExplosion;
    public float fuerzaExplosion;
}
