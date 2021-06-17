using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
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

    AudioSource master, sfx, ambiente, grupo, ost, efectos;

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

            switch (s.mixer)
            {
                // En caso de no haber un audio source para el tipo de audio, se crea uno nuevo en el game object
                case Sonido.Mixer.master:
                    if(master == null)
                    {
                        s.origen = gameObject.AddComponent<AudioSource>();
                    } else
                    {
                        s.origen = master;
                    }
                    s.origen.outputAudioMixerGroup = mixer.FindMatchingGroups("Master")[0];
                    break;
                case Sonido.Mixer.sfx:
                    if (sfx == null)
                    {
                        s.origen = gameObject.AddComponent<AudioSource>();
                    }
                    else
                    {
                        s.origen = sfx;
                    }
                    s.origen.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/SFX")[0];
                    break;
                case Sonido.Mixer.ambiente:
                    if (ambiente == null)
                    {
                        s.origen = gameObject.AddComponent<AudioSource>();
                    }
                    else
                    {
                        s.origen = ambiente;
                    }
                    s.origen.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/SFX/Ambiente")[0];
                    break;
                case Sonido.Mixer.grupo:
                    if (grupo == null)
                    {
                        s.origen = gameObject.AddComponent<AudioSource>();
                    }
                    else
                    {
                        s.origen = grupo;
                    }
                    s.origen.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/SFX/Grupal")[0];
                    break;
                case Sonido.Mixer.ost:
                    if (ost == null)
                    {
                        s.origen = gameObject.AddComponent<AudioSource>();
                    }
                    else
                    {
                        s.origen = ost;
                    }
                    s.origen.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/OST")[0];
                    break;
                case Sonido.Mixer.efectos:
                    if (efectos == null)
                    {
                        s.origen = gameObject.AddComponent<AudioSource>();
                    }
                    else
                    {
                        s.origen = efectos;
                    }
                    s.origen.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/SFX/Efectos")[0];
                    break;
                default:
                    break;
            }

        }
    }

    public void Play(int indice)
    {
        if (timer <= 0)
        {
            Sonido s = Array.Find(sonidos, sonido => sonido.indice == indice);

            s.origen.clip = s.clip;
            s.origen.volume = s.volumen;
            s.origen.pitch = s.tono;
            s.origen.loop = s.bucle;
            s.origen.Play();
            timer = maxTimer;
        }
    }

    public void PauseAll()
    {
        if(master != null)
        {
            FadeOut(master);
            master.Stop();
        }
        if (sfx != null)
        {
            FadeOut(sfx);
            sfx.Stop();
        }
        if (ambiente != null)
        {
            FadeOut(ambiente);
            ambiente.Stop();
        }
        if (grupo != null)
        {
            FadeOut(grupo);
            grupo.Stop();
        }
        if (ost != null)
        {
            FadeOut(ost);
            ost.Stop();
        }
        if (efectos != null)
        {
            FadeOut(efectos);
            efectos.Stop();
        }
    }

    IEnumerator FadeOut(AudioSource source)
    {
        for (float ft = 1f; ft >= 0; ft -= 0.1f)
        {
            source.volume = ft;

            yield return new WaitForSeconds(.1f);
        }
    }
}
