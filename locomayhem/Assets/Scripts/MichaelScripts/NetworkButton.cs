using Unity.Netcode;
using UnityEngine;

public class NetworkButton : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private NetworkVariable<bool> isBlue = new NetworkVariable<bool>(false);

    private void Start()
    {
        UpdateColor();
        isBlue.OnValueChanged += OnStateChanged;
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
        isBlue.Value = !isBlue.Value;
    }

    private void OnMouseDown()
    {
        ToggleButtonRpc();
    }
}