using System.Collections;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.AddCoin();

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        yield return MoveToPosition(transform.position + Vector3.up * 2f);
        yield return MoveToPosition(transform.position + Vector3.down * 2f);

        Destroy(gameObject);
    }

    private IEnumerator MoveToPosition(Vector3 endPosition)
    {
        float elapsed = 0f;
        float duration = 0.25f;

        Vector3 startPosition = transform.position;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = endPosition;
    }

}
