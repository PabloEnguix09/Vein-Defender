using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEM : MonoBehaviour
{
    public SphereCollider collider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Terreno"))
        {
            Destroy(gameObject);
        }
        if(other.gameObject.CompareTag("Enemigo"))
        {
            other.gameObject.GetComponent<Enemigo>().Ralentizar();
        }
    }
}
