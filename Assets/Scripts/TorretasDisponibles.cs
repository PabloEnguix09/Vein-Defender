using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------
// NAME: TorretaDisponibles.cs
// STATUS: WIP
// GAMEOBJECT: objeto
// DESCRIPTION: descripcion
//
// AUTHOR: Juan Ferrera Sala
// FEATURES ADDED: creamos el Script y pasamos a recibirTorretasYPreviews a invocar torretas.
//
// AUTHOR: Jorge Grau
// FEATURES ADDED: 
// ---------------------------------------------------

public class TorretasDisponibles : MonoBehaviour
{
    // 1,3,2,0
    // 1,2

    //Crear lista de torretas
    public List<GameObject> torretasTotales; //Camara,Inmortal,Balear,Scutum

    //Crear lista de preview torretas
    public List<GameObject> previewsTotales; //CamaraPrev,InmortalPrev,BalearPrev,ScutumPrev

    //Crear lista de torretas
    public List<GameObject> torretasDisponibles; //Inmortal,Balear

    //Crear lista de preview torretas
    public List<GameObject> previewsDisponibles; //InmortalPrev,BalearPrev

    //Torretas torretas elegidas
    public List<GameObject> torretasUso; //Inmortal,Balear

    //Torretas preview elegidas
    public List<GameObject> previewsUso; //InmortalPrev,BalearPrev,

    public InvocarTorreta invocarTorreta;
    public HUD hud;

    // Start is called before the first frame update

    public void torretasElegidas(List<int> indice)
    {
        //hud.recibirIndices(indice);
        for(int i = indice.Count; i < 8; i++)
        {
            indice.Add(0);
        }
        recibirInd(indice);
    }

    public void recibirInd(List<int> indices)
    {

        for (int i = 0; i < indices.Count; i++)
        {
            torretasDisponibles.Add(torretasTotales[indices[i]]);
            previewsDisponibles.Add(previewsTotales[indices[i]]);
        }

        invocarTorreta.recibirTorretasYPreviews(torretasDisponibles, previewsDisponibles);
    }
}