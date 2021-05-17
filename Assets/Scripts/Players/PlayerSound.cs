using UnityEngine;
using UnityEngine.UI;

public class PlayerSound : MonoBehaviour
{
        public AudioClip[] playerWalk;
        public AudioClip rangedAttack;
        public AudioClip[] meleeAttack;
        public AudioClip[] playerHurt;

        bool isWalkAudioPlaying;
        bool isRangedAttackAudioPlaying;
        bool isMeleeAttackAudioPlaying;

        AudioSource playerSource;

        void Awake()
        {
                playerSource = GetComponent<AudioSource>();
        }

        public void WalkAudioPlay()
        {
                if (!isWalkAudioPlaying)
                {
                        isWalkAudioPlaying = true;
                        int index = Random.Range(0, playerWalk.Length);
                        playerSource.clip = playerWalk[index];
                        playerSource.Play();
                        RangedAttackAudioEnd();
                        MeleeAttackAudioEnd();
                }
        }

        public void WalkAudioEnd()
        {
                isWalkAudioPlaying = false;
        }

        public void RangedAttackAudioPlay()
        {
                if (!isRangedAttackAudioPlaying)
                {
                        isRangedAttackAudioPlaying = true;
                        playerSource.clip = rangedAttack;
                        playerSource.Play();
                        WalkAudioEnd();
                        MeleeAttackAudioEnd();
                }
        }

        public void RangedAttackAudioEnd()
        {
                isRangedAttackAudioPlaying = false;
        }

        public void MeleeAttackAudioPlay()
        {
                if (!isMeleeAttackAudioPlaying)
                {
                        isMeleeAttackAudioPlaying = true;
                        int index = Random.Range(0, meleeAttack.Length);
                        playerSource.clip = meleeAttack[index];
                        playerSource.Play();
                        WalkAudioEnd();
                        RangedAttackAudioEnd();
                }
        }

        public void MeleeAttackAudioEnd()
        {
                isMeleeAttackAudioPlaying = false;
        }

        public void HurtAudioPlay()
        {
                int index = Random.Range(0, playerHurt.Length);
                playerSource.clip = playerHurt[index];
                playerSource.Play();
        }
}
