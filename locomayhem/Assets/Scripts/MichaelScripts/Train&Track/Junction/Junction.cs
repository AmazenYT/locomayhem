using UnityEngine;

public class Junction : MonoBehaviour
{
    public int selectedRoute = 0;

    private TrackNode junctionNode;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;


    private void Awake()
    {
        junctionNode = GetComponent<TrackNode>();
    }

    public void Start()
    {
        UpdateArrows();
    }

    private void UpdateArrows()
    {
        leftArrow.SetActive(selectedRoute == 0);
        rightArrow.SetActive(selectedRoute == 1);
    }

    public void SwitchRoute()
    {
        selectedRoute++;

        if (selectedRoute > 1)
            selectedRoute = 0;

        spriteRenderer.sprite = sprites[selectedRoute];
        UpdateArrows();
        Debug.Log("Route: " + selectedRoute);
    }

    public TrackNode GetExit(TrackNode previousNode)
    {
        if (junctionNode.connections.Count < 3)
            return null;

        // Coming from A
        if (previousNode == junctionNode.connections[0])
        {
            return selectedRoute == 0
                ? junctionNode.connections[1] // B
                : junctionNode.connections[2]; // C
        }

        // Coming from B or C
        return junctionNode.connections[0];
    }
}