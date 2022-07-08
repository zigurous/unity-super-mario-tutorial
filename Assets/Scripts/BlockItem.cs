using System.Collections;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        Collider2D collider = GetComponent<Collider2D>();

        spriteRenderer.enabled = false;
        rigidbody.isKinematic = true;
        collider.isTrigger = true;

        yield return new WaitForSeconds(0.25f);

        spriteRenderer.enabled = true;

        float elapsed = 0f;
        float duration = 0.5f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + Vector3.up;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        rigidbody.isKinematic = false;
        collider.isTrigger = false;
    }

}
