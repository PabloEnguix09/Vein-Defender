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
    public Text audioGeneral;
    public Text audioEfectos;
    public Text audioMusica;
    Resolution[] resoluciones;
    public Dropdown resolucionDropdown;
    public Dropdown calidadDropdown;

    public GameObject aplicar;
    public GameObject ajustes;

    private int resActual;
    private float volGenActual;
    private float volEfActual;
    private float volMusActual;
    private int calActual;
    private bool compActual;

    private float volGen;
    private float volEf;
    private float volMus;
    private void Awake()
    {
        resoluciones = Screen.resolutions;
        int i = 0;
        while (!resoluciones[i].Equals(Screen.currentResolution))
        {
            i++;
        }
        resolucionDropdown.value = i;
        resolucionDropdown.RefreshShownValue();

        resActual = i;
        audioMixer.GetFloat("volumen", out volGenActual);
        audioMixer.GetFloat("efectos", out volEfActual);
        audioMixer.GetFloat("musica", out volMusActual);
        calActual = QualitySettings.GetQualityLevel();
        compActual = Screen.fullScreen;

        calidadDropdown.value = calActual;
        calidadDropdown.RefreshShownValue();
    }
    public void SetVolumenGeneral (float volumen)
    {
        volGen = volumen;
        audioMixer.SetFloat("volumen", volumen);
        audioGeneral.text = Mathf.Abs(Mathf.CeilToInt(((volumen + 80) / -80 * 100))).ToString();
    }

    public void SetVolumenEfectos(float volumen)
    {
        volEf = volumen;
        audioMixer.SetFloat("efectos", volumen);
        audioEfectos.text = Mathf.Abs(Mathf.CeilToInt(((volumen + 80) / -80 * 100))).ToString();
    }
    public void SetVolumenMusica(float volumen)
    {
        volMus = volumen;
        audioMixer.SetFloat("musica", volumen);
        audioMusica.text = Mathf.Abs(Mathf.CeilToInt(((volumen + 80) / -80 * 100))).ToString();
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
    public void ApplyChanges()
    {
        PlayerPrefs.SetInt("UnityGraphicsQuality", QualitySettings.GetQualityLevel());
        PlayerPrefs.SetFloat("Screenmanager Resolution Width", Screen.currentResolution.width);
        PlayerPrefs.SetFloat("Screenmanager Resolution Height", Screen.currentResolution.height);
        volGenActual = volGen;
        volMusActual = volMus;
        volEfActual = volEf;
        PlayerPrefs.Save();
    }
    public void DiscardChanges()
    {
        SetVolumenGeneral(volGenActual);
        SetVolumenEfectos(volEfActual);
        SetVolumenMusica(volMusActual);
        SetResolucion(resActual);

        resolucionDropdown.value = resActual;
        resolucionDropdown.RefreshShownValue();

        SetCalidad(calActual);
        calidadDropdown.value = calActual;
        calidadDropdown.RefreshShownValue();
        SetPantallaCompleta(compActual);
        ApplyChanges();
    }
}
