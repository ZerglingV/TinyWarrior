using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
        public AudioSource anotherPortalSource;
        public AudioClip[] portalEnter;
        public Vector2 destination;
        public Animator portalEffectAnimator;

        void OnTriggerEnter2D(Collider2D collision)
        {
                if (collision.name.StartsWith("Player"))
                {
                        collision.transform.position = destination;
                        PlayPortalEffect();
                        PlayPortalAudio();
                }
        }

        void PlayPortalEffect()
        {
                portalEffectAnimator.Play("Portal");
        }

        void PlayPortalAudio()
        {
                int index = Random.Range(0, portalEnter.Length);
                anotherPortalSource.clip = portalEnter[index];
                anotherPortalSource.Play();
        }
}
