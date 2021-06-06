// ---------------------------------------------------
// NAME: EnemigoBasico
// STATUS: WIP
// GAMEOBJECT: sistema
// DESCRIPTION: maneja la creacion de los objetos y sus datos
//
// AUTHOR: Luis
// FEATURES ADDED: Creado el Scriptable Object de los Enemigos, conjuntado entre Torretas, Enemigos y Bases
//
// AUTHOR: Pau
// FEATURES ADDED: añadidas las variables rango de disparo y velocidad de disparo
//
// AUTHOR: Jorge
// FEATURES ADDED: ataqueTemporal, ataqueFinal, inivisibilidad, subterraneo, vuela, atacaJugador, atacaTorretas
// ---------------------------------------------------
using UnityEngine;

[CreateAssetMenu(fileName = "Entidad", menuName = "Objetos/Entidad", order = 1)]
public class EntidadSO : ScriptableObject
{
    [Header("Ataque General")]
    public float rangoDeteccion;
    public bool atacaJugador;
    public bool atacaTorretas;
    public Tipo tipoAtaque;

    [Header("Vida")]
    public float vidaMaxima;
    public float escudoMaximo;
    public float escudoRegen;
    public float timerEscudo;
    public bool invisibilidad;
    public bool subterraneo;
    public float reduccionAtaque;
    OrientacionReduccionAtaque orientacionReduccionAtaque;

    [Header("Movimiento")]
    public bool vuela;
    public float velocidadMaxima;
    public Movimiento movimiento;

    [Header("Disparador")]
    public float ataqueDisparo;
    public float velocidadRotacion;
    public float rangoDisparo;
    public float velocidadDisparo;
    public Quaternion anguloDisparo;

    [Header("Explosivos")]
    public bool explosivo;
    public float rangoExplosion;
    public float ataqueExplosion;

    public enum Movimiento
    {
        terrestre, volador, subterraneo, torreta
    }

    public enum Tipo
    {
        potenciador, balas, laser, sinDisparo
    }

    public enum OrientacionReduccionAtaque
    {
        front, back, sides
    }
}
