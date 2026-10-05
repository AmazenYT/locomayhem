using UnityEngine;

public class Train : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private TrackNode previousNode;

    private TrackNode currentNode;
    private TrackNode targetNode;

    private bool moving = false;

    public void SetStartingNode(TrackNode startNode)
    {
        currentNode = startNode;

        if (currentNode.connections.Count > 0)
        {
            targetNode = currentNode.connections[0];
        }
    }


    public void StartTrain()
    {
        moving = true;
    }

    private void Update()
    {
        if (!moving)
            return;

        if (targetNode == null)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetNode.transform.position,
            speed * Time.deltaTime);

        if (Vector3.Distance(transform.position,
            targetNode.transform.position) < 0.05f)
        {
            ArriveAtNode();
        }
    }

    /*private void ArriveAtNode()
    {
        previousNode = currentNode;
        currentNode = targetNode;

        Station station = currentNode.GetComponent<Station>();

        if (station != null)
        {
            station.TrainArrived(this);
            return;
        }

        Junction junction = currentNode.GetComponent<Junction>();

        if (junction != null)
        {
            targetNode = junction.GetExit(currentNode);
            return;
        }

        if (currentNode.connections.Count > 0)
        {
            targetNode = currentNode.connections[0];
        }
    }*/
    private void ArriveAtNode()
{
    previousNode = currentNode;
    currentNode = targetNode;

    Station station = currentNode.GetComponent<Station>();

    if (station != null)
    {
        station.TrainArrived(this);
        return;
    }

    Junction junction = currentNode.GetComponent<Junction>();

    if (junction != null)
    {
        targetNode = junction.GetExit(previousNode);
        return;
    }

    if (currentNode.connections.Count > 0)
    {
        TrackNode nextNode = null;

        foreach (TrackNode node in currentNode.connections)
        {
            if (node != previousNode)
            {
                nextNode = node;
                break;
            }
        }

        // End of the line - reverse direction
        if (nextNode == null)
        {
            nextNode = previousNode;
        }

        targetNode = nextNode;
    }
}

    public void StopTrain()
    {
        moving = false;
    }

    public void LeaveStation(int exitIndex)
    {
        Debug.Log("LeaveStation called");
        Debug.Log("Exit chosen: " + exitIndex);

        if (currentNode.connections.Count <= exitIndex)
        {
            Debug.Log("Not enough connections!");
            return;
        }

        Debug.Log("Going to " + targetNode.name);
            

        targetNode = currentNode.connections[exitIndex];

        moving = true;
    }
}