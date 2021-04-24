using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TorretaInmortal", menuName = "Objetos/Torreta", order = 2)]
public class TorretaInmortal : MonoBehaviour
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
