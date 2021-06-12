using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Text audioTexto;
    Resolution[] resoluciones;
    public Dropdown resolucionDropdown;
    private void Start()
    {
        resoluciones = Screen.resolutions;
        int i = 0;
        while(!resoluciones[i].Equals(Screen.currentResolution))
        {
            i++;
        }
        resolucionDropdown.value = i;
        resolucionDropdown.RefreshShownValue();
    }
    public void SetVolumen (float volumen)
    {
        audioMixer.SetFloat("volumen", volumen);
        audioTexto.text = Mathf.Abs(Mathf.CeilToInt(((volumen + 80) / -80 * 100))).ToString();
    }

    public void SetCalidad(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetPantallaCompleta(bool completa)
    {
        Screen.fullScreen = completa;
    }
    public void SetResolucion(int resolucion)
    {
        Screen.SetResolution(resoluciones[resolucion].width, resoluciones[resolucion].height, Screen.fullScreen);
    }
    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }
}
