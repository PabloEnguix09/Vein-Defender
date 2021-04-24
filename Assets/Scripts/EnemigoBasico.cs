    // ---------------------------------------------------
// NAME: EnemigoBasico
// STATUS: WIP
// GAMEOBJECT: sistema
// DESCRIPTION: maneja la creacion de los objetos Enemigo y sus datos
//
// AUTHOR: Luis
// FEATURES ADDED: Creado el Scriptable Object de las torretas
// ---------------------------------------------------
using UnityEngine;

[CreateAssetMenu(fileName = "Enemigo", menuName = "Objetos/Enemigo", order = 1)]
public class EnemigoBasico : ScriptableObject
{
    public float ataque;
    public float vidaMaxima;
    public float velocidad;
    public float rango;
    public enum tipo
    {
        terrestre, volador, subterraneo
    }
}
