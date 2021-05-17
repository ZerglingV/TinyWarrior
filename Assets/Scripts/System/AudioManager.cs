using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
        static AudioManager current;

        AudioSource musicSource;

        void Awake()
        {
                current = this;

                musicSource = gameObject.AddComponent<AudioSource>();
        }
}
