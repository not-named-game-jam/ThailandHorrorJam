using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource playingMusic;
    private List<AudioSource> playingSfx = new List<AudioSource>();
    public float sfxVolume = 1f;
    public float musicVolume = 1f;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        for (int i = playingSfx.Count - 1; i >= 0; i--)
        {
            if (!playingSfx[i].isPlaying)
            {
                Destroy(playingSfx[i]);
                playingSfx.RemoveAt(i);
            }
        }
    }

    public void PlayMusic(string music, float volume = 1f) {
        if (playingMusic != null) playingMusic.Stop();
        playingMusic = gameObject.AddComponent<AudioSource>();
        playingMusic.loop = true;
        playingMusic.volume = volume * musicVolume;
        AudioClip clip = Resources.Load<AudioClip>("Sounds/Music/" + music);
        if(clip == null) {
            Debug.LogError("Music not found: " + music);
            return;
        }
        playingMusic.clip = clip;
        playingMusic.Play();
    }

    public void PlaySfx(string sfxName, float volume = 1f, float pitch = 1f) {
        if(string.IsNullOrEmpty(sfxName)) return;
        AudioClip clip = Resources.Load<AudioClip>("Sounds/SFX/" + sfxName);
        if (clip == null) {
            Debug.LogError($"SFX not found: {sfxName}");
            return;
        }
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume * sfxVolume;
        source.pitch = Mathf.Clamp(pitch, 0.5f, 2f);
        source.Play();
        playingSfx.Add(source);
    }

    public void StopMusic() {
        if (playingMusic != null) playingMusic.Stop();
    }
}
