using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistController : MonoBehaviour
{
        float range;
        Vector2 movement;
        GameObject parent;
        Animator parentAnimator;
        Collider2D parentCollision;
        Rigidbody2D fistRB;

        void Awake()
        {
                fistRB = GetComponent<Rigidbody2D>();
                parent = transform.parent.gameObject;
                range = parent.GetComponent<PlayerProperty>().fistRange;
                parentCollision = parent.GetComponent<Collider2D>();
                parentAnimator = parent.GetComponent<Animator>();

                movement.x = parentAnimator.GetFloat("Horizontal");
                movement.y = parentAnimator.GetFloat("Vertical");
                movement = movement.normalized;
                transform.localScale = Vector2.one * range;
                fistRB.rotation = Vector2.SignedAngle(new Vector2(0f, 1f), movement); // rotation of arrow
        }
}
