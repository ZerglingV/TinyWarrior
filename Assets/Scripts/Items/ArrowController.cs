using UnityEngine;

public class ArrowController : MonoBehaviour
{
        public GameObject arrowHitAudioSourcePrefab;
        public Collider2D parentCollision; // record parent collision to check whether arrow collided player is this arrow's parent

        float arrowSpeed;
        Vector2 movement;
        GameObject player;
        Animator parentAnimator;
        Rigidbody2D arrowRB;

        void Awake()
        {
                arrowRB = GetComponent<Rigidbody2D>();
                player = transform.parent.gameObject;
                arrowSpeed = player.GetComponent<PlayerProperty>().arrowSpeed;
                parentCollision = player.GetComponent<Collider2D>();
                parentAnimator = player.GetComponent<Animator>();

                movement.x = parentAnimator.GetFloat("Horizontal");
                movement.y = parentAnimator.GetFloat("Vertical");
                movement = movement.normalized;
                arrowRB.transform.position = new Vector2(arrowRB.transform.position.x, arrowRB.transform.position.y - 0.2f); // position of arrow
                arrowRB.transform.Rotate(0, 0, Vector2.SignedAngle(new Vector2(0f, 1f), movement)); // rotation of arrow
                transform.SetParent(GameObject.Find("ArrowCollection").transform);
        }

        void FixedUpdate()
        {
                Movement();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
                if (collision != parentCollision) // if collision is not parent player
                {
                        arrowHitAudioSourcePrefab.transform.position = transform.position;
                        Instantiate(arrowHitAudioSourcePrefab, transform.parent);
                        Destroy(gameObject);
                }
        }

        void Movement()
        {
                arrowRB.velocity = movement * arrowSpeed * Time.fixedDeltaTime;
        }
}
