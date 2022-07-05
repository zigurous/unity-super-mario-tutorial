using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityMovement : MonoBehaviour
{
    public float speed = 2f;
    public Vector2 initialDirection = Vector2.left;

    private new Rigidbody2D rigidbody;
    private Vector2 direction;
    private Vector2 velocity;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enabled = false;
    }

    private void Start()
    {
        direction = initialDirection;
    }

    private void OnBecameVisible()
    {
        enabled = !EditorApplication.isPaused;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void FixedUpdate()
    {
        velocity.x = direction.x * speed;
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;

        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);

        if (rigidbody.Raycast(direction)) {
            direction = -direction;
        }

        if (rigidbody.Raycast(Vector2.down)) {
            velocity.y = 0f;
        }
    }

}
