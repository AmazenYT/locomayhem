using Unity.Netcode;
using UnityEngine;

public class TrainManager : NetworkBehaviour
{
    [SerializeField] private GameObject trainPrefab;

    [Header("Spawn Locations")]
    [SerializeField] private TrackNode[] startNodes;

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

        trainObject.GetComponent<NetworkObject>().Spawn();
    }
}