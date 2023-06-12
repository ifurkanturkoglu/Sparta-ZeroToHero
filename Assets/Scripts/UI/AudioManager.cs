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
        master.value = master.maxValue;
        music.value = music.maxValue;
        sfx.value = sfx.maxValue;

        audioSourceMusic.Play();

    }
    public void MasterValue()
    {
        audioMixer.SetFloat("master", Mathf.Log10(master.value)*20);
    }
    public void MusicValue()
    {
        audioMixer.SetFloat("music",  Mathf.Log10(music.value)*20);
    }
    public void SoundEffectValue()
    {
        audioMixer.SetFloat("sfx",  Mathf.Log10(sfx.value)*20);
    }
}