using UnityEngine;

public class Carriage : MonoBehaviour
{
    public Transform target;
    public float followDistance = 1f;
    public float moveSpeed = 5f;

    private void Update()
    {
        Vector3 direction = target.position - transform.position;

        if (direction.magnitude > followDistance)
        {
            transform.position += direction.normalized * moveSpeed * Time.deltaTime;
        }

        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}