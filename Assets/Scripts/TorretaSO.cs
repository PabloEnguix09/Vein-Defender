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
//
// AUTHOR: Jorge Grau
// FEATURES ADDED: antiaerea,invisibilidad,reducirDaño,frente,espalda,lados,reduccion
// ---------------------------------------------------

using UnityEngine;

[CreateAssetMenu(fileName = "TorretaBasica", menuName = "Objetos/Torreta", order = 1)]
public class TorretaSO : ScriptableObject
{
    public int energia;
    public int energiaAlt;
    public float vidaMaxima;
    public float ataque;
    public float cadenciaDisparo;
    public TipoDisparo tipoDisparo;
    public Quaternion anguloDisparo;
    public float velocidadRotacion;
    public float distanciaDisparo;
    public float radioExplosion;
    public float danyoExplosion;


    public Visual visual;
    public Variantes variantes;
    public ReduceDanyo reduceDanyo;
    public Escudo escudo;

    public enum TipoDisparo
    {
        laser = 1,
        balas = 2
    }
}
[System.Serializable]
public class Variantes
{
    public bool antiaerea;
    public bool invisibilidad;
    public bool regeneracion;
}
[System.Serializable]
public class Escudo
{
    public float escudoMaximo;
    public float escudoRegen;
}
[System.Serializable]
public class ReduceDanyo
{
    public bool reducirDanyo;
    public bool frente;
    public bool espalda;
    public bool lados;
    public float reduccion;
}
[System.Serializable]
public class Visual
{
    public string nombre;
    public Sprite imagen;
    public GameObject prefab;
    public GameObject previewPrefab;
}
