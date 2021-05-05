using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ataque : ScriptableObject
{
    public float fuerza;
    public GameObject origen;
    public Tipo tipo;
    public enum Tipo
    {
        laser, balas
    }
    public float radioExplosion;
    public float fuerzaExplosion;
}
