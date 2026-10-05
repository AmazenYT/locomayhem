using UnityEngine;

public class TrainButton : MonoBehaviour
{
    [SerializeField] private Train train;

    private void OnMouseDown()
    {
        //train.SwitchRoute();
        train.StartTrain();
    }
}