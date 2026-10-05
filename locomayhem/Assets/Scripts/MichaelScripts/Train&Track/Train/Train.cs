using UnityEngine;
using System.Collections;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Train otherTrain = collision.gameObject.GetComponent<Train>();
        
        if (otherTrain != null)
        {
            
            StopTrain();
            otherTrain.StopTrain();
        }
    }

    private IEnumerator WaitAndGo()
    {
        yield return new WaitForSeconds(5f);

        Destroy(gameObject);
        Debug.Log("5 seconds passed!");

        // use StartCoroutine(WaitAndGo()); to call it
        // hmmm
    }


    public void StartTrain()
    {
        moving = true;
    }

private void Update()
{
    if (!moving || targetNode == null)
        return;

    if (targetNode.occupyingTrain != null &&
        targetNode.occupyingTrain != this)
    {
        Crash(targetNode.occupyingTrain);
        return;
    }

    Vector3 direction = targetNode.transform.position - transform.position;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //transform.rotation = Quaternion.Euler(0, 0, angle);

    Quaternion targetRotation =
    Quaternion.Euler(0, 0, angle);
    transform.rotation = Quaternion.Lerp(
        transform.rotation,
        targetRotation,
        10f * Time.deltaTime);


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
        if (currentNode != null)
        {
            currentNode.occupyingTrain = null;
        }
        
        previousNode = currentNode;
        currentNode = targetNode;
        
        if (currentNode.occupyingTrain != null)
        {
            Crash(currentNode.occupyingTrain);
            return;
        }
        
        currentNode.occupyingTrain = this;

        Station station = currentNode.GetComponent<Station>();
        
        if (station != null)
        {
            if (station.HasTrain())
            {
                Crash(station.GetCurrentTrain());
                return;
            }
            
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

    private void Crash(Train otherTrain)
    {
        Debug.Log(name + " crashed into " + otherTrain.name);
        
        StopTrain();
        otherTrain.StopTrain();

        Destroy(otherTrain.gameObject, 5f);
        Destroy(gameObject, 5f);


    }
}