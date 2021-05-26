using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
        public Image hurtDurationImage; // following UI component
        public Image healthImage; // following UI component
        public PlayerSound playerSound;
        public float hurtDuration;
        public float hurtSpiteDuration;

        GameSocketClient gameSocketClient;
        GameManager gameManager;
        PlayerProperty playerProperty;
        Animator playerAnimator;
        Collider2D playerCollider;

        float receivedMeleeDamage;
        float receivedRangedDamage;
        float maxHealth;
        float health;
        float hurtSpriteCount;

        SpriteRenderer sprite;

        void Awake()
        {
                gameSocketClient = GameObject.Find("ClientProperty").GetComponent<GameSocketClient>();
                gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

                playerProperty = GetComponent<PlayerProperty>();
                playerAnimator = GetComponent<Animator>();
                playerCollider = GetComponent<Collider2D>();

                receivedMeleeDamage = playerProperty.receivedMeleeDamage;
                receivedRangedDamage = playerProperty.receivedRangedDamage;
                maxHealth = playerProperty.maxHealth;
                health = maxHealth;
                playerAnimator.SetFloat("Health", maxHealth);

                sprite = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
                if (hurtSpriteCount <= 0)
                {
                        sprite.material.SetFloat("_FlashAmount", 0);
                }
                else
                {
                        hurtSpriteCount -= Time.deltaTime;
                }
                if (hurtDurationImage.fillAmount >= healthImage.fillAmount)
                {
                        hurtDurationImage.fillAmount -= hurtDuration;
                }
                else
                {
                        hurtDurationImage.fillAmount = healthImage.fillAmount;
                }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
                if (collision.name == "Arrow(Clone)")
                {
                        if (collision.GetComponent<ArrowController>().parentCollision != playerCollider)
                        {
                                health -= receivedRangedDamage;
                                playerAnimator.SetFloat("Health", health);
                                healthImage.fillAmount = health / maxHealth;
                                HurtShader();
                                playerSound.PlayHurtAudio();
                        }
                }
                if (collision.name == "Fist(Clone)" && collision.transform.parent != transform)
                {
                        health -= receivedMeleeDamage;
                        playerAnimator.SetFloat("Health", health);
                        healthImage.fillAmount = health / maxHealth;
                        HurtShader();
                        playerSound.PlayHurtAudio();
                }
                if (health <= 0 && gameObject.name.Replace("Player", "") == gameSocketClient.yourIndex)
                {
                        gameManager.YourPlayerDead();
                }
        }

        void HurtShader()
        {
                sprite.material.SetFloat("_FlashAmount", 1);
                hurtSpriteCount = hurtSpiteDuration;
        }

        // when player dies, make it can not move
        void ChangeToStatic()
        {
                playerSound.PlayDeathAudio();
                Rigidbody2D playerRB = GetComponent<Rigidbody2D>();
                playerRB.bodyType = RigidbodyType2D.Static;
        }

        void Die()
        {
                Destroy(transform.gameObject);
        }
}
