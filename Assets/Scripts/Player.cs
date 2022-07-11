using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    private PlayerSpriteRenderer activeRenderer;

    public CapsuleCollider2D capsuleCollider { get; private set; }
    public DeathAnimation deathAnimation { get; private set; }

    public bool big => bigRenderer.enabled;
    public bool dead => deathAnimation.enabled;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        deathAnimation = GetComponent<DeathAnimation>();
    }

    public void Hit()
    {
        if (!dead)
        {
            if (big) {
                Shrink();
            } else {
                Death();
            }
        }
    }

    public void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }

    public void Grow()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

        StartCoroutine(ScaleAnimation());
    }

    public void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        activeRenderer = smallRenderer;

        capsuleCollider.size = new Vector2(1f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);

        StartCoroutine(ScaleAnimation());
    }

    private IEnumerator ScaleAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            if (Time.frameCount % 4 == 0)
            {
                smallRenderer.enabled = !smallRenderer.enabled;
                bigRenderer.enabled = !smallRenderer.enabled;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
    }

}
