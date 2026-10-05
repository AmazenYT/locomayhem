using Unity.Netcode;
using UnityEngine;

public class TrainTest : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;

    [Header("Rail Network")]
    [SerializeField] private TrackNode currentNode;
    [SerializeField] private TrackNode targetNode;

    private TrackNode previousNode;

    private bool canMove;
    private bool isWaitingAtStation;

    private StationNode currentStation;

    private void Update()
    {
        if (!IsServer)
            return;

        if (!canMove)
            return;

        if (isWaitingAtStation)
            return;

        if (targetNode == null)
            return;

        MoveToTarget();
    }

    private void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetNode.transform.position,
            speed * Time.deltaTime);

        Vector2 direction =
            targetNode.transform.position - transform.position;

        if (direction != Vector2.zero)
        {
            float angle =
                Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation =
                Quaternion.Euler(0f, 0f, angle);
        }

        if (Vector2.Distance(
                transform.position,
                targetNode.transform.position) < 0.05f)
        {
            ArrivedAtNode();
        }
    }

    private void ArrivedAtNode()
    {
        previousNode = currentNode;
        currentNode = targetNode;

        // Snap exactly onto the node.
        transform.position = currentNode.transform.position;

        StationNode station =
            currentNode.GetComponent<StationNode>();

        if (station != null)
        {
            ArriveAtStation(station);
            return;
        }

        ChooseNextNode();
    }

    private void ArriveAtStation(StationNode station)
    {
        isWaitingAtStation = true;
        currentStation = station;
        targetNode = null;

        station.TrainArrived(this);

        Debug.Log(
            gameObject.name + " is waiting at " + station.gameObject.name);
    }

    /// <summary>
    /// Called by the station when a player selects a direction.
    /// This should only be called on the server.
    /// </summary>
    public bool DepartStation(TrackNode selectedNode)
    {
        if (!IsServer)
            return false;

        if (!isWaitingAtStation)
        {
            Debug.LogWarning("Train is not waiting at a station.");
            return false;
        }

        if (currentNode == null || selectedNode == null)
        {
            Debug.LogWarning("Current node or selected node is null.");
            return false;
        }

        // Make sure the chosen node is actually connected
        // to the station's TrackNode.
        if (!currentNode.connections.Contains(selectedNode))
        {
            Debug.LogWarning(
                selectedNode.gameObject.name +
                " is not connected to this station.");
            return false;
        }

        StationNode stationLeaving = currentStation;

        targetNode = selectedNode;
        isWaitingAtStation = false;
        currentStation = null;
        canMove = true;

        if (stationLeaving != null)
        {
            stationLeaving.TrainDeparted(this);
        }

        Debug.Log(
            gameObject.name +
            " departing towards " +
            selectedNode.gameObject.name);

        return true;
    }

    private void ChooseNextNode()
    {
        if (currentNode == null ||
            currentNode.connections.Count == 0)
        {
            targetNode = null;
            Debug.Log("Train reached the end of the track.");
            return;
        }

        if (currentNode.connections.Count == 1)
        {
            targetNode = currentNode.connections[0];
            return;
        }

        // Avoid immediately going backwards.
        foreach (TrackNode node in currentNode.connections)
        {
            if (node != previousNode)
            {
                targetNode = node;
                return;
            }
        }

        targetNode = currentNode.connections[0];
    }

    public void SetStartingNode(TrackNode startNode)
    {
        currentNode = startNode;
        transform.position = startNode.transform.position;

        if (currentNode.connections.Count > 0)
        {
            targetNode = currentNode.connections[0];
        }
        else
        {
            targetNode = null;
        }
    }

    public void SetMoving(bool move)
    {
        if (!IsServer)
            return;

        canMove = move;
    }

    public bool IsWaitingAtStation()
    {
        return isWaitingAtStation;
    }

    public TrackNode GetCurrentNode()
    {
        return currentNode;
    }
}