using UnityEngine;

public class DispatchButton : MonoBehaviour
{
    [SerializeField] private Station station;

    private void OnMouseDown()
    {
        station.DispatchTrain();
    }
}