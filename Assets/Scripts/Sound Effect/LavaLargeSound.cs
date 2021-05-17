using UnityEngine;

public class LavaLargeSound : MonoBehaviour
{
        public AudioSource lavaPopSource;

        bool isInvokingPop;

        void Update()
        {
                if (!isInvokingPop)
                {
                        isInvokingPop = true;
                        Invoke(nameof(PlayLavaPopAudio), Random.Range(1f, 3f)); // play lavaPop audio per random in  seconds
                }
        }

        void PlayLavaPopAudio()
        {
                lavaPopSource.Play();
                isInvokingPop = false;
        }
}