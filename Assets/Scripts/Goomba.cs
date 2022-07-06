using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite flatSprite;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<EntityMovement>().enabled = false;
            GetComponent<AnimatedSprite>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = flatSprite;
            Destroy(gameObject, 0.5f);
        }
    }

}
