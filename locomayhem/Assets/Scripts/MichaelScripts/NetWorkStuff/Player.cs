using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public NetworkVariable<int> TeamId = new();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
        }

        if (IsOwner)
        {
            Debug.Log("This is my player");
        }
    }
}