using UnityEngine;

public class LavaSmallSound : MonoBehaviour
{
        public AudioClip[] lavaSmall;

        AudioSource lavaSmallPopSource;
        AudioSource lavaSmallSource;
        bool isInvokingFlow;
        bool isInvokingPop;

        void Awake()
        {
                lavaSmallSource = GetComponents<AudioSource>()[0];
                lavaSmallPopSource = GetComponents<AudioSource>()[1];
        }

        void Update()
        {
                if (!isInvokingFlow)
                {
                        isInvokingFlow = true;
                        Invoke(nameof(PlayLavaAudio), Random.Range(0f, 2f)); // play lavaPop audio per random in  seconds
                }
                if (!isInvokingPop)
                {
                        isInvokingPop = true;
                        Invoke(nameof(PlayLavaPopAudio), Random.Range(1f, 3f)); // play lavaPop audio per random in  seconds
                }
        }

        void PlayLavaPopAudio()
        {
                lavaSmallPopSource.Play();
                isInvokingPop = false;
        }

        void PlayLavaAudio()
        {
                int index = Random.Range(0, lavaSmall.Length);
                lavaSmallSource.clip = lavaSmall[index];
                lavaSmallSource.Play();
                isInvokingFlow = false;
        }
}