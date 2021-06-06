using UnityEngine;
// ---------------------------------------------------
// NAME: PotenciadorSO.cs
// STATUS: DONE
// GAMEOBJECT: ControladorEntidad
// DESCRIPTION: potenciador de enemigos o aliados
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: Traspaso del código y cambios básicos en la implementación
// ---------------------------------------------------

[CreateAssetMenu(fileName = "PotenciadorSO", menuName = "Componentes/PotenciadorSO")]
public class PotenciadorSO : ScriptableObject
{
    public void Potenciar(ControladorEntidad controlador, GameObject gameObject)
    {
        // Recogemos todos los objetivos de la zona
        GameObject[] enemigo = GameObject.FindGameObjectsWithTag("Enemigo");

        for (int i = 0; i < enemigo.Length; i++)
        {
            // Su tiene un enemigo en rango le da un buff
            if (Vector3.Distance(gameObject.transform.position, enemigo[i].transform.position) < controlador.stats.rangoDisparo)
            {
                enemigo[i].GetComponent<ControladorEntidad>().Potenciado(true, gameObject);
            }
            // Su tiene un enemigo fuera de rango se lo quita
            else if (Vector3.Distance(gameObject.transform.position, enemigo[i].transform.position) > controlador.stats.rangoDisparo)
            {
                enemigo[i].GetComponent<ControladorEntidad>().Potenciado(false, gameObject);
            }
        }
    }
}
