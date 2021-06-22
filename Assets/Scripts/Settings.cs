using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ---------------------------------------------------
// NAME: Settings.cs
// STATUS: DONE
// GAMEOBJECT: SettingsMenu
// DESCRIPTION: Guarda y modifica los componentes de los ajuster
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: Sliders de sonido
//
// AUTHOR: Pablo Enguix
// FEATURES ADDED: Menu
// ---------------------------------------------------

public class Settings : MonoBehaviour
{
    [Header("EMPIEZA ACTIVADO SIEMPRE")]
    public AudioMixer audioMixer;
    public Text audioGeneral;
    public Text audioEfectos;
    public Text audioMusica;
    public Text audioAmbiente;
    Resolution[] resoluciones;
    public Dropdown resolucionDropdown;
    public Dropdown calidadDropdown;

    [Header("Sliders volumen")]
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider ostSlider;
    public Slider ambienteSlider;

    public AudioSource ostSource, sfxSource, masterSource, ambienteSource;

    private int resActual;
    private float currVolMaster;
    private float currVolSfx;
    private float currVolOst;
    float currVolAmbiente;
    private int calActual;
    private bool compActual;

    private void Start()
    {
        resoluciones = Screen.resolutions;
        resolucionDropdown.options.Clear();

        int currentRes = 0;
        for(int i = 0;i<resoluciones.Length;i++)
        {
            resolucionDropdown.options.Add(new Dropdown.OptionData(resoluciones[i].ToString()));
            if(resoluciones[i].width == Screen.currentResolution.width && resoluciones[i].height == Screen.currentResolution.height && (Screen.currentResolution.refreshRate - resoluciones[i].refreshRate) < 2)
            {
                currentRes = i;
            }
        }

        resolucionDropdown.value = currentRes;
        resolucionDropdown.RefreshShownValue();

        // Calidad de la pantalla
        calActual = QualitySettings.GetQualityLevel();
        compActual = Screen.fullScreen;
        calidadDropdown.value = calActual;
        calidadDropdown.RefreshShownValue();

        // Recoge los valores del player prefs para el volumen
        masterSlider.value = PlayerPrefs.GetFloat("Volumen Master", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("Volumen Ost", 0.75f);
        ostSlider.value = PlayerPrefs.GetFloat("Volumen Sfx", 0.75f);
        ambienteSlider.value = PlayerPrefs.GetFloat("Volumen Ambiente", 0.75f);

        // Coge las variables del audiomixer
        audioMixer.GetFloat("VolMaster", out currVolMaster);
        audioMixer.GetFloat("VolSfx", out currVolSfx);
        audioMixer.GetFloat("VolOst", out currVolOst);
        audioMixer.GetFloat("VolAmbiente", out currVolAmbiente);

        // Desactiva el objeto de los ajustes
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    #region Llamadas desde los sliders de sonido
    public void SetVolumenGeneral (float volumen)
    {
        audioMixer.SetFloat("VolMaster", Mathf.Log10(volumen) * 20);
        audioGeneral.text = Mathf.FloorToInt(volumen * 100).ToString();
    }
    public void SetVolumenEfectos(float volumen)
    {
        audioMixer.SetFloat("VolSfx", Mathf.Log10(volumen) * 20);
        audioEfectos.text = Mathf.FloorToInt(volumen * 100).ToString();
    }
    public void SetVolumenMusica(float volumen)
    {
        audioMixer.SetFloat("VolOst", Mathf.Log10(volumen) * 20);
        audioMusica.text = Mathf.FloorToInt(volumen * 100).ToString();
    }
    public void SetVolumenAmbiente(float volumen)
    {
        audioMixer.SetFloat("VolAmbiente", Mathf.Log10(volumen) * 20);
        audioMixer.SetFloat("VolGrupal", Mathf.Log10(volumen) * 20);

        audioAmbiente.text = Mathf.FloorToInt(volumen * 100).ToString();
    }
    public void PruebaSonidoMaster()
    {
        masterSource.Play();
    }
    public void PruebaSonidoSfx()
    {
        sfxSource.Play();
    }
    public void PruebaSonidoOst()
    {
        ostSource.Play();
    }
    public void PruebaSonidoAmbiente()
    {
        ambienteSource.Play();
    }
    #endregion 

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
        Screen.SetResolution(resoluciones[resolucion].width, resoluciones[resolucion].height, Screen.fullScreen, resoluciones[resolucion].refreshRate);
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
        PlayerPrefs.SetFloat("Screenmanager Resolution RefreshRate", Screen.currentResolution.refreshRate);

        // Guarda los valores en los playerPrefs
        currVolSfx = sfxSlider.value;
        currVolOst = ostSlider.value;
        currVolMaster = masterSlider.value;
        currVolAmbiente = ambienteSlider.value;
        PlayerPrefs.SetFloat("Volumen Master", currVolMaster);
        PlayerPrefs.SetFloat("Volumen Ost", currVolOst);
        PlayerPrefs.SetFloat("Volumen Sfx", currVolSfx);
        PlayerPrefs.SetFloat("Volumen Ambiente", currVolAmbiente);
        PlayerPrefs.Save();

        gameObject.SetActive(false);
    }
    public void DiscardChanges()
    {
        SetVolumenGeneral(currVolMaster);
        SetVolumenEfectos(currVolSfx);
        SetVolumenMusica(currVolOst);
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
