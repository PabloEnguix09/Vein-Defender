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
    float timer = 0;
    float maxTimer = 0.2f;

    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

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
        if (timer <= 0)
        {
            Sonido s = Array.Find(sonidos, sonido => sonido.indice == indice);
            s.origen.Play();
            timer = maxTimer;
        }
    }
}
