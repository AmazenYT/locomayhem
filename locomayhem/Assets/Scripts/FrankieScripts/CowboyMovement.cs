using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float stoppingDistance = 0.1f;

    private Rigidbody2D rb;
    private Vector2 targetPosition;
    private bool isMoving;

    [SerializeField] public int visibility = 3;
    public Tilemap Fogtilemap;

    private void Awake()
    {
        Fogtilemap = GameObject.FindGameObjectWithTag("Fog").GetComponent<Tilemap>();
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
        UpdateFog();

        
        if (Vector2.Distance(newPosition, targetPosition) <= stoppingDistance)
        {
            rb.MovePosition(targetPosition);
            isMoving = false;
        }
    }

    private void UpdateFog()
    {
        Vector3Int currentplayerpos = Fogtilemap.WorldToCell(transform.position);

        for (int i = -visibility; i <= visibility;  i++)
        {
            for (int j = -visibility; j <= visibility; j++)
            {
                Fogtilemap.SetTile(currentplayerpos + new Vector3Int(i, j, 0), null);
            }
        }
    }
}