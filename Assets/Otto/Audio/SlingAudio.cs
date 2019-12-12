using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SlingAudio : MonoBehaviourPunCallbacks
{

    public AudioSource SlingSource;

    public AudioClip SlingLoad;
    public AudioClip SlingShot;
    public AudioClip BallBounce;

    GameObject Ball;

    int i = 0;

    public float slingVolumeMultiplier;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Ball == null) {
            Ball = GameObject.FindGameObjectWithTag("Ball");
        }

        if (Ball != null && i == 0)
        {
            SlingSource = Ball.GetComponent<AudioSource>();
            i++;
        }
    }

    
    public void PlaySlingLoadSound() {
        this.photonView.RPC("PlayLoadSound", RpcTarget.All);
    }

    public void PlaySlingShotSound() {
        this.photonView.RPC("PlayShotSound", RpcTarget.All);
    }

    public void PlayBallBounceSound(float volume) {
        this.photonView.RPC("PlayBounceSound", RpcTarget.All, volume);
    }

    [PunRPC]
    public void PlayShotSound() {
        SlingSource.clip = SlingShot;
        SlingSource.volume = 0.8f * slingVolumeMultiplier;
        SlingSource.Play();
    }

    [PunRPC]
    public void PlayBounceSound(float volume) {
        SlingSource.clip = BallBounce;
        SlingSource.volume = volume * slingVolumeMultiplier;
        SlingSource.Play();
    }

    [PunRPC]
    public void PlayLoadSound() {
        SlingSource.clip = SlingLoad;
        SlingSource.volume = 0.8f * slingVolumeMultiplier;
        SlingSource.Play();
    }
}
