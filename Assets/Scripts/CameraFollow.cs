using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Drag Player
    private Vector3 offset = new Vector3(0, 0, -10f);

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }
        transform.position = target.position + offset;
    }
}