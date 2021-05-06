// ---------------------------------------------------
// NAME: EnemigoBasico
// STATUS: WIP
// GAMEOBJECT: sistema
// DESCRIPTION: maneja la creacion de los objetos Enemigo y sus datos
//
// AUTHOR: Luis
// FEATURES ADDED: Creado el Scriptable Object de las Torreta
//
// AUTHOR: Pau
// FEATURES ADDED: añadidas las variables rango de disparo y velocidad de disparo
// ---------------------------------------------------
using UnityEngine;

[CreateAssetMenu(fileName = "Enemigo", menuName = "Objetos/Enemigo", order = 1)]
public class EnemigoBasico : ScriptableObject
{
    public float ataque;
    public float ataqueExplosion;
    public float rangoExplosion;
    public float velocidadDisparo;
    public float vidaMaxima;
    public float velocidad;
    public float velocidadDeRotacion;
    public float rango;
    public float rangoDisparo;
    public bool invisibilidad;
    public bool subterraneo;
    public bool vuela;

    public enum tipo
    {
        terrestre, volador, subterraneo
    }
}
