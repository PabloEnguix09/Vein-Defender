using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{

    public float velocidad;

    [Range(0, 1)]
    [SerializeField]
    public float fuerza;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        velocidad = 100f;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * velocidad, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
