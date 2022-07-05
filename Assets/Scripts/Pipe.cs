using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform connection;
    public KeyCode enterKeyCode = KeyCode.S;
    public Vector3 enterDirection = Vector3.down;
    public Vector3 exitDirection = Vector3.zero;
    public float animationDuration = 1f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (connection != null && other.CompareTag("Player"))
        {
            if (Input.GetKey(enterKeyCode)) {
                StartCoroutine(Enter(other.transform));
            }
        }
    }

    private IEnumerator Enter(Transform mario)
    {
        mario.GetComponent<PlayerMovement>().enabled = false;

        Vector3 enteredPosition = transform.position + enterDirection;

        yield return MoveToPosition(mario, enteredPosition);
        yield return new WaitForSeconds(animationDuration);

        if (exitDirection != Vector3.zero)
        {
            mario.position = connection.position - exitDirection;
            yield return MoveToPosition(mario, connection.position + exitDirection);
        }

        mario.position = connection.position;
        mario.GetComponent<PlayerMovement>().enabled = true;
    }

    private IEnumerator MoveToPosition(Transform mario, Vector3 endPosition)
    {
        float elapsed = 0f;

        Vector3 startPosition = mario.position;

        while (elapsed < animationDuration)
        {
            float t = elapsed / animationDuration;

            mario.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        mario.position = endPosition;
    }

}
