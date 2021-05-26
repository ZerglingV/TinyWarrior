using UnityEngine;

public class PlayerController : MonoBehaviour
{
        public GameObject arrow;
        public GameObject fist;
        public bool canControl;

        PlayerProperty playerProperty;
        Rigidbody2D playerRB;
        Animator playerAnimator;
        Vector2 movement;
        float moveSpeed;
        bool canCreateArrow = true;
        bool canCreateFist = true;

        void Awake()
        {
                playerProperty = GetComponent<PlayerProperty>();
                moveSpeed = playerProperty.moveSpeed;
                playerRB = GetComponent<Rigidbody2D>();
                playerAnimator = GetComponent<Animator>();
        }

        void Update()
        {
                if (canControl)
                {
                        MeleeAttack();
                        RangedAttack();
                }
        }

        void FixedUpdate()
        {
                if (canControl)
                {
                        if (!playerAnimator.GetBool("MeleeAttack") && !playerAnimator.GetBool("RangedAttack"))
                        {
                                Movement();
                                SwitchAnimation();
                        }
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
                        if (!playerAnimator.GetBool("RangedAttack"))
                        {
                                playerAnimator.SetBool("MeleeAttack", true);
                                playerRB.velocity = Vector2.zero;
                        }
                }
        }

        void MeleeAttackStop()
        {
                playerAnimator.SetBool("MeleeAttack", false);
        }

        void CreateFist()
        {
                if (canCreateFist)
                {
                        canCreateFist = false;
                        Instantiate(fist, transform);  // initialize a arrow
                }
        }

        void AllowCreateFist()
        {
                canCreateFist = true;
        }

        #endregion

        #region -- RangedAttack --

        void RangedAttack()
        {
                if (Input.GetKeyDown(KeyCode.J))
                {
                        if (!playerAnimator.GetBool("MeleeAttack"))
                        {
                                playerAnimator.SetBool("RangedAttack", true);
                                playerRB.velocity = Vector2.zero;
                        }
                }
        }

        void RangedAttackStop()
        {
                playerAnimator.SetBool("RangedAttack", false);
        }

        void CreateArrow()
        {
                if (canCreateArrow)
                {
                        canCreateArrow = false;
                        Instantiate(arrow, transform);  // initialize a arrow
                }
        }

        void AllowCreateArrow()
        {
                canCreateArrow = true;
        }

        #endregion
}
