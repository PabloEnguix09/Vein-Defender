using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticulas : MonoBehaviour
{
    // ---------------------------------------------------
    // NAME: ExplosionParticulas.cs
    // STATUS: WIP
    // GAMEOBJECT: Explosion
    // DESCRIPTION: Este archivo le pone un "tiempo de vida" a la explosion
    //
    // AUTHOR: Jorge Grau
    // FEATURES ADDED: La explosion se autodestruye al pasar una cantidad de tiempo
    // ---------------------------------------------------

    private float contador = 0;
    public float limite = 0;

    void Update()
    {
        if(contador > limite)
        {
            Destroy(gameObject);
        }
        contador += Time.deltaTime;
    }
}
