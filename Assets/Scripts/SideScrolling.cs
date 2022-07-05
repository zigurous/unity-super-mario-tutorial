using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SideScrolling : MonoBehaviour
{
    private new Camera camera;
    private Transform player;

    public float height = 6.5f;
    public float undergroundHeight = -9.5f;
    public float undergroundThreshold = -2f;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        player = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;

        // track the player moving to the right
        cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x);

        // set height offset for above/below ground
        if (player.position.y < undergroundThreshold) {
            cameraPosition.y = undergroundHeight;
        } else {
            cameraPosition.y = height;
        }

        transform.position = cameraPosition;
    }

}
