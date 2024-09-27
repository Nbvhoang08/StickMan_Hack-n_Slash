using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[CreateAssetMenu(fileName = "SoundData", menuName = "SoundData", order = 0)]
public class SoundData : ScriptableObject
{
    // Start is called before the first frame update
    public List<AudioClip> backGroundMusic;
    public List<AudioClip> sfx;
    public List<AudioClip> playSound;
    public AudioMixer audioMixer;

    public List<AudioClip> BackGroundMusic
    {
        get => backGroundMusic;
        set => backGroundMusic = value;
    }

    public List<AudioClip> Sfx
    {
        get => sfx;
        set => sfx = value;
    }
    public List<AudioClip> PlaySound
    {
        get => playSound;
        set => playSound = value;
    }
    public AudioMixer AudioMixer
    {
        get => audioMixer;
        set => audioMixer = value;
    }
}
