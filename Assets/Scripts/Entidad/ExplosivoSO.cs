using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplosivoSO", menuName = "Componentes/ExplosivoSO")]
public class ExplosivoSO : ScriptableObject
{
    public void Explotar(ControladorEntidad controlador, float fuerza, GameObject gameObject)
    {
        // Impacto de explosion y objetivos afectados
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, controlador.stats.rangoExplosion);

        Ataque ataqueObjeto = ScriptableObject.CreateInstance<Ataque>();

        ataqueObjeto.fuerza = fuerza;
        ataqueObjeto.tipo = Ataque.Tipo.balas;
        ataqueObjeto.origen = gameObject;

        // Hace un ataque contra todos los objetivos dentro del rango
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Base"))
            {
                Base estructura = colliders[i].gameObject.GetComponent<Base>();
                estructura.RecibirAtaque(ataqueObjeto);
            }

            else if (colliders[i].CompareTag("Torreta"))
            {
                Torreta estructura = colliders[i].gameObject.GetComponent<Torreta>();

                estructura.RecibirAtaque(ataqueObjeto);
            }

            else if (colliders[i].CompareTag("Enemigo"))
            {
                ControladorEntidad otroEnemigo = colliders[i].gameObject.GetComponent<ControladorEntidad>();
                otroEnemigo.RecibeAtaque(ataqueObjeto);
            }

            else if (colliders[i].CompareTag("Player"))
            {
                Personaje personaje = colliders[i].gameObject.GetComponent<Personaje>();
                personaje.RecibirAtaque(ataqueObjeto);
            }
        }

        controlador.Muerte();
    }
}
