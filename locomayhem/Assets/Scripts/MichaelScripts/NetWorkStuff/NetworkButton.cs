using Unity.Netcode;
using UnityEngine;

public class NetworkButton : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TrainTest train;

    private NetworkVariable<bool> isBlue = new(false);

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void OnNetworkSpawn()
    {
        isBlue.OnValueChanged += OnStateChanged;
        UpdateColor();
    }

    public override void OnDestroy()
    {
        isBlue.OnValueChanged -= OnStateChanged;
        base.OnDestroy();
    }

    private void OnStateChanged(bool oldValue, bool newValue)
    {
        UpdateColor();
    }

    private void UpdateColor()
    {
        spriteRenderer.color = isBlue.Value ? Color.blue : Color.red;
    }
    
    [Rpc(SendTo.Server)]
    public void ToggleButtonRpc()
    {
        if (isBlue.Value)
            return;

        isBlue.Value = true;

        TrainTest train = TrainManager.Instance.GetTrain();

        if (train != null)
        {
            train.SetMoving(true);
        }
    }


    private void OnMouseDown()
    {
        if (NetworkManager.Singleton.IsClient)
        {
            ToggleButtonRpc();
        }
    }
}