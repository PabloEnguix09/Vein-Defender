using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: CajaDrop.cs
// STATUS: WIP
// GAMEOBJECT: Caja Drop
// DESCRIPTION: controla el comportamiento de la caja de drops
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: invocacion de torreta y disolver la caja
//
// AUTHOR: Adrian
// FEATURES ADDED: parabola al invocar
// ---------------------------------------------------
public class CajaDrop : MonoBehaviour
{
    public GameObject torreta;
    public Transform centro;
    Animator animator;
    Rigidbody rb;

    public AnimationCurve curve;
    [Range(0,1)]
    public float velocidadCaida;
    public Vector3 final;
    public Vector3 inicio;
    float time;

    bool invocando;

    AudioHandler audioHandler;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        time = 0;

        audioHandler = GetComponent<AudioHandler>();
        invocando = false;
    }

    void Update()
    {
        //el multiplicador es para la velocidad de caida
        time += Time.deltaTime* velocidadCaida;
        Vector3 pos = Vector3.Lerp(inicio, final, time);
        //cambia la altitud en relación a la curva
        pos.y = inicio.y + ((final.y - inicio.y) * curve.Evaluate(time));
        transform.position = pos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Terreno")
        {
            if (!invocando)
            {
                invocando = true;
                // Se paraliza la caja
                rb.constraints = RigidbodyConstraints.FreezeAll;
                rb.detectCollisions = false;
                // Se pone la variable en el animator para disolver la caja
                animator.SetBool("Disolver", true);
                // Se invoca la torreta correspondiente en el sitio
                GameObject aux = Instantiate(torreta.gameObject, centro.position, transform.rotation);
                // Activa la torreta
                if (aux.GetComponent<Torreta>())
                {
                    aux.GetComponent<Torreta>().enabled = true;
                }

                audioHandler.Play(0);
                // Se destruye en un segundo
                Destroy(gameObject, 1);
            }
            
        }

        
    }

    

}
