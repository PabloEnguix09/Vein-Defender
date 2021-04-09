using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticulas : MonoBehaviour
{
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
