using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public static SFXPlayer instance;
    public AudioSource player;

    public void Awake()
    {
        if (instance)
        {
            Debug.LogError("More than one SFXPlayer in scene!");
            return;
        }

        instance = this;
    }

    public void PlayEffect(AudioClip effect, float volume)
    {
        player.volume = volume;
        player.clip = effect;
        player.Play();
    }

    public void PlayEffectSingular(AudioClip effect, float volume)
    {
        if (!player.isPlaying)
        {
            PlayEffect(effect, volume);
        }
    }
}
