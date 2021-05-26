using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
        AudioSource buttonClickSource;

        void Awake()
        {
                buttonClickSource = GetComponent<AudioSource>();
        }

        public void PlayButtonClickAudio()
        {
                buttonClickSource.Play();
        }
}
