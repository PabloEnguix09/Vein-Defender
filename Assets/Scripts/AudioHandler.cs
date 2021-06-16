using UnityEngine.Audio;
using System;
using UnityEngine;
// ---------------------------------------------------
// NAME: AudioHandler.cs
// STATUS: WIP
// GAMEOBJECT: AudioHandler
// DESCRIPTION: Controla la ejecucion de efectos de sonido
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: 
// ---------------------------------------------------
public class AudioHandler : MonoBehaviour
{

    public Sonido[] sonidos;
    public AudioMixer mixer;

    private void Awake()
    {

        foreach (Sonido s in sonidos)
        {
            s.origen = gameObject.AddComponent<AudioSource>();
            s.origen.clip = s.clip;
            s.origen.volume = s.volumen;
            s.origen.pitch = s.tono;
            s.origen.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/SFX")[0];
        }
    }

    public void Play(int indice)
    {
        Sonido s = Array.Find(sonidos, sonido => sonido.indice == indice);
        s.origen.Play();
    }
}
