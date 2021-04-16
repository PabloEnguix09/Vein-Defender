// ---------------------------------------------------
// NAME: TorretaBasica
// STATUS: WIP
// GAMEOBJECT: sistema
// DESCRIPTION: maneja la creacion de los objetos Torreta y sus datos
//
// AUTHOR: Luis
// FEATURES ADDED: Creado el Scriptable Object de las torretas
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
    public enum tipoDisparo
    {
        laser, balas
    }
    public Quaternion anguloDisparo;
    public float velocidadRotacion;
    public float distanciaDisparo;
    public bool antiaerea;
}
