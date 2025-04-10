using UnityEngine;

public static class Extensions
{
    private static LayerMask layerMask = LayerMask.GetMask("Default");

    // Checks if the rigidbody is colliding with an object in a given direction.
    // For example, if you want to check if the player is touching the ground,
    // you can pass Vector2.down. If you want to check if the player is running
    // into a wall, you can pass Vector2.right or Vector2.left.
    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        if (rigidbody.isKinematic) {
            return false;
        }

        Vector2 edge = rigidbody.ClosestPoint(rigidbody.position + direction);
        float radius = (edge - rigidbody.position).magnitude / 2f;
        float distance = radius + 0.125f;

        Vector2 point = rigidbody.position + (direction.normalized * distance);
        Collider2D collider = Physics2D.OverlapCircle(point, radius, layerMask);
        return collider != null && collider.attachedRigidbody != rigidbody;
    }

    // Checks if the transform is facing another transform in a given direction.
    // For example, if you want to check if the player stomps on an enemy, you
    // would pass the player transform, the enemy transform, and Vector2.down.
    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        Vector2 direction = other.position - transform.position;
        return Vector2.Dot(direction.normalized, testDirection) > 0.25f;
    }

}
