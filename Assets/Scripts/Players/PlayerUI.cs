using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
        public Vector2 offset;
        public Image injuryImage; // following UI component
        public Image healthImage; // following UI component
        public Text nameText; // following UI component
        public GameObject youTitle; // following UI component
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
                        healthImage.fillAmount = health / maxHealth;
                        playerSound.PlayHurtAudio();
                }
                if (collision.name == "Fist(Clone)" && collision.transform.parent != transform)
                {
                        health -= receivedMeleeDamage;
                        playerAnimator.SetFloat("Health", health);
                        healthImage.fillAmount = health / maxHealth;
                        playerSound.PlayHurtAudio();
                }
        }

        void SetUIPosition()
        {
                screenPosition = Camera.main.WorldToScreenPoint(transform.position);
                healthImage.rectTransform.position = screenPosition + offset;
                injuryImage.rectTransform.position = screenPosition + offset;
                nameText.rectTransform.position = screenPosition - offset * 1.5f;
                youTitle.transform.position = screenPosition + offset * 1.8f;
        }

        // when player dies, make it not to move
        void ChangeToStatic()
        {
                playerSound.PlayDeathAudio();
                Rigidbody2D playerRB = GetComponent<Rigidbody2D>();
                playerRB.bodyType = RigidbodyType2D.Static;
        }

        void Die()
        {
                Destroy(transform.parent.gameObject);
        }
}
