using UnityEngine;

public class MysteryBlock : MonoBehaviour
{
    public PowerUp powerUp;
    public Sprite emptyBlock;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.DotTest(transform, Vector2.up)) {
                Reveal();
            }
        }
    }

    private void Reveal()
    {
        GetComponent<SpriteRenderer>().sprite = emptyBlock;

        if (powerUp.type == PowerUp.Type.Coin) {
            GameManager.Instance.AddCoin();
        } else {
            Instantiate(powerUp, transform.position + Vector3.up, Quaternion.identity);
        }

        Destroy(this);
    }

}
