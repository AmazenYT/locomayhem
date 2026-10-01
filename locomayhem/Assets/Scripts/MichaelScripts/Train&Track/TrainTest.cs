using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class TrainTest : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;

    [Header("Rail Network")]
    [SerializeField] private TrackNode currentNode;
    [SerializeField] private TrackNode targetNode;

    private bool isWaiting;

    private TrackNode previousNode;

    private void Update()
    {
        if (!IsServer)
        return;


        if (targetNode == null || isWaiting)
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

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (Vector2.Distance(transform.position,
            targetNode.transform.position) < 0.05f)
        {
            ArrivedAtNode();
        }
    }

    private void ArrivedAtNode()
    {
        previousNode = currentNode;
        currentNode = targetNode;
        
        StationNode station = currentNode.GetComponent<StationNode>();
        
        if (station != null)
        {
            StartCoroutine(StopAtStation(station));
            return;
        }
        
        ChooseNextNode();
    }

    private void ChooseNextNode()
    {
        if (currentNode.connections.Count == 0)
        {
            targetNode = null;
            Debug.Log("Train reached end of track.");
            return;
        }

        // If there's only one connection
        if (currentNode.connections.Count == 1)
        {
            targetNode = currentNode.connections[0];
            return;
        }

        // Avoid immediately going backwards
        foreach (TrackNode node in currentNode.connections)
        {
            if (node != previousNode)
            {
                targetNode = node;
                return;
            }
        }

        // Fallback
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
    }

    private IEnumerator StopAtStation(StationNode station)
    {
        isWaiting = true;
        
        station.TrainArrived();
        
        yield return new WaitForSeconds(station.stopTime);
        
        station.TrainDeparted();
        isWaiting = false;
        
        ChooseNextNode();
    }

    
}