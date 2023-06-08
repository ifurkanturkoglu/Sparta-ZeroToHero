using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Slider master, music, sfx;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource audioSourceMusic, audioSourceSFX;

    private void Start()
    {
        master.value = (music.maxValue + music.minValue) / 4;
        music.value = music.maxValue;
        sfx.value = sfx.maxValue;

        audioSourceMusic.Play();

    }
    public void MasterValue()
    {
        audioMixer.SetFloat("master", master.value);
    }
    public void MusicValue()
    {
        audioMixer.SetFloat("music", music.value);
    }
    public void SoundEffectValue()
    {
        audioMixer.SetFloat("sfx", sfx.value);
    }
}