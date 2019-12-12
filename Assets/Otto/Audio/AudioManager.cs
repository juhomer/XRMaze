using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource musicSource;

    public AudioClip FunkyLab;
    public AudioClip SlowLab;

    public float volumeMultiplier;
    private bool changer;
    // Start is called before the first frame update
    void Start()
    {
        float random = Random.Range(0f, 2f);
        if (random <= 1f) {
            musicSource.clip = FunkyLab;
            changer = true;
            musicSource.volume = 0.8f * volumeMultiplier;
            musicSource.PlayDelayed(2f);
        } else {
            musicSource.clip = SlowLab;
            changer = false;
            musicSource.volume = 0.8f * volumeMultiplier;
            musicSource.PlayDelayed(2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!musicSource.isPlaying) {
            if (changer) {
                ChangeMusic(SlowLab);
                changer = false;
            } else {
                ChangeMusic(FunkyLab);
                changer = true;
            }
        }
    }

    public void ChangeMusic(AudioClip clip) {
        musicSource.clip = clip;
        musicSource.volume = 0.8f * volumeMultiplier;
        musicSource.PlayDelayed(4f);
    }
}
