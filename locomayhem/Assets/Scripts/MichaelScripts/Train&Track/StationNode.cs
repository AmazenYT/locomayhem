using UnityEngine;

public class StationNode : TrackNode
{
    [Header("Station Settings")]
    public string stationName;
    public float stopTime = 3f;

    [Header("Passengers")]
    public int waitingPassengers;

    public void TrainArrived()
    {
        Debug.Log($"Train arrived at {stationName}");
    }

    public void TrainDeparted()
    {
        Debug.Log($"Train departed from {stationName}");
    }
}