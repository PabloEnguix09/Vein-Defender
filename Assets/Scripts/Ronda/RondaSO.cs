using UnityEngine;

// ---------------------------------------------------
// NAME: RondaSO
// STATUS: WIP
// GAMEOBJECT: sistema
// DESCRIPTION: maneja la creacion de los objetos y sus datos
//
// AUTHOR: Jorge
// FEATURES ADDED: Creado el Scriptable Object de las Rondas
// ---------------------------------------------------

[CreateAssetMenu(fileName = "Ronda", menuName = "Objetos/Ronda", order = 1)]
public class RondaSO : ScriptableObject
{
    [Header("General")]

    public float radioSpawn;

    [Header("Primer Enemigo")]
    public int limitePrimerEnemigo;
    public float tiempoAparicionPrimerEnemigo;
    public GameObject primerTipoDeEnemigo;

    [Header("Segundo Enemigo")]
    public int limiteSegundoEnemigo;
    public float tiempoAparicionSegundoEnemigo;
    public GameObject segundoTipoDeEnemigo;

    [Header("Tercer Enemigo")]
    public int limiteTercerEnemigo;
    public float tiempoAparicionTercerEnemigo;
    public GameObject tercerTipoDeEnemigo;

}
