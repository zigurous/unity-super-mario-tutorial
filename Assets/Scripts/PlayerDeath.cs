using System.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public SpriteRenderer deathSpriteRenderer;
    public float animationDuration = 3f;

    private void Start()
    {
        StartCoroutine(DeathAnimation());
    }

    private IEnumerator DeathAnimation()
    {
        float elapsed = 0f;

        // disable all player movement
        PlayerMovement movement = GetComponent<PlayerMovement>();
        movement.enabled = false;

        // disable collider so mario can fall through the floor
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        // swap sprite to death sprite
        movement.idle.enabled = false;
        movement.run.enabled = false;
        movement.jump.enabled = false;
        movement.slide.enabled = false;
        deathSpriteRenderer.enabled = true;

        // jump at 2/3 power
        Vector3 velocity = Vector3.up * movement.jumpVelocity * 0.66f;
        Vector3 bottomEdge = Camera.main.ScreenToWorldPoint(Vector3.zero);

        // apply gravity until the animation is finished
        while (elapsed < animationDuration)
        {
            transform.position += velocity * Time.deltaTime;
            velocity.y += movement.gravity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        GameManager.Instance.ResetLevel();
    }

}
