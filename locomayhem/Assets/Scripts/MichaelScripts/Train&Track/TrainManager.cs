using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class TrainManager : NetworkBehaviour
{
    [SerializeField] private GameObject trainPrefab;

    public static TrainManager Instance;
    private TrainTest spawnedTrain;

    [Header("Spawn Locations")]
    [SerializeField] private TrackNode[] startNodes;

    [Header("Spawned Trains")]
    private List<TrainTest> trains = new();

    private void Awake()
    {
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
            return;

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

        TrainTest train = trainObject.GetComponent<TrainTest>();

        train.SetStartingNode(startNode);

        spawnedTrain = train;

        trainObject.GetComponent<NetworkObject>().Spawn();
        
    }
    public TrainTest GetTrain()
    {
        return spawnedTrain;
    }
}