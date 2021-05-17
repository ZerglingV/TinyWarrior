using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
        public GameObject arrowHitAudioSourcePrefab;

        float arrowSpeed;
        Vector2 movement;
        GameObject parent;
        Collider2D parentCollision;
        Animator parentAnimator;
        Rigidbody2D arrowRB;

        void Awake()
        {
                arrowRB = GetComponent<Rigidbody2D>();
                parent = transform.parent.gameObject;
                arrowSpeed = parent.GetComponent<PlayerProperty>().arrowSpeed;
                parentCollision = parent.GetComponent<Collider2D>();
                parentAnimator = parent.GetComponent<Animator>();

                movement.x = parentAnimator.GetFloat("Horizontal");
                movement.y = parentAnimator.GetFloat("Vertical");
                movement = movement.normalized;
                arrowRB.position = new Vector2(arrowRB.transform.position.x, arrowRB.transform.position.y - 0.2f); // position of arrow
                arrowRB.rotation = Vector2.SignedAngle(new Vector2(0f, 1f), movement); // rotation of arrow
        }

        void FixedUpdate()
        {
                Movement();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
                if (collision != parentCollision) // if collission is not parent player
                {
                        arrowHitAudioSourcePrefab.transform.position = transform.position;
                        Instantiate(arrowHitAudioSourcePrefab);
                        Destroy(this.gameObject);
                }
        }

        void Movement()
        {
                arrowRB.velocity = movement * arrowSpeed * Time.fixedDeltaTime;
        }
}
