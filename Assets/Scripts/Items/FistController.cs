using UnityEngine;

public class FistController : MonoBehaviour
{
        float range;
        Vector2 movement;
        GameObject parent;
        Animator parentAnimator;
        Rigidbody2D fistRB;
        int durationTime;

        void Awake()
        {
                fistRB = GetComponent<Rigidbody2D>();
                parent = transform.parent.gameObject;
                range = parent.GetComponent<PlayerProperty>().fistRange;
                parentAnimator = parent.GetComponent<Animator>();

                movement.x = parentAnimator.GetFloat("Horizontal");
                movement.y = parentAnimator.GetFloat("Vertical");
                movement = movement.normalized;
                transform.localScale = Vector2.one * range;
                fistRB.rotation = Vector2.SignedAngle(new Vector2(0f, 1f), movement); // rotation of arrow
        }

        void Update()
        {
                if (++durationTime >= 14)
                {
                        Destroy(this.gameObject);
                }
        }
}
