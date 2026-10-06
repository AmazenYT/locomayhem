using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class Station : MonoBehaviour
{
    private Train currentTrain;

    [SerializeField] private SpriteRenderer leftLever;
    [SerializeField] private SpriteRenderer rightLever;

    [SerializeField] private Sprite leftLeverSelected;
    [SerializeField] private Sprite leftLeverUnselected;

    [SerializeField] private Sprite rightLeverSelected;
    [SerializeField] private Sprite rightLeverUnselected;

    public int selectedExit = 0;

    public List<Passenger> waitingPassengers = new List<Passenger>();
    public TMP_Text passengerText;

    public void AddPassenger(Passenger passenger)
    {
        waitingPassengers.Add(passenger);
        UpdatePassengerText();
    }
    private void UpdatePassengerText()
    {
        passengerText.text = waitingPassengers.Count.ToString();
    }

    public bool HasTrain()
    {
        return currentTrain != null;
    }

    public Train GetCurrentTrain()
    {
        return currentTrain;
    }



    public void TrainArrived(Train train)
    {
        Debug.Log("TrainArrived called");
        currentTrain = train;

        currentTrain.StopTrain();

        Debug.Log("Train arrived");

        foreach (Passenger passenger in waitingPassengers)
        {
            train.passengers.Add(passenger);
        }
        
        waitingPassengers.Clear();
        UpdatePassengerText();
        train.UpdatePassengerText();
    }



    //public void SetDirection(int direction)
    //{
        //selectedExit = direction;
    //}

    public void SetLeft()
    {
        selectedExit = 0;
        leftLever.sprite = leftLeverSelected;
        rightLever.sprite = rightLeverUnselected;

    }

    public void SetRight()
    {
        selectedExit = 1;
        leftLever.sprite = leftLeverUnselected;
        rightLever.sprite = rightLeverSelected;

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