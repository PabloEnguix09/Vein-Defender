// ---------------------------------------------------
// NAME: EnemigoBasico
// STATUS: WIP
// GAMEOBJECT: sistema
// DESCRIPTION: maneja la creacion de los objetos Enemigo y sus datos
//
// AUTHOR: Luis
// FEATURES ADDED: Creado el Scriptable Object de los Enemigos
//
// AUTHOR: Pau
// FEATURES ADDED: añadidas las variables rango de disparo y velocidad de disparo
//
// AUTHOR: Jorge
// FEATURES ADDED: ataqueTemporal, ataqueFinal, inivisibilidad, subterraneo, vuela, atacaJugador, atacaTorretas
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
    public float velocidadMaxima;
    public float velocidadDeRotacion;
    public float rango;
    public float rangoDisparo;
    public bool invisibilidad;
    public bool subterraneo;
    public bool vuela;
    public bool atacaJugador;
    public bool atacaTorretas;

    public enum tipo
    {
        terrestre, volador, subterraneo
    }
}
