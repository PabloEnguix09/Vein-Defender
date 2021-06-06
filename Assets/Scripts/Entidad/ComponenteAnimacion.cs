using UnityEngine;

// ---------------------------------------------------
// NAME: EnemigoAnimacion.cs
// STATUS: WIP
// GAMEOBJECT: Enemigo
// DESCRIPTION: Controla las animaciones del enemigo actual
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: Idle, Disparo, Bloquear en el sitio
// ---------------------------------------------------

public class ComponenteAnimacion : MonoBehaviour
{
    Animator animator;
    ControladorEntidad controlador;

    [Header("Explosivos")]
    public float explosionTimeOffset;
    public GameObject explosion;

    bool destruido = false;

    // Start is called before the first frame update
    void Start()
    {
        controlador = GetComponent<ControladorEntidad>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(destruido)
        {
            explosionTimeOffset -= Time.deltaTime;
            // El enemigo bomba instancia unas particulas de explosion
            if(explosionTimeOffset <= 0 && controlador.stats.explosivo)
            {
                // Particulas explosion
                Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            }
        }
    }

    public void Bloqueado(bool estado)
    {
        animator.SetBool("Bloqueado", estado);
    }

    public void Dispara()
    {
        animator.SetTrigger("Disparo");
    }

    public void Destruido()
    {
        animator.SetBool("Destruido", true);
        destruido = true;
    }
}
