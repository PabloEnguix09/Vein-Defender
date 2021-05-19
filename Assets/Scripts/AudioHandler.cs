using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{

    AudioSource source;

    public AudioClip[] clips;

    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }
    public void PlaySound(int index, bool loop)
    {
        source.clip = clips[index];

        source.loop = loop;

        source.Play();
    }
}
