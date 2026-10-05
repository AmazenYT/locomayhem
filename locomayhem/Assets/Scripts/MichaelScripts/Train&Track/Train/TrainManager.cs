using UnityEngine;
using System.Collections.Generic;

public class TrainManager : MonoBehaviour
{
    [SerializeField] private GameObject trainPrefab;

    public static TrainManager Instance;

    [Header("Spawn Locations")]
    [SerializeField] private TrackNode[] startNodes;

    [Header("Spawned Trains")]
    private List<Train> trains = new();

    private void Awake()
    {
        Instance = this;
    }

    /*public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

        foreach (TrackNode node in startNodes)
        {
            SpawnTrain(node);
        }
    }*/

    private void Start()
    {

        foreach (TrackNode node in startNodes)
        {
            SpawnTrain(node);
        }
    }

private void SpawnTrain(TrackNode startNode)
{
    GameObject trainObject = Instantiate(
        trainPrefab,
        startNode.transform.position,
        Quaternion.identity);

    Train train = trainObject.GetComponent<Train>();

    train.SetStartingNode(startNode);

    Station station = startNode.GetComponent<Station>();
    
    if (station != null)
    {
        station.TrainArrived(train);
    }
    
    trains.Add(train);
}

}