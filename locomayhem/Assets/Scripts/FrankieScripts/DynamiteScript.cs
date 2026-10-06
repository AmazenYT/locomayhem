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

        if (railTilemap != null)
        {
            // Make absolutely sure there is a tile here
            if (railTilemap.HasTile(railCell))
            {
                // Remove the rail tile
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
        else
        {
            Debug.LogError(
                "Dynamite has no Rail Tilemap reference!"
            );
        }

        Destroy(gameObject);
    }
}