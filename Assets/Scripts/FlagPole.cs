using System.Collections;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public GameObject flag;
    public Transform poleBottom;
    public Transform castle;
    public float speed = 8f;

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
        while (Vector2.Distance(transform.position, position) > 0.125f)
        {
            transform.position = Vector2.MoveTowards(transform.position, position, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = position;
    }

    private IEnumerator LevelCompleteSequence(Transform mario)
    {
        mario.GetComponent<PlayerMovement>().enabled = false;

        yield return MoveToPosition(mario, poleBottom.position);
        yield return MoveToPosition(mario, mario.position + Vector3.right);
        yield return MoveToPosition(mario, mario.position + Vector3.right + Vector3.down);
        yield return MoveToPosition(mario, castle.position);
        yield return new WaitForSeconds(0.25f);

        mario.gameObject.SetActive(false);

        FindObjectOfType<GameManager>().NextLevel();
    }

}
