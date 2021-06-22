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
    public AudioMixer audioMixer;
    public Text audioGeneral;
    public Text audioEfectos;
    public Text audioMusica;
    Resolution[] resoluciones;
    public Dropdown resolucionDropdown;
    public Dropdown calidadDropdown;

    [Header("Sliders volumen")]
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider ostSlider;

    [Header("Botones")]
    public GameObject aplicar;
    public GameObject ajustes;


    private int resActual;
    private float currVolMaster;
    private float currVolSfx;
    private float currVolOst;
    private int calActual;
    private bool compActual;

    private float volMaster;
    private float volSfx;
    private float volOst;
    private void Awake()
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
        
        // Coge las variables del audiomixer
        audioMixer.GetFloat("VolMaster", out currVolMaster);
        audioMixer.GetFloat("VolSfx", out currVolSfx);
        audioMixer.GetFloat("VolOst", out currVolOst);

        // Calidad de la pantalla
        calActual = QualitySettings.GetQualityLevel();
        compActual = Screen.fullScreen;
        calidadDropdown.value = calActual;
        calidadDropdown.RefreshShownValue();

        // Recoge los valores del player prefs para el volumen
        volMaster = PlayerPrefs.GetFloat("Volumen Master");
        volOst = PlayerPrefs.GetFloat("Volumen Ost");
        volSfx = PlayerPrefs.GetFloat("Volumen Sfx");

        // Aplica los valores por defecto a los sliders
        masterSlider.value = volMaster;
        sfxSlider.value = volSfx;
        ostSlider.value = volOst;
    }

    #region Llamadas desde los sliders de sonido
    public void SetVolumenGeneral (float volumen)
    {
        volMaster = volumen;
        audioMixer.SetFloat("VolMaster", volumen);
        audioGeneral.text = Mathf.Abs(Mathf.CeilToInt(((volumen + 80) / -80 * 100))).ToString();
    }

    public void SetVolumenEfectos(float volumen)
    {
        volSfx = volumen;
        audioMixer.SetFloat("VolSfx", volumen);
        audioEfectos.text = Mathf.Abs(Mathf.CeilToInt(((volumen + 80) / -80 * 100))).ToString();
    }
    public void SetVolumenMusica(float volumen)
    {
        volOst = volumen;
        audioMixer.SetFloat("VolOst", volumen);
        audioMusica.text = Mathf.Abs(Mathf.CeilToInt(((volumen + 80) / -80 * 100))).ToString();
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
        currVolSfx = volSfx;
        currVolOst = volOst;
        currVolMaster = volMaster;
        PlayerPrefs.SetFloat("Volumen Master", currVolMaster);
        PlayerPrefs.SetFloat("Volumen Ost", currVolOst);
        PlayerPrefs.SetFloat("Volumen Sfx", currVolSfx);
        PlayerPrefs.Save();
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
