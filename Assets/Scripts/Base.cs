using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField]
    public float salud = 1;

    public float Salud
    {
        get { return salud; }

        set
        {
            value = Mathf.Clamp01(value);
            salud = value;
            if (salud <= 0)
            {
                Debug.Log("Destruido");
                //Destroy(gameObject);
            }
        }
    }
}
