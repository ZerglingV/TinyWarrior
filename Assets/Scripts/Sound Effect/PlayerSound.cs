using UnityEngine;
using UnityEngine.UI;

public class PlayerSound : MonoBehaviour
{
        public AudioClip[] playerWalk;
        public AudioClip rangedAttack;
        public AudioClip[] meleeAttack;
        public AudioClip[] playerHurt;
        public AudioClip[] death;

        bool isPlayWalkAudioing;
        bool isPlayRangedAttackAudioing;
        bool isPlayMeleeAttackAudioing;

        AudioSource playerSource;

        void Awake()
        {
                playerSource = GetComponent<AudioSource>();
        }

        public void PlayWalkAudio()
        {
                if (!isPlayWalkAudioing)
                {
                        isPlayWalkAudioing = true;
                        int index = Random.Range(0, playerWalk.Length);
                        playerSource.clip = playerWalk[index];
                        playerSource.Play();
                        EndRangedAttackAudio();
                        EndMeleeAttackAudio();
                }
        }

        public void EndWalkAudio()
        {
                isPlayWalkAudioing = false;
        }

        public void PlayRangedAttackAudio()
        {
                if (!isPlayRangedAttackAudioing)
                {
                        isPlayRangedAttackAudioing = true;
                        playerSource.clip = rangedAttack;
                        playerSource.Play();
                        EndWalkAudio();
                        EndMeleeAttackAudio();
                }
        }

        public void EndRangedAttackAudio()
        {
                isPlayRangedAttackAudioing = false;
        }

        public void PlayMeleeAttackAudio()
        {
                if (!isPlayMeleeAttackAudioing)
                {
                        isPlayMeleeAttackAudioing = true;
                        int index = Random.Range(0, meleeAttack.Length);
                        playerSource.clip = meleeAttack[index];
                        playerSource.Play();
                        EndWalkAudio();
                        EndRangedAttackAudio();
                }
        }

        public void EndMeleeAttackAudio()
        {
                isPlayMeleeAttackAudioing = false;
        }

        public void PlayHurtAudio()
        {
                int index = Random.Range(0, playerHurt.Length);
                playerSource.clip = playerHurt[index];
                playerSource.Play();
        }

        public void PlayDeathAudio()
        {
                int index = Random.Range(0, death.Length);
                playerSource.clip = death[index];
                playerSource.Play();
        }
}
