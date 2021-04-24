using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparo : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: Disparo.cs
    // STATUS: WIP
    // GAMEOBJECT: Disparo
    // DESCRIPTION: descripcion
    //
    // AUTHOR: Adrian
    // FEATURES ADDED: Se asigna el daño de la torreta a la bala y la posiciona apuntando hacia donde apunta la torreta
    // ---------------------------------------------------

    public GameObject bala;

    public void Disparar(float ataque, float radioExplosion,float danyoEplosion)
    {
        //asignar a la bala el daño de la torreta
        bala.GetComponent<Bala>().fuerza = ataque;
        bala.GetComponent<Bala>().radioExplosion =radioExplosion;
        bala.GetComponent<Bala>().danyoExplosion = danyoEplosion;
        //Generar la bala apuntando el la direccion que apunta la torreta
        Instantiate(bala, transform.position, transform.rotation);
        Debug.Log("pium");
    }
}
