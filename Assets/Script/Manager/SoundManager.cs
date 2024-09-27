using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;


public enum FxID
{
    Click
}
public class SoundManager : Singleton<SoundManager>
{

    public SoundData soundData;
    public AudioSource MusicSource;
    public AudioMixerGroup[] AudioMixerGroups;
    public AudioMixer mixer;
    public int index = 0;
    public Slider music;
   


    protected override void Awake()
    {
        base.Awake();

        MusicSource = gameObject.AddComponent<AudioSource>();
        MusicSource.loop = true;
        MusicSource.outputAudioMixerGroup = AudioMixerGroups[0];

    }

    public void Start()
    {
        ChangeMusic();
        PlayMusic(true);
        ValueChangeInSlider();
    }
    public void ChangeMusic()
    {
        MusicSource.clip = soundData.BackGroundMusic[index];
    }

    private void Update()
    {
        ChangeMusic();
    }


    public void PlayMusic(bool play)
    {
        if (play)
        {
            MusicSource.Play();
        }
        else
        {
            MusicSource.Stop();
        }
    }
/*    public void PlayFx(FxID ID)
    {
        SfxSource.PlayOneShot(soundData.Sfx[(int)ID]);
    }*/

   /* public void PlayFxClicked()
    {
        SfxSource.PlayOneShot(soundData.Sfx[0]);
    }
*/
   /* public void ActionSound(int idx)
    {
        if (idx < soundData.PlaySound.Count && idx >= 0)
        {
            PlaySource.PlayOneShot(soundData.PlaySound[idx]);
        }

    }*/


    public void ValueChangeInSlider()
    {
        mixer.SetFloat("GameMixer", Mathf.Log10(music.value) * 20);
    }
}