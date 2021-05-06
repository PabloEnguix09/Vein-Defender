// ---------------------------------------------------
// NAME: TorretaBasica
// STATUS: WIP
// GAMEOBJECT: sistema
// DESCRIPTION: maneja la creacion de los objetos Torreta y sus datos
//
// AUTHOR: Luis
// FEATURES ADDED: Creado el Scriptable Object de las Torreta
//
// AUTHOR: Pau Blanes
// FEATURES ADDED: escudoActual
// ---------------------------------------------------

using UnityEngine;

[CreateAssetMenu(fileName = "TorretaBasica", menuName = "Objetos/Torreta", order = 1)]
public class TorretaBasica : ScriptableObject
{
    public string nombre;
    public int energia;
    public int energiaAlt; 
    public float vidaMaxima;
    public float escudoMaximo;
    public float escudoRegen;
    public float ataque;
    public float cadenciaDisparo;
    public float escudoActual;
    public TipoDisparo tipoDisparo;
    public enum TipoDisparo
    {
        laser = 1,
        balas = 2
    }
    public Quaternion anguloDisparo;
    public float velocidadRotacion;
    public float distanciaDisparo;
    public bool antiaerea;
    public bool invisibilidad;
    public float radioExplosion;
    public float danyoExplosion;
}
