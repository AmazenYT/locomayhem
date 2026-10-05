using UnityEngine;

public class StationDirectionButtonReal : MonoBehaviour
{
    [SerializeField] private Station station;
    [SerializeField] private int direction;

    private void OnMouseDown()
    {
        station.SetDirection(direction);

        Debug.Log("Direction set to " + direction);
    }
}