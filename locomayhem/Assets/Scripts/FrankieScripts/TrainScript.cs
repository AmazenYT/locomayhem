using UnityEngine;
using UnityEngine.AI;

public class TrainScript : MonoBehaviour
{
    public NavMeshAgent trainAgent;

    public Transform junctionWaypoint;
    public Transform station1Waypoint;
    public Transform station2Waypoint;

    public float trainSpeed = 5f;

    void Start()
    {
        trainAgent.speed = trainSpeed;
        trainAgent.isStopped = true;
        trainAgent.updateRotation = false;
        trainAgent.updateUpAxis = false;
    }

    public void StartTrain()
    {
        trainAgent.isStopped = false;
        trainAgent.SetDestination(junctionWaypoint.position);
    }

    public void Station1()
    {
        trainAgent.isStopped = false;
        trainAgent.SetDestination(station1Waypoint.position);
    }

    public void Station2()
    {
        trainAgent.isStopped = false;
        trainAgent.SetDestination(station2Waypoint.position);
    }
}