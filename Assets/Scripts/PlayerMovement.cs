using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    private new Rigidbody2D rigidbody;

    public float moveSpeed = 8f;
    public float acceleration = 0.75f;
    public float deceleration = 0.25f;
    public float maxJumpHeight = 4.5f;
    public float maxJumpTime = 1f;

    private Vector2 velocity;
    private bool jumping;
    private float damping;

    public Vector2 Velocity => velocity;
    public bool IsJumping => jumping;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HorizontalMovement();

        if (Raycast(Vector2.down)) {
            GroundedMovement();
        } else {
            AirborneMovement();
        }

        ApplyGravity();
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector3 leftEdge = camera.ScreenToWorldPoint(Vector3.zero);

        position += velocity * Time.fixedDeltaTime;
        position.x = Mathf.Max(position.x, leftEdge.x + 0.5f);

        rigidbody.MovePosition(position);
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = camera.transform.position;
        cameraPosition.x = Mathf.Max(cameraPosition.x, transform.position.x);
        camera.transform.position = cameraPosition;
    }

    private void HorizontalMovement()
    {
        // get horizontal input
        float axis = Input.GetAxis("Horizontal") * moveSpeed;

        // accelerate / decelerate
        if (Mathf.Abs(axis) > Mathf.Abs(velocity.x)) {
            velocity.x = Mathf.SmoothDamp(velocity.x, axis, ref damping, acceleration);
        } else {
            velocity.x = Mathf.SmoothDamp(velocity.x, axis, ref damping, deceleration);
        }

        // check if running into a wall
        if (Raycast(Vector2.right * velocity.x)) {
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
        if (Raycast(Vector2.up)) {
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

        // terminal velocity
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private bool Raycast(Vector2 direction)
    {
        float pixelSize = 1f / 16f;
        float distance = pixelSize * 2f;
        float radius = (1f - distance) / 2f;

        LayerMask layerMask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, direction.normalized, distance, layerMask);

        return hit.collider != null && hit.rigidbody != this.rigidbody;
    }

}
