using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    public AudioSource audioSource; 
    [SerializeField] List<AudioClip> attacksAudioClips;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null){
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void attackAudioClips(){
        int random = Random.Range(0,attacksAudioClips.Count);
        AudioClip clip = attacksAudioClips[random];
        audioSource.PlayOneShot(clip);
    }
    
}
