using UnityEngine;

public class ArrowHitSound : MonoBehaviour
{
        public AudioClip[] arrowHit;

        AudioSource arrowHitSource;

        void Awake()
        {
                arrowHitSource = GetComponent<AudioSource>();
                PlayArrowHitAudio();
        }

        void Update()
        {
                if (!arrowHitSource.isPlaying)
                {
                        Destroy(this.gameObject);
                }
        }

        void PlayArrowHitAudio()
        {
                int index = Random.Range(0, arrowHit.Length);
                arrowHitSource.clip = arrowHit[index];
                arrowHitSource.Play();
        }
}