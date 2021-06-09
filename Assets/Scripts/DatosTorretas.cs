using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DatosTorretas : MonoBehaviour
{
    public Text nombre;
    public Text energía;
    public Text vida;
    public Text ataque;
    public Text cadencia;
    public Text tipoDisparo;
    public Text rango;
    public Text escudo;
    public Text caracteristicas;
    public Text reduccionDaño;
    public Text descripcion;

    public void MostrarDatos(TorretaSO torreta)
    {
        nombre.text = "Nombre: " + torreta.visual.nombre;
        energía.text = "Uso energético: " + torreta.energia;
        vida.text = "Salud: " + torreta.vidaMaxima;
        if(torreta.energia != torreta.energiaAlt)
        {
            energía.text += " (" + torreta.energiaAlt + " máx)";
        }
        ataque.text = "Daño: " + torreta.ataque;
        cadencia.text = "Cadencia de disparo: " + torreta.cadenciaDisparo;
        tipoDisparo.text = "Munición: " + torreta.tipoDisparo;
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
        caracteristicas.text = "Características: ";
        if (torreta.variantes.antiaerea)
        {
            caracteristicas.text += "Antiaérea";
        }
        if (torreta.variantes.invisibilidad)
        {
            ComprobarLista(caracteristicas, "Características: ");
            caracteristicas.text += "Invisibilidad";
        }
        if (torreta.variantes.regeneracion)
        {
            ComprobarLista(caracteristicas, "Características: ");
            caracteristicas.text += "Regeneración";
        }
        if(!torreta.variantes.antiaerea && !torreta.variantes.invisibilidad && !torreta.variantes.regeneracion) 
        {
            caracteristicas.text += "Ninguna";
        }
        reduccionDaño.text = "Reducción de daño: ";
        if(torreta.reduceDanyo.reducirDanyo)
        {
            if(torreta.reduceDanyo.frente)
            {
                reduccionDaño.text += "Frontal";
            }
            if (torreta.reduceDanyo.espalda)
            {
                ComprobarLista(reduccionDaño, "Reducción de daño: ");
                reduccionDaño.text += "Trasera";
            } 
            if (torreta.reduceDanyo.lados)
            {
                ComprobarLista(reduccionDaño, "Reducción de daño: ");
                reduccionDaño.text += "Lateral";
            }
            reduccionDaño.text += "("+ torreta.reduceDanyo.reduccion +")";
        }
        else
        {
            reduccionDaño.text += "No";
        }
        descripcion.text = "Descripción: " + torreta.visual.descripcion;
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
