using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private Vector2 velocity;
    private float inputAxis;

    public float moveSpeed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;
    public float jumpVelocity => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow(maxJumpTime / 2f, 2f);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || inputAxis != 0f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f);
    public bool falling => velocity.y < 0f && !grounded;

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

        idle.enabled = true;
        run.enabled = false;
        jump.enabled = false;
        slide.enabled = false;
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
        // move mario based on his velocity
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        // clamp within the screen bounds
        Vector3 leftEdge = camera.ScreenToWorldPoint(Vector3.zero);
        Vector3 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rigidbody.MovePosition(position);
    }

    private void LateUpdate()
    {
        // enable/disable sprites based on animation state
        jump.enabled = jumping;
        slide.enabled = !jumping && running && sliding;
        run.enabled = !jumping && running && !sliding;
        idle.enabled = !jumping && !running && !sliding;
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
            velocity.y = jumpVelocity;
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
        // check if falling
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        // apply gravity and terminal velocity
        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // check if mario is falling onto the head of an enemy
        if (falling && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.SetActive(false);

            // bounce off enemy head
            velocity.y = jumpVelocity / 2f;
            jumping = true;
        }
    }

}
