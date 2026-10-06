using UnityEngine;

public class StationDirectionLeverRight : MonoBehaviour
{
    [SerializeField] private Station station;

    private void OnMouseDown()
    {
        station.SetRight();
    }
}