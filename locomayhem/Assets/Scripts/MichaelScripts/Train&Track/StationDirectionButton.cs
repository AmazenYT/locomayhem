using Unity.Netcode;
using UnityEngine;

public class StationDirectionButton : NetworkBehaviour
{
    [Header("Direction Settings")]
    [SerializeField] private StationNode station;
    [SerializeField] private TrackNode directionNode;

    [Header("Button Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Direction button clicked: " + gameObject.name);

        if (!IsSpawned)
        {
            Debug.LogWarning(
                gameObject.name +
                " is not spawned as a NetworkObject.");

            return;
        }

        if (NetworkManager.Singleton == null)
        {
            Debug.LogWarning("NetworkManager.Singleton is null.");
            return;
        }

        if (!NetworkManager.Singleton.IsClient)
        {
            Debug.LogWarning(
                "This game instance is not running as a client.");

            return;
        }

        SelectDirectionRpc();
    }

    [Rpc(SendTo.Server)]
    private void SelectDirectionRpc()
    {
        Debug.Log(
            "Direction button RPC reached the server: " +
            gameObject.name);

        if (station == null)
        {
            Debug.LogWarning(
                gameObject.name +
                " does not have a Station assigned.");

            return;
        }

        if (directionNode == null)
        {
            Debug.LogWarning(
                gameObject.name +
                " does not have a Direction Node assigned.");

            return;
        }

        if (!station.HasWaitingTrain())
        {
            Debug.LogWarning(
                "There is no train waiting at station: " +
                station.gameObject.name);

            return;
        }

        Debug.Log(
            "Sending train from " +
            station.gameObject.name +
            " towards " +
            directionNode.gameObject.name);

        station.SelectDirection(directionNode);

        if (spriteRenderer != null)
        {
            SetPressedColorRpc();
        }
    }

    [Rpc(SendTo.Everyone)]
    private void SetPressedColorRpc()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.green;
        }
    }
}
