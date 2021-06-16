using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextMission : MonoBehaviour
{

    public Text texto;
    public GameObject textoFin;
    public BufferTexto buffer;
    public int GameScene;

    public float velocidadEscritura;
    public float velocidadParpadeoCursor;

    string textoBuffer;
    string cursor;

    public bool finCinematica;

    // Start is called before the first frame update
    void Start()
    {
        texto.text = "";
        StartCoroutine(Escribir());
        StartCoroutine(ParpadeoCursor());
        

    }

    // Update is called once per frame
    void Update()
    {
        texto.text = textoBuffer + cursor;

        if (finCinematica)
        {
            textoFin.SetActive(true);
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(GameScene);
            }
        }
    }

    IEnumerator Escribir()
    {
        for (int i = 0; i < buffer.textos.Length; i++)
        {
            for (int j = 0; j < buffer.textos[i].texto.Length; j++)
            {
                textoBuffer += buffer.textos[i].texto[j];
                yield return new WaitForSeconds(velocidadEscritura);
            }

            textoBuffer += "\r\n";
            yield return new WaitForSeconds(buffer.textos[i].segundosEspera);

            if (buffer.textos[i].limpiarPágina)
            {
                textoBuffer = "";
            }
        }
        finCinematica = true;
        //yield return new WaitForSeconds(0.01F);
    }

    IEnumerator ParpadeoCursor()
    {
        bool cambio = true;
        while (true)
        {
            if (cambio)
            {
                cursor = "";
            }
            else
            {
                cursor = "_";
            }
            yield return new WaitForSeconds(velocidadParpadeoCursor);
            cambio = !cambio;
        }
    }
}
