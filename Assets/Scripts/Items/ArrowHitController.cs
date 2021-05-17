using UnityEngine;

public class ArrowHitController : MonoBehaviour
{
        public AudioClip[] arrowHit;

        AudioSource audioSource;

        void Awake()
        {
                audioSource = GetComponent<AudioSource>();
                PlayArrowHitAudio();
        }

        void Update()
        {
                if (!audioSource.isPlaying)
                {
                        Destroy(this.gameObject);
                }
        }

        void PlayArrowHitAudio()
        {
                int index = Random.Range(0, arrowHit.Length);
                audioSource.clip = arrowHit[index];
                audioSource.Play();
        }
}