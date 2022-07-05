using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites;
    public float framerate = 1f / 6f;

    private SpriteRenderer spriteRenderer;
    private int frame;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enabled = false;
    }

    private void OnBecameVisible()
    {
        enabled = !EditorApplication.isPaused;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(Animate), framerate, framerate);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Animate()
    {
        frame++;

        if (frame >= sprites.Length) {
            frame = 0;
        }

        if (frame >= 0 && frame < sprites.Length) {
            spriteRenderer.sprite = sprites[frame];
        }
    }

}
