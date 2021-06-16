using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComportamientoMira : MonoBehaviour
{
    public controlPartida controlPartida;
    public GameObject camara;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit punto;

        if(Physics.Raycast(camara.transform.position, camara.transform.forward, out punto, 50, LayerMask.GetMask("Interactuable")))
        {
            
            Quaternion VisionRotacion = Quaternion.Euler(0, 0, 45);
            //rotacion suave
            Vector3 rotacion = Quaternion.Lerp(transform.rotation, VisionRotacion, Time.deltaTime * 10).eulerAngles;
            transform.rotation = Quaternion.Euler(rotacion.x, rotacion.y, rotacion.z);
            GetComponent<Image>().color = new Color(0, 170, 236, 126);
            controlPartida.Interactuar(true);
        }
        else
        {
            Quaternion VisionRotacion = Quaternion.Euler(0, 0, 0);
            //rotacion suave
            Vector3 rotacion = Quaternion.Lerp(transform.rotation, VisionRotacion, Time.deltaTime * 10).eulerAngles;
            GetComponent<Image>().color = new Color(255, 255, 255, 126); ;

            transform.rotation = Quaternion.Euler(rotacion.x, rotacion.y, rotacion.z);
            controlPartida.Interactuar(false);

        }
    }
}
