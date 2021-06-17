using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerGrupoEnemigos : MonoBehaviour
{
    int cantidad = 0;
    AudioHandler audioHandler;

    // Start is called before the first frame update
    void Start()
    {
        audioHandler = GetComponent<AudioHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        cantidad = transform.childCount;

        if(cantidad > 1)
        {
            audioHandler.Play(0);
        } else if(cantidad == 1)
        {
            audioHandler.Play(1);
        } else
        {
            audioHandler.PauseAll();
        }
    }
}
