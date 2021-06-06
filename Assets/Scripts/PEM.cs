using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEM : MonoBehaviour
{
    public float tiempoRalentizacion;
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
            Ataque ataque = new Ataque();
            ataque.tipo = Ataque.Tipo.pem;
            ataque.ralentizacion = tiempoRalentizacion;
            other.gameObject.GetComponent<ControladorEntidad>().RecibeAtaque(ataque);
        }
    }
}
