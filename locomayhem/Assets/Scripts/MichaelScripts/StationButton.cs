using UnityEngine;
using Unity.Netcode;

public class StationButton : MonoBehaviour
{
    [SerializeField] private Station station;
    [SerializeField] private int direction;

    private void OnMouseDown()
    {
        station.SetDirection(direction);
        station.DispatchTrain();
    }
}
