using UnityEngine;

public class JunctionButton : MonoBehaviour
{
    [SerializeField] private Junction junction;

    private void OnMouseDown()
    {
        junction.SwitchRoute();
    }
}