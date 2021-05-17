using UnityEngine;

public class PlayerController : MonoBehaviour
{
        public GameObject arrow;
        public GameObject fist;

        Rigidbody2D playerRB;
        Animator playerAnimator;
        Vector2 movement;
        bool canCreateArrow = true;
        bool canCreateFist = true;
        float moveSpeed;

        void Awake()
        {
                moveSpeed = GetComponent<PlayerProperty>().moveSpeed;
                playerRB = GetComponent<Rigidbody2D>();
                playerAnimator = GetComponent<Animator>();
        }

        void Update()
        {
                MeleeAttack();
                RangedAttack();
        }

        void FixedUpdate()
        {
                if (!playerAnimator.GetBool("MeleeAttack") && !playerAnimator.GetBool("RangedAttack"))
                {
                        Movement();
                        SwitchAnimation();
                }
        }

        void Movement()
        {
                movement.x = Input.GetAxis("Horizontal");
                movement.y = Input.GetAxis("Vertical");
                if (movement.magnitude > 1)
                {
                        playerRB.velocity = movement.normalized * moveSpeed * Time.fixedDeltaTime;
                }
                else
                {
                        playerRB.velocity = movement * moveSpeed * Time.fixedDeltaTime;
                }
        }

        void SwitchAnimation()
        {
                if (movement != Vector2.zero) // using value of movement to switch blend tree of 'Idle' animation
                {
                        playerAnimator.SetFloat("Horizontal", movement.x);
                        playerAnimator.SetFloat("Vertical", movement.y);
                }
                if (movement.magnitude > 1)
                {
                        playerAnimator.SetFloat("Speed", movement.normalized.magnitude);
                }
                else
                {
                        playerAnimator.SetFloat("Speed", movement.magnitude);
                }
        }

        #region -- MeleeAttack --
        void MeleeAttack()
        {
                if (Input.GetKeyDown(KeyCode.K))
                {
                        playerAnimator.SetBool("MeleeAttack", true);
                        playerRB.velocity = Vector2.zero;
                }
        }

        void MeleeAttackStop()
        {
                playerAnimator.SetBool("MeleeAttack", false);
                canCreateFist = true;
                Destroy(GameObject.Find("Fist(Clone)"));
        }

        void CreateFist()
        {
                if (canCreateFist)
                {
                        GameObject.Instantiate(fist, transform);  // initialize a arrow
                        canCreateFist = false;
                }
        }
        #endregion

        #region -- RangedAttack --
        void RangedAttack()
        {
                if (Input.GetKeyDown(KeyCode.J))
                {
                        playerAnimator.SetBool("RangedAttack", true);
                        playerRB.velocity = Vector2.zero;
                }
        }

        void RangedAttackStop()
        {
                playerAnimator.SetBool("RangedAttack", false);
                canCreateArrow = true;
        }

        void CreateArrow()
        {
                if (canCreateArrow)
                {
                        Instantiate(arrow, transform);  // initialize a arrow
                        canCreateArrow = false;
                }
        }
        #endregion
}
