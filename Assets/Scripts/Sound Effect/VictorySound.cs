using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictorySound : MonoBehaviour
{
        public AudioClip[] victoryMusic;
        public AudioClip victoryVoice;

        AudioSource victoryMusicAudio;
        AudioSource victoryVoiceAudio;

        void Awake()
        {
                victoryMusicAudio = GetComponents<AudioSource>()[0];
                victoryVoiceAudio = GetComponents<AudioSource>()[1];
                PlayVictoryBothAudio();
        }

        void PlayVictoryBothAudio()
        {
                int index = Random.Range(0, victoryMusic.Length);
                victoryMusicAudio.clip = victoryMusic[index];
                victoryMusicAudio.Play();
                victoryVoiceAudio.clip = victoryVoice;
                victoryVoiceAudio.Play();
        }
}
