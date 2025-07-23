using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance {  get { return instance; } }

    public AudioClip flipSound;
    public AudioClip matchSound;
    public AudioClip misMatchsound;
    public AudioClip gameOversound;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();  
    }

    public void FlipSound() => audioSource.PlayOneShot(flipSound);
    public void MatchSound() => audioSource.PlayOneShot(matchSound);
    public void MisMatchSound() => audioSource.PlayOneShot(misMatchsound);
    public void GameOverSound() => audioSource.PlayOneShot(gameOversound);

}
