using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComportamientoMira : MonoBehaviour
{
    public controlPartida controlPartida;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit punto;

        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out punto, 1000, LayerMask.GetMask("Interactuable")))
        {
            controlPartida.Interactuar(true);
            transform.rotation = Quaternion.Euler(0,0,45);
        }
        else
        {
            controlPartida.Interactuar(false);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
