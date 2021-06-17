using UnityEngine;

// ---------------------------------------------------
// NAME: EnemigoControlador.cs
// STATUS: WIP
// GAMEOBJECT: Enemigo
// DESCRIPTION: centro de control de todos los comandos del enemigo
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: centro de control de todos los comandos del enemigo
// ---------------------------------------------------

public class ControladorEntidad : MonoBehaviour
{
    #region Variables
    // Objeto enemigo
    public EntidadSO stats;

    // Componentes del enemigo
    public string nombre;
    ComponenteAnimacion animacion;
    ComponenteAtaque ataque;
    ComponenteVida vida;
    ComponenteMovimiento movimiento;

    [Header("Componentes disparo")]
    public Transform spawnerBalas;
    public GameObject balaObjeto;
    public Transform parteQueRota;

    public bool marcado;
    #endregion

    private void Awake()
    {
        nombre = stats.nombre;
        animacion = GetComponent<ComponenteAnimacion>();
        ataque = GetComponent<ComponenteAtaque>();
        vida = GetComponent<ComponenteVida>();
        movimiento = GetComponent<ComponenteMovimiento>();
    }

    public void ObjetivoEnRango()
    {
        movimiento.Parar();
        animacion.Bloqueado(true);
    }

    public void RecibeAtaque(Ataque ataque)
    {
        vida.RecibirAtaque(ataque);
    }

    public void Ralentizado(float cantidad, float tiempo)
    {
        movimiento.Ralentizado(cantidad, tiempo);
    }

    public void Debilitado(float cantidad, float tiempo)
    {
        ataque.Debilitado(cantidad, tiempo);
    }

    public void Caminar()
    {
        movimiento.Caminar();
        animacion.Bloqueado(false);
    }

    public void Muerte()
    {
        animacion.Destruido();
        Destroy(gameObject, 0.5f);
    }

    public void Disparar()
    {
        animacion.Dispara();
    }

    public void Explotar()
    {
        ataque.Explotar();
    }

    public void Marcar()
    {
        marcado = true;
    }

    public void Potenciado(bool estado, GameObject potenciador)
    {
        if(stats.tipoAtaque != EntidadSO.Tipo.potenciador)
        {
            ataque.Potenciado(estado, potenciador);
        }
    }

    public void AsignarBases(Base b1, Base b2, Base b3)
    {
        movimiento.AsignarBases(b1, b2, b3);
    }

}
