using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float stoppingDistance = 0.1f;

    [Header("Dynamite")]
    [SerializeField] private GameObject dynamitePrefab;
    [SerializeField] private float dynamitePlacementDistance = 1.5f;
    [SerializeField] private LayerMask railLayer;
    [SerializeField] private Tilemap railTilemap;

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
        // Point and click movement
        if (Input.GetMouseButtonDown(0))
        {
            SetTargetPosition();
        }

        // Place dynamite
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPlaceDynamite();
        }
    }

    private void FixedUpdate()
    {
        MoveToTarget();
    }

    private void SetTargetPosition()
    {
        Vector3 mousePosition = Input.mousePosition;

        Vector3 worldPosition =
            Camera.main.ScreenToWorldPoint(mousePosition);

        targetPosition = new Vector2(
            worldPosition.x,
            worldPosition.y
        );

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

    private void TryPlaceDynamite()
    {
        // Make sure the Tilemap has been assigned
        if (railTilemap == null)
        {
            Debug.LogError("Rail Tilemap has not been assigned!");
            return;
        }

        // Make sure the dynamite prefab has been assigned
        if (dynamitePrefab == null)
        {
            Debug.LogError("Dynamite Prefab has not been assigned!");
            return;
        }

        // Check that there is a rail collider nearby
        Collider2D rail = Physics2D.OverlapCircle(
            transform.position,
            dynamitePlacementDistance,
            railLayer
        );

        if (rail == null)
        {
            Debug.Log("No rail nearby.");
            return;
        }

        // Convert the cowboy's position to a Tilemap cell
        Vector3Int playerCell =
            railTilemap.WorldToCell(transform.position);

        Vector3Int closestCell = playerCell;
        float closestDistance = float.MaxValue;

        // Search the cells around the cowboy
        for (int x = -2; x <= 2; x++)
        {
            for (int y = -2; y <= 2; y++)
            {
                Vector3Int cell =
                    new Vector3Int(
                        playerCell.x + x,
                        playerCell.y + y,
                        0
                    );

                // Does this cell actually contain a rail tile?
                if (!railTilemap.HasTile(cell))
                    continue;

                // Get the exact center of this tile
                Vector3 cellCenter =
                    railTilemap.GetCellCenterWorld(cell);

                // How far is the tile from the cowboy?
                float distance =
                    Vector2.Distance(
                        transform.position,
                        cellCenter
                    );

                // Is this the closest rail tile we've found?
                if (distance < closestDistance &&
                    distance <= dynamitePlacementDistance)
                {
                    closestDistance = distance;
                    closestCell = cell;
                }
            }
        }

        // We didn't find an actual rail tile
        if (!railTilemap.HasTile(closestCell))
        {
            Debug.Log("No rail tile found nearby.");
            return;
        }

        // Put the dynamite exactly in the center of the rail tile
        Vector3 dynamitePosition =
            railTilemap.GetCellCenterWorld(closestCell);

        // Create dynamite
        GameObject dynamiteObject = Instantiate(
            dynamitePrefab,
            dynamitePosition,
            Quaternion.identity
        );

        // Get Dynamite component
        Dynamite dynamite =
            dynamiteObject.GetComponent<Dynamite>();

        if (dynamite == null)
        {
            Debug.LogError(
                "The Dynamite prefab does not have a Dynamite script!"
            );

            Destroy(dynamiteObject);
            return;
        }

        // Give Dynamite the Tilemap AND exact tile to destroy
        dynamite.Initialize(
            railTilemap,
            closestCell
        );

        Debug.Log(
            "Dynamite placed on rail tile: " + closestCell
        );
    }
}