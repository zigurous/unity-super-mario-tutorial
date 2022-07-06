using UnityEngine;

public class MysteryBlock : MonoBehaviour
{
    public PowerUp powerUp;
    public Sprite emptyBlock;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            Reveal();
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
