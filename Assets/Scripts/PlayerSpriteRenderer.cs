using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSpriteRenderer : MonoBehaviour
{
    private PlayerMovement movement;
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public AnimatedSprite run;

    private void Awake()
    {
        movement = GetComponentInParent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        float speed = Mathf.Abs(movement.getVelocityX());

        run.enabled = movement.running;

        // By checking if the speed is greater than or equal to Mathf.Epsilon,
        // we can apply the default framerate of the run animation to cutscenes if needed.
        if(speed >= Mathf.Epsilon) {
            run.framerate = 1 / (6f * Mathf.Sqrt(speed));
        }else {
            run.framerate = 1 / 6f;
        }

        if (movement.jumping) {
            spriteRenderer.sprite = jump;
        } else if (movement.sliding) {
            spriteRenderer.sprite = slide;
        } else if (!movement.running) {
            spriteRenderer.sprite = idle;
        }
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
        run.enabled = false;
    }
}
