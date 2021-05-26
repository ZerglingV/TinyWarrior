using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
        public float moveSpeed = 500f;

        Rigidbody2D playerRB;
        Animator playerAnimator;
        Vector2 movement;

        void Awake()
        {
                playerRB = GetComponent<Rigidbody2D>();
                playerAnimator = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
                Movement();
                SwitchAnimation();
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

}
