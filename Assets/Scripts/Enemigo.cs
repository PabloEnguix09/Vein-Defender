using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemigo : MonoBehaviour
{
    private Base base1;
    private Base base2;
    private Base base3;
    public static Transform final;
    public NavMeshAgent agente;
    private Transform objetivo;


    [Range(0, 1)]
    [SerializeField]
    public float vida;

    public float Vida
    {
        get { return vida; }

        set
        {
            value = Mathf.Clamp01(value);
            vida = value;
        }
    }

    [Range(0, 1)]
    [SerializeField]
    public float fuerza;

    public float Fuerza
    {
        get { return fuerza; }

        set
        {
            value = Mathf.Clamp01(value);
            fuerza = value;
        }
    }

    public float velocidad;

    public float Velocidad
    {
        get { return velocidad; }

        set
        {
            value = Mathf.Clamp01(value);
            velocidad = value;
        }
    }

    public float vision;

    public float Vision
    {
        get { return vision; }

        set
        {
            value = Mathf.Clamp01(value);
            vision = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {

        agente = GetComponent<NavMeshAgent>();

        if (base1.Salud > 0)
        {
            agente.destination = base1.transform.position;
        }

        else if (base2.Salud > 0)
        {
            agente.destination = base2.transform.position;
        }

        else if (base3.Salud > 0)
        {
            agente.destination = base3.transform.position;
        }

        else
        {
            agente.destination = final.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {

        objetivo = BuscarObjetivo();

        agente.destination = objetivo.position;

    }

    public void AsignarBases(Base base1, Base base2, Base base3)
    {
        this.base1 = base1;
        this.base2 = base2;
        this.base3 = base3;

    }

    Transform BuscarObjetivo()
    {
        Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, vision);

        for (int i = 0; i < colliders.Length; i++)
        {

            if (colliders[i].CompareTag("Player"))
            {
                return objetivo = colliders[i].transform;
            }

            else if (colliders[i].CompareTag("Torretas"))
            {
                return objetivo = colliders[i].transform;
            }
        }

        if (base1.Salud > 0)
        {
            return objetivo = base1.transform;
        }

        else if (base2.Salud > 0 && base1.Salud <= 0)
        {
            return objetivo = base2.transform;
        }

        else if (base3.Salud > 0 && base1.Salud <= 0 && base2.Salud <= 0)
        {
            return objetivo = base3.transform;
        }

        else
        {
            return objetivo = final.transform;
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<Bala>() != null)
        {
            Bala bala = other.gameObject.GetComponent<Bala>();
            vida -= bala.fuerza;
            Debug.Log(other.name);
            Destroy(other.gameObject);
        }
    }

}
