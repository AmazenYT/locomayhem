using UnityEngine;
using UnityEngine.Tilemaps;

public class Dynamite : MonoBehaviour
{
    [SerializeField] private float explosionDelay = 5f;

    private Tilemap railTilemap;
    private Vector3Int railCell;

    public void Initialize(
        Tilemap tilemap,
        Vector3Int cell
    )
    {
        railTilemap = tilemap;
        railCell = cell;
    }

    private void Start()
    {
        Invoke(nameof(Explode), explosionDelay);
    }

    private void Explode()
    {
        Debug.Log("BOOM!");

        // Destroy the rail tile
        DestroyRailTile();

        // Destroy the TrackNode on that rail
        DestroyTrackNode();

        // Destroy the dynamite itself
        Destroy(gameObject);
    }

    private void DestroyRailTile()
    {
        if (railTilemap == null)
        {
            Debug.LogError(
                "Dynamite has no Rail Tilemap reference!"
            );

            return;
        }

        if (railTilemap.HasTile(railCell))
        {
            railTilemap.SetTile(railCell, null);

            Debug.Log(
                "Destroyed rail tile: " + railCell
            );
        }
        else
        {
            Debug.LogWarning(
                "No rail tile exists at: " + railCell
            );
        }
    }

    private void DestroyTrackNode()
    {
        // Get the world position of the rail cell
        Vector3 railWorldPosition =
            railTilemap.GetCellCenterWorld(railCell);

        Transform[] allTransforms =
        FindObjectsByType<Transform>(
        FindObjectsInactive.Include
    );

        foreach (Transform currentTransform in allTransforms)
        {
            // Ignore ourselves
            if (currentTransform == transform)
                continue;

            // Only look for TrackNode objects
            if (!currentTransform.name.StartsWith("TrackNode"))
                continue;

            // Check how far the node is from the destroyed rail tile
            float distance = Vector2.Distance(
                currentTransform.position,
                railWorldPosition
            );

            // Node is close enough to this rail tile
            if (distance <= 0.5f)
            {
                Debug.Log(
                    "Destroyed TrackNode: " +
                    currentTransform.name
                );

                Destroy(currentTransform.gameObject);
            }
        }
    }
}