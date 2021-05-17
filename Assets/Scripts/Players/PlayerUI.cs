using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
        public Vector2 offset;
        public Text nameUI; // following name UI component
        public Image healthUI; // following health UI component
        public Image injuryUI; // following injury UI component
        public PlayerSound playerSound;

        Vector2 screenPosition;
        Animator playerAnimator;
        float health;
        float maxHealth;
        float receivedMeleeDamage;
        float receivedRangedDamage;

        void Awake()
        {
                maxHealth = GetComponent<PlayerProperty>().maxHealth;
                receivedMeleeDamage = GetComponent<PlayerProperty>().receivedMeleeDamage;
                receivedRangedDamage = GetComponent<PlayerProperty>().receivedRangedDamage;
                playerAnimator = GetComponent<Animator>();
                playerAnimator.SetFloat("Health", maxHealth);
                health = maxHealth;
        }

        void FixedUpdate()
        {
                SetUIPosition();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
                if (collision.name == "Arrow(Clone)" && collision.transform.parent != transform)
                {
                        health -= receivedRangedDamage;
                        playerAnimator.SetFloat("Health", health);
                        healthUI.fillAmount = health / maxHealth;
                        playerSound.HurtAudioPlay();
                }
                if (collision.name == "Fist(Clone)" && collision.transform.parent != transform)
                {
                        health -= receivedMeleeDamage;
                        playerAnimator.SetFloat("Health", health);
                        healthUI.fillAmount = health / maxHealth;
                        playerSound.HurtAudioPlay();
                }
        }

        void SetUIPosition()
        {
                screenPosition = Camera.main.WorldToScreenPoint(transform.position);
                healthUI.rectTransform.position = screenPosition + offset;
                injuryUI.rectTransform.position = screenPosition + offset;
                nameUI.rectTransform.position = screenPosition - offset * 1.3f;
        }

        // when player dies, make it not to move
        void ChangeToStatic()
        {
                Rigidbody2D playerRB = GetComponent<Rigidbody2D>();
                playerRB.bodyType = RigidbodyType2D.Static;
        }

        void Die()
        {
                Destroy(transform.parent.gameObject);
        }
}
