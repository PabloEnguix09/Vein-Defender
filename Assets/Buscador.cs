using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buscador : MonoBehaviour
{
    public AudioListener[] listener;

    // Start is called before the first frame update
    void Start()
    {
        listener = FindObjectsOfType<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
