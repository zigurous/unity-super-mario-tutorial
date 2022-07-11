using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform connection;
    public KeyCode enterKeyCode = KeyCode.S;
    public Vector3 enterDirection = Vector3.down;
    public Vector3 exitDirection = Vector3.zero;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (connection != null && other.CompareTag("Player"))
        {
            if (Input.GetKey(enterKeyCode)) {
                StartCoroutine(Enter(other.transform));
            }
        }
    }

    private IEnumerator Enter(Transform player)
    {
        player.GetComponent<Player>().enabled = false;

        Vector3 enteredPosition = transform.position + enterDirection;

        yield return MoveToPosition(player, enteredPosition);
        yield return new WaitForSeconds(1f);

        Camera.main.GetComponent<SideScrolling>().SetUnderground(connection.position.y < 0f);

        if (exitDirection != Vector3.zero)
        {
            player.position = connection.position - exitDirection;
            yield return MoveToPosition(player, connection.position + exitDirection);
        }

        player.position = connection.position;
        player.GetComponent<Player>().enabled = true;
    }

    private IEnumerator MoveToPosition(Transform player, Vector3 endPosition)
    {
        float elapsed = 0f;
        float duration = 1f;

        Vector3 startPosition = player.position;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            player.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        player.position = endPosition;
    }

}
