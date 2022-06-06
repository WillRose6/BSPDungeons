using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    public Player player;
    public AudioSource fullMusicSource;
    public AudioSource backgroundMusicSource;
    public float musicMinimum, musicMaximum;

    private void Start()
    {
        SetBackgroundMusic(References.instance.Songs[Random.Range(0, References.instance.Songs.Count)]);
    }

    private IEnumerator ChangeMusicLevelToGoal(AudioSource source, float goal)
    {
        while(Mathf.Abs(source.volume - goal) > 0.02f) { 
            source.volume = Mathf.Clamp(Mathf.Lerp(source.volume, goal, Time.deltaTime/2), musicMinimum, musicMaximum);
            yield return null;
        }
    }

    public void SetBackgroundMusic(AudioClip clip)
    {
        fullMusicSource.clip = clip;
        fullMusicSource.Play();
    }
}
