using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MejoraSlot : MonoBehaviour, IPointerClickHandler
{
    public string nombre;
    public string nombreMejora;
    public SistemaMejoras mejoras;
    public Canvas canvas;
    public CameraController camara;

    public void OnPointerClick(PointerEventData eventData)
    {

        //Mejoras personaje
        Activar(nameof(mejoras.vidaTbyte), ref mejoras.vidaTbyte);
        Activar(nameof(mejoras.escudoTbyte), ref mejoras.escudoTbyte);
        Activar(nameof(mejoras.mejoraMinimapa), ref mejoras.mejoraMinimapa);
        Activar(nameof(mejoras.mejoraMagnetismo), ref mejoras.mejoraMagnetismo);
        Activar(nameof(mejoras.mejoraImpulso), ref mejoras.mejoraImpulso);
        Activar(nameof(mejoras.mejoraEnergia), ref mejoras.mejoraEnergia);
        Activar(nameof(mejoras.mejoraMarcador), ref mejoras.mejoraMarcador);
        Activar(nameof(mejoras.mejoraDebilitante), ref mejoras.mejoraDebilitante);
        Activar(nameof(mejoras.mejoraSparky), ref mejoras.mejoraSparky);
        Activar(nameof(mejoras.explosionPEM), ref mejoras.explosionPEM);

        //Desbloqeuar torretas
        Activar(nameof(mejoras.hoplitaMohawk), ref mejoras.hoplitaMohawk);
        Activar(nameof(mejoras.inmortalFantasma), ref mejoras.inmortalFantasma);
        Activar(nameof(mejoras.scutumBerserker), ref mejoras.scutumBerserker);
        Activar(nameof(mejoras.balearTrampa), ref mejoras.balearTrampa);

        //Mejoras de torretas
        Activar(nameof(mejoras.costeEnergia), ref mejoras.costeEnergia);
        Activar(nameof(mejoras.balasReforzadas), ref mejoras.balasReforzadas);
        Activar(nameof(mejoras.laseresMejorados), ref mejoras.laseresMejorados);
        Activar(nameof(mejoras.cadenciaMejorada), ref mejoras.cadenciaMejorada);
        Activar(nameof(mejoras.escanerAmenazas), ref mejoras.escanerAmenazas);
        Activar(nameof(mejoras.escudoEnergia), ref mejoras.escudoEnergia);
        Activar(nameof(mejoras.balaExplosiva), ref mejoras.balaExplosiva);
        Activar(nameof(mejoras.laserPerforante), ref mejoras.laserPerforante);
        Activar(nameof(mejoras.laserPerseguidor), ref mejoras.laserPerseguidor);
        Activar(nameof(mejoras.balaPEM), ref mejoras.balaPEM);

        //Mejoras de utilidad
        Activar(nameof(mejoras.mejoraCamara), ref mejoras.mejoraCamara);
        Activar(nameof(mejoras.mejoraCaminos), ref mejoras.mejoraCaminos);
        Activar(nameof(mejoras.escudoDefensor), ref mejoras.escudoDefensor);
    }

    internal void Clear()
    {
        nombre = "";
        nombreMejora = "";
    }

    public void Activar(string mejoraNombre, ref bool mejora)
    {
        if (nombreMejora.Equals(mejoraNombre))
        {
            mejora = true;
            canvas.gameObject.SetActive(false);
            camara.BloquearCamara(false);
        }
    }
}
