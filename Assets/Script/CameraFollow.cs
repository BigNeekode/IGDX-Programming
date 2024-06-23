using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

            // Smoothly move the camera towards the desired position
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        }
    }
}