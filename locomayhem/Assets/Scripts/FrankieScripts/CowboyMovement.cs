using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float stoppingDistance = 0.1f;

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool isMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = rb.position;
    }

    private void Update()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            SetTargetPosition();
        }
    }

    private void FixedUpdate()
    {
        MoveToTarget();
    }

    private void SetTargetPosition()
    {
        Vector3 mousePosition = Input.mousePosition;

        
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        
        targetPosition = new Vector2(worldPosition.x, worldPosition.y);

        isMoving = true;
    }

    private void MoveToTarget()
    {
        if (!isMoving)
            return;

        Vector2 currentPosition = rb.position;

       
        Vector2 newPosition = Vector2.MoveTowards(
            currentPosition,
            targetPosition,
            moveSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);

        
        if (Vector2.Distance(newPosition, targetPosition) <= stoppingDistance)
        {
            rb.MovePosition(targetPosition);
            isMoving = false;
        }
    }
}