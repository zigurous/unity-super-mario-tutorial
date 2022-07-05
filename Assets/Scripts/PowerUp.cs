using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Starman,
    }

    public Type type;

    private void Collect()
    {
        switch (type)
        {
            case Type.Coin:
                FindObjectOfType<GameManager>().AddCoin();
                break;
            case Type.ExtraLife:
                FindObjectOfType<GameManager>().AddLife();
                break;
            case Type.MagicMushroom:
                // TODO
                break;
            case Type.Starman:
                // TODO
                break;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            Collect();
        }
    }

}
