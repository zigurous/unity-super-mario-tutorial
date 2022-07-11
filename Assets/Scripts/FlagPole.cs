using System.Collections;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public GameObject flag;
    public Transform poleBottom;
    public Transform castle;
    public float speed = 6f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MoveToPosition(flag.transform, poleBottom.position));
            StartCoroutine(LevelCompleteSequence(other.transform));
        }
    }

    private IEnumerator MoveToPosition(Transform transform, Vector3 position)
    {
        while (Vector3.Distance(transform.position, position) > 0.125f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = position;
    }

    private IEnumerator LevelCompleteSequence(Transform player)
    {
        player.GetComponent<Player>().enabled = false;

        yield return MoveToPosition(player, poleBottom.position);
        yield return MoveToPosition(player, player.position + Vector3.right);
        yield return MoveToPosition(player, player.position + Vector3.right + Vector3.down);
        yield return MoveToPosition(player, castle.position);

        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        GameManager.Instance.NextLevel();
    }

}
