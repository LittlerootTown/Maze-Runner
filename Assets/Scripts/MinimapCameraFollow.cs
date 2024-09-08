using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    public Transform player; // Assign your player's transform here

    void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        // Rotate with the player
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }

}
