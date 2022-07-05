using System.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public Sprite deathSprite;

    private void OnEnable()
    {
        StartCoroutine(Animate());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Animate()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = deathSprite;

        yield break; // TODO
    }

}
