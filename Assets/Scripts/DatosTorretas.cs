using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatosTorretas : MonoBehaviour
{
    public Text nombre;
    public Text energ�a;
    public Text vida;
    public Text ataque;
    public Text cadencia;
    public Text tipoDisparo;
    public Text rango;
    public Text escudo;
    public Text caracteristicas;
    public Text reduccionDa�o;
    public Text descripcion;

    public void MostrarDatos(TorretaSO torreta)
    {
        nombre.text = "Nombre: " + torreta.visual.nombre;
        energ�a.text = "Uso energ�tico: " + torreta.energia;
        vida.text = "Salud: " + torreta.vidaMaxima;
        if(torreta.energia != torreta.energiaAlt)
        {
            energ�a.text += " (" + torreta.energiaAlt + " m�x)";
        }
        ataque.text = "Da�o: " + torreta.ataque;
        cadencia.text = "Cadencia de disparo: " + torreta.cadenciaDisparo;
        tipoDisparo.text = "Munici�n: " + torreta.tipoDisparo;
        rango.text = "Rango: " + torreta.distanciaDisparo;
        escudo.text = "Escudo: ";
        if (torreta.escudo.escudoMaximo > 0)
        {
            escudo.text += torreta.escudo.escudoMaximo + "("+torreta.escudo.escudoRegen+" regen)";
        }
        else
        {
            escudo.text += "No";
        }
        caracteristicas.text = "Caracter�sticas: ";
        if (torreta.variantes.antiaerea)
        {
            caracteristicas.text += "Antia�rea";
        }
        if (torreta.variantes.invisibilidad)
        {
            ComprobarLista(caracteristicas, "Caracter�sticas: ");
            caracteristicas.text += "Invisibilidad";
        }
        if (torreta.variantes.regeneracion)
        {
            ComprobarLista(caracteristicas, "Caracter�sticas: ");
            caracteristicas.text += "Regeneraci�n";
        }
        if(!torreta.variantes.antiaerea && !torreta.variantes.invisibilidad && !torreta.variantes.regeneracion) 
        {
            caracteristicas.text += "Ninguna";
        }
        reduccionDa�o.text = "Reducci�n de da�o: ";
        if(torreta.reduceDanyo.reducirDanyo)
        {
            if(torreta.reduceDanyo.frente)
            {
                reduccionDa�o.text += "Frontal";
            }
            if (torreta.reduceDanyo.espalda)
            {
                ComprobarLista(reduccionDa�o, "Reducci�n de da�o: ");
                reduccionDa�o.text += "Trasera";
            } 
            if (torreta.reduceDanyo.lados)
            {
                ComprobarLista(reduccionDa�o, "Reducci�n de da�o: ");
                reduccionDa�o.text += "Lateral";
            }
            reduccionDa�o.text += "("+ torreta.reduceDanyo.reduccion +")";
        }
        else
        {
            reduccionDa�o.text += "No";
        }
        descripcion.text = "Descripci�n: " + torreta.visual.descripcion;
        gameObject.SetActive(true);
    }
    public void OcultarDatos()
    {
        gameObject.SetActive(false);
    }

    public void ComprobarLista(Text texto, string comprobar)
    {
        if(!texto.text.Equals(comprobar))
        {
            texto.text += ", ";
        }
    }
}
