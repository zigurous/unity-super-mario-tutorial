using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MarioSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PlayerMovement movement;

    public Sprite[] run;
    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public Sprite death;

    private int runIndex;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        float rate = 1f / 6f;
        InvokeRepeating(nameof(AnimateRun), rate, rate);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void LateUpdate()
    {
        float velocity = movement.Velocity.x;

        if (movement.IsJumping)
        {
            spriteRenderer.sprite = jump;
        }
        else if (Mathf.Abs(velocity) > 0.25f)
        {
            float axis = Input.GetAxis("Horizontal");

            if ((axis > 0f && velocity < 0f) || (axis < 0f && velocity > 0f)) {
                spriteRenderer.sprite = slide;
            } else {
                spriteRenderer.sprite = run[runIndex];
            }
        }
        else
        {
            spriteRenderer.sprite = idle;
        }
    }

    private void AnimateRun()
    {
        runIndex++;

        if (runIndex >= run.Length) {
            runIndex = 0;
        }
    }

}
