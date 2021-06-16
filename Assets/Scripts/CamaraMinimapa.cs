using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ---------------------------------------------------
// NAME: CamaraMinimapa.cs
// STATUS: DONE
// GAMEOBJECT: CamaraMinimapa
// DESCRIPTION: Movimiento de la camara de minimapa
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: movimiento de la camara
// ---------------------------------------------------

public class CamaraMinimapa : MonoBehaviour
{

    Transform camara;

    public Transform marcadorPersonaje;

    // Start is called before the first frame update
    void Start()
    {
        camara = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(90, camara.rotation.eulerAngles.y, 0);

        marcadorPersonaje.rotation = Quaternion.Euler(0, 180 + camara.rotation.eulerAngles.y, 0);
    }
}
