using UnityEngine;


public class Station : MonoBehaviour
{
    [SerializeField] private Train startingTrain;
    private Train currentTrain;

    public int selectedExit = 0;

    public bool HasTrain()
    {
        return currentTrain != null;
    }

    private void Start()
    {
        if (startingTrain != null)
        {
            currentTrain = startingTrain;
            currentTrain.StopTrain();
        }
    }

    public void TrainArrived(Train train)
    {
        Debug.Log("TrainArrived called");
        currentTrain = train;

        currentTrain.StopTrain();

        Debug.Log("Train arrived");
    }

    public void SetDirection(int direction)
    {
        selectedExit = direction;
    }

    public void DispatchTrain()
    {
        Debug.Log("Dispatch pressed");

        if (currentTrain == null)
        {
            Debug.Log("No train to dispatch");
            return;
        }

        Debug.Log("Dispatching train");
        currentTrain.LeaveStation(selectedExit);

        currentTrain = null;
    }
}