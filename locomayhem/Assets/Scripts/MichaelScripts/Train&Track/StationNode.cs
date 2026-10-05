using Unity.Netcode;
using UnityEngine;

public class StationNode : NetworkBehaviour
{
    [Header("Station Information")]
    [SerializeField] private string stationName = "Station";

    private TrainTest waitingTrain;

    private NetworkVariable<bool> hasTrain = new(false);

    public override void OnNetworkSpawn()
    {
        hasTrain.OnValueChanged += OnHasTrainChanged;
        UpdateStationVisual();
    }

    public override void OnNetworkDespawn()
    {
        hasTrain.OnValueChanged -= OnHasTrainChanged;
        base.OnNetworkDespawn();
    }

    private void OnHasTrainChanged(
        bool oldValue,
        bool newValue)
    {
        UpdateStationVisual();
    }

    private void UpdateStationVisual()
    {
        Debug.Log(
            stationName +
            " has train: " +
            hasTrain.Value);
    }

    public void TrainArrived(TrainTest train)
    {
        if (!IsServer)
            return;

        if (train == null)
            return;

        if (waitingTrain != null)
        {
            Debug.LogWarning(
                stationName +
                " already has a train waiting.");
            return;
        }

        waitingTrain = train;
        hasTrain.Value = true;

        Debug.Log(
            train.gameObject.name +
            " arrived at " +
            stationName);
    }

    public void TrainDeparted(TrainTest train)
    {
        if (!IsServer)
            return;

        if (waitingTrain != train)
            return;

        Debug.Log(
            train.gameObject.name +
            " departed from " +
            stationName);

        waitingTrain = null;
        hasTrain.Value = false;
    }

    /// <summary>
    /// Called on the server by a direction button.
    /// </summary>
    public void SelectDirection(TrackNode selectedNode)
    {
        if (!IsServer)
            return;

        if (waitingTrain == null)
        {
            Debug.Log(
                "There is no train waiting at " +
                stationName);

            return;
        }

        if (selectedNode == null)
        {
            Debug.LogWarning(
                "The selected direction node is missing.");

            return;
        }

        waitingTrain.DepartStation(selectedNode);
    }

    public bool HasWaitingTrain()
    {
        return waitingTrain != null;
    }

    public bool HasTrainNetworked()
    {
        return hasTrain.Value;
    }
}