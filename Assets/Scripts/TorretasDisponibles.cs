using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorretasDisponibles : MonoBehaviour
{

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

    // Start is called before the first frame update

    public void torretasElegidas(List<int> indice)
    {

        for (int i = 0; i < indice.Count; i++)
        {
            torretasDisponibles.Add(torretasTotales[indice[i]]);
            previewsDisponibles.Add(previewsTotales[indice[i]]);
        }

        //Funcion de pasar
        // torretasUso = HUD.recibirTorretas(torretasDisponibles);
        // previewUso = HUD.recibirPreview(previewDisponibles);

        invocarTorreta.recibirTorretasYPreviews(torretasDisponibles, previewsDisponibles);

        //invocarTorreta.recibirTorretasYPreviews(torretasUso, previewsUso);

    }
}