using System.Collections.Generic;
using UnityEngine;
// ---------------------------------------------------
// NAME: InvocadorSO.cs
// STATUS: DONE
// GAMEOBJECT: Madre
// DESCRIPTION: El enemigo Madre genera enemigos
//
// AUTHOR: Jorge
// FEATURES ADDED: Configuración y funcionalidad de madre al completo
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: Configuración y funcionalidad de madre al completo
// ---------------------------------------------------
public class InvocadorSO
{
    public void Invocar(Base base1, Base base2, Base base3, List<GameObject> objetos, float radioAparicion, Transform transform)
    {
        int xPos1 = (int)(transform.position.x - radioAparicion);
        int xPos2 = (int)(transform.position.x + radioAparicion);
        float xPos = buscarPunto(xPos1, xPos2);
        int zPos1 = (int)(transform.position.z - radioAparicion);
        int zPos2 = (int)(transform.position.z + radioAparicion);
        float zPos = buscarPunto(zPos1, zPos2);

        // Escoge un enemigo de la lista
        int numeroEnemigo = Random.Range(0, objetos.Count);
        /*
        // Se crea el enemigo y se le asignan las bases
        GameObject creado = Instantiate(objetos[numeroEnemigo], new Vector3(xPos, 1, zPos), Quaternion.identity);
        ControladorEntidad enemigoCreado = creado.GetComponent<ControladorEntidad>();
        enemigoCreado.AsignarBases(base1, base2, base3);
        */
    }

    // Busca un punto lo suficientemente alejado de ella para que no choquen los colliders
    int buscarPunto(int Pos1, int Pos2)
    {
        int Pos = 0;
        while (Pos < 17 && Pos > -17)
        {
            Pos = Random.Range(Pos1, Pos2);
        }
        return Pos;
    }
}
