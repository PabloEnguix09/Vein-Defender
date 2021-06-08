using UnityEngine;
// ---------------------------------------------------
// NAME: PotenciadorSO.cs
// STATUS: DONE
// GAMEOBJECT: ControladorEntidad
// DESCRIPTION: potenciador de enemigos o aliados
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: Traspaso del c�digo y cambios b�sicos en la implementaci�n
// ---------------------------------------------------


public class PotenciadorSO
{
    public void Potenciar(ControladorEntidad controlador, GameObject gameObject)
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, controlador.stats.rangoDisparo);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.CompareTag("Enemigo"))
            {
                hitCollider.GetComponent<ControladorEntidad>().Potenciado(true, gameObject);
            }
        }
    }
}
