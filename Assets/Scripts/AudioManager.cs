using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField]
    AudioClip MenuSound;

    [SerializeField]
    AudioClip ButtonSound;

    [SerializeField]
    AudioClip CompleteSound;

    [SerializeField]
    AudioClip[] LevelMusic;
    [SerializeField]
    AudioClip[] ClickSounds = new AudioClip[4];

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

        audioSource = this.GetComponent<AudioSource>();
    }

    public void StartMusic(int levelNum)
    {
        audioSource.clip = LevelMusic[levelNum];
        audioSource.loop = false;
        audioSource.Play();
    }
    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void PlayMenuSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(MenuSound, 1f);
    }

    public void PlayButtonSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(ButtonSound, 1f);
    }

    public void PlayCompleteSound()
    {
        if (Globals.AudioOn)
            audioSource.PlayOneShot(CompleteSound, 1f);
    }

    public void PlayClickSound()
    {
        int num = Random.Range(0, ClickSounds.Length - 1);
        audioSource.PlayOneShot(ClickSounds[num], .5f);
    }
}
