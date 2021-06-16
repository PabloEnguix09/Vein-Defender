using UnityEngine;

[System.Serializable]
public class TextoALeer
{
    public int segundosEspera;
    public bool limpiarPágina;

    [TextArea]
    public string texto;

    public TextoALeer(int segundosEspera, string texto)
    {
        this.segundosEspera = segundosEspera;
        this.texto = texto;
    }


}
