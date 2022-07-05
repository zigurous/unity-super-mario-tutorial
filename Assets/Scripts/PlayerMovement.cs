using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    private Vector2 velocity;
    private bool grounded;
    private bool jumping;

    [Header("Physics")]
    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;

    [Header("Sprites")]
    public SpriteRenderer idle;
    public SpriteRenderer run;
    public SpriteRenderer jump;
    public SpriteRenderer slide;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        collider.enabled = true;
        velocity = Vector2.zero;
    }

    private void OnDisable()
    {
        collider.enabled = false;
        velocity = Vector2.zero;

        idle.gameObject.SetActive(true);
        run.gameObject.SetActive(false);
        jump.gameObject.SetActive(false);
        slide.gameObject.SetActive(false);
    }

    private void Update()
    {
        HorizontalMovement();

        grounded = rigidbody.Raycast(Vector2.down);

        if (grounded) {
            GroundedMovement();
        } else {
            AirborneMovement();
        }

        ApplyGravity();
    }

    private void FixedUpdate()
    {
        Vector3 leftEdge = camera.ScreenToWorldPoint(Vector3.zero);
        Vector3 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rigidbody.MovePosition(position);
    }

    private void LateUpdate()
    {
        float axis = Input.GetAxis("Horizontal");
        float velocityX = Mathf.Abs(velocity.x);

        bool running = velocityX > 0.25f || axis != 0f;
        bool slidingLeft = axis > 0f && velocity.x < 0f;
        bool slidingRight = axis < 0f && velocity.x > 0f;
        bool sliding = slidingLeft || slidingRight;

        jump.gameObject.SetActive(jumping);
        slide.gameObject.SetActive(!jumping && running && sliding);
        run.gameObject.SetActive(!jumping && running && !sliding);
        idle.gameObject.SetActive(!jumping && !running && !sliding);
    }

    private void HorizontalMovement()
    {
        // accelerate / decelerate
        float axis = Input.GetAxis("Horizontal");
        velocity.x = Mathf.MoveTowards(velocity.x, axis * moveSpeed, moveSpeed * Time.deltaTime);

        // check if running into a wall
        if (rigidbody.Raycast(Vector2.right * velocity.x)) {
            velocity.x = 0f;
        }

        // flip sprite to face direction
        if (velocity.x > 0f) {
            transform.eulerAngles = Vector3.zero;
        } else if (velocity.x < 0f) {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void GroundedMovement()
    {
        // prevent gravity from infinitly building up
        velocity.y = Mathf.Max(velocity.y, 0f);
        jumping = velocity.y > 0f;

        // perform jump
        if (Input.GetButtonDown("Jump"))
        {
            // calculate jump velocity
            float timeToApex = maxJumpTime / 2f;
            velocity.y = (2f * maxJumpHeight) / timeToApex;
            jumping = true;
        }
    }

    private void AirborneMovement()
    {
        // check if bonked head
        if (velocity.y > 0f && rigidbody.Raycast(Vector2.up)) {
            velocity.y = 0f;
        }
    }

    private void ApplyGravity()
    {
        // calculate gravity
        float timeToApex = maxJumpTime / 2f;
        float gravity = (-2f * maxJumpHeight) / Mathf.Pow(timeToApex, 2f);

        // check if falling
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        // apply gravity
        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

}
