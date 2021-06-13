using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SeleccionarMejora : MonoBehaviour
{
    public SistemaMejoras mejoras;
    public MejoraItem[] listaMejoras;

    public MejoraSlot[] mejoraSlots;
    public List<Text> textos;

    // Start is called before the first frame update
    private void Start()
    {
        MostrarMejoras();
    }
    public void ClearMejoras()
    {
        for(int i = 0; i < mejoraSlots.Length; i++)
        {
            mejoraSlots[i].Clear();
            textos[i].text = "";
        }
    }
    public void MostrarMejoras()
    {
        ClearMejoras();
        int i = 0;
        while (i < mejoraSlots.Length)
        {
            int aleatorizador = Random.Range(0, listaMejoras.Length - 1);
            for (int j = 0; j < listaMejoras.Length; j++)
            {
                // Mejoras Personaje
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.vidaTbyte), ref mejoras.vidaTbyte);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.escudoTbyte), ref mejoras.escudoTbyte);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.mejoraMinimapa), ref mejoras.mejoraMinimapa);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.mejoraMagnetismo), ref mejoras.mejoraMagnetismo);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.mejoraImpulso), ref mejoras.mejoraImpulso);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.mejoraEnergia), ref mejoras.mejoraEnergia);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.mejoraMarcador), ref mejoras.mejoraMarcador);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.mejoraDebilitante), ref mejoras.mejoraDebilitante);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.mejoraSparky), ref mejoras.mejoraSparky);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.explosionPEM), ref mejoras.explosionPEM);

                //Desbloquear torretas
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.hoplitaMohawk), ref mejoras.hoplitaMohawk);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.inmortalFantasma), ref mejoras.inmortalFantasma);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.scutumBerserker), ref mejoras.scutumBerserker);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.balearTrampa), ref mejoras.balearTrampa);

                //Mejoras de torretas
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.costeEnergia), ref mejoras.costeEnergia);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.balasReforzadas), ref mejoras.balasReforzadas);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.laseresMejorados), ref mejoras.laseresMejorados);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.cadenciaMejorada), ref mejoras.cadenciaMejorada);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.escanerAmenazas), ref mejoras.escanerAmenazas);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.escudoEnergia), ref mejoras.escudoEnergia);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.balaExplosiva), ref mejoras.balaExplosiva);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.laserPerforante), ref mejoras.laserPerforante);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.laserPerseguidor), ref mejoras.laserPerseguidor);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.balaPEM), ref mejoras.balaPEM);

                //Mejoras de utilidad
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.mejoraCamara), ref mejoras.mejoraCamara);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.mejoraCaminos), ref mejoras.mejoraCaminos);
                ComprobarActivada(ref mejoraSlots[i], textos[i], listaMejoras[aleatorizador], nameof(mejoras.escudoDefensor), ref mejoras.escudoDefensor);
            }
            if (textos[i].text != "")
            {
                i++;
            }
        }
    }

    public void ComprobarActivada(ref MejoraSlot slot, Text texto, MejoraItem item, string nombreMejora, ref bool mejora)
    {
        if(slot.nombre == "")
        {
            if (nombreMejora.Equals(item.nombreMejora) && !mejora)
            {
                if (!mejoraSlots[0].nombreMejora.Equals(nombreMejora) && !mejoraSlots[1].nombreMejora.Equals(nombreMejora) && !mejoraSlots[2].nombreMejora.Equals(nombreMejora))
                {
                    item.transform.position = slot.transform.position;
                    
                    //item.gameObject.SetActive(true);
                    texto.text = item.nombre;
                    //slot.gameObject.SetActive(false);
                    slot.nombre = item.nombre;
                    slot.nombreMejora = item.nombreMejora;
                    slot.GetComponent<Image>().sprite = item.GetComponent<Image>().sprite;
                }
            }
        }
    }
}
