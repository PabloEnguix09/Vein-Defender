using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextMission : MonoBehaviour
{
    //texto de la pantalla
    public Text texto;

    //aviso de fin de cinematica
    public GameObject textoFin;

    //textos a escribir
    public BufferTexto buffer;

    //la siguiente escena a cargar
    public int GameScene;

    
    public float velocidadEscritura;
    public float velocidadParpadeoCursor;

    //textos a rellenar el text
    string textoBuffer;
    string cursor;

    //fin de la cinematica
    public bool finCinematica;

    void Start()
    {
        //vaciar el texto
        texto.text = "";
        //empezar a escribir
        StartCoroutine(Escribir());
        //parpadeo del cursor
        StartCoroutine(ParpadeoCursor());
        

    }

    void Update()
    {
        //actualizar el texto según el buffer y el cursor
        texto.text = textoBuffer + cursor;

        //si ha terminado el texto
        if (finCinematica)
        {
            //mostrar el texto de abajo
            textoFin.SetActive(true);
            if (Input.anyKeyDown)
            {
                //al pulsar cualquier tecla carga la siguiente escena
                SceneManager.LoadScene(GameScene);
            }
        }
    }

    IEnumerator Escribir()
    {
        //recorre todos los textos que hay en el buffer
        for (int i = 0; i < buffer.textos.Length; i++)
        {
            //recorre cada letra del string
            for (int j = 0; j < buffer.textos[i].texto.Length; j++)
            {
                //añade la letra
                textoBuffer += buffer.textos[i].texto[j];
                //se espera según la velocidad de escritura
                yield return new WaitForSeconds(velocidadEscritura);
            }

            //añadimos un salto de linea
            textoBuffer += "\r\n";
            //esperamos el tiempo que cada linea dicte
            yield return new WaitForSeconds(buffer.textos[i].segundosEspera);

            //en caso de ser el final de una página
            if (buffer.textos[i].limpiarPágina)
            {
                //vaciar el texto
                textoBuffer = "";
            }
        }
        //marcar el fin de la cinematica
        finCinematica = true;
    }

    IEnumerator ParpadeoCursor()
    {
        bool cambio = true;
        //se ejecuta constantemente
        while (true)
        {
            //si esta activado lo desactiva y viceversa
            if (cambio)
            {
                cursor = "";
            }
            else
            {
                cursor = "_";
            }

            //velocidad del parpadeo
            yield return new WaitForSeconds(velocidadParpadeoCursor);
            //hacer el cambio
            cambio = !cambio;
        }
    }
}
