using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
        public AudioSource anotherPortalSource;
        public AudioClip[] portalEnter;
        public Vector2 destination;

        void OnTriggerEnter2D(Collider2D collision)
        {
                if (collision.name == "Player")
                {
                        collision.transform.position = destination;
                        PlayPortalAudio();
                }
        }

        void PlayPortalAudio()
        {
                int index = Random.Range(0, portalEnter.Length);
                anotherPortalSource.clip = portalEnter[index];
                anotherPortalSource.Play();
        }
}
