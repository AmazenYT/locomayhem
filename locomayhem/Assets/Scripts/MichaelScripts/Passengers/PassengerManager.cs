using UnityEngine;

public class PassengerManager : MonoBehaviour
{
    public Station[] stations;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnPassenger), 2f, 5f);
    }

    void SpawnPassenger()
    {
        Station start = stations[Random.Range(0, stations.Length)];

        Station destination = stations[Random.Range(0, stations.Length)];

        while (destination == start)
        {
            destination = stations[Random.Range(0, stations.Length)];
        }

        start.AddPassenger(new Passenger(destination));
    }
}