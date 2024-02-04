using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Debug Message", menuName = "Item/Effect Activations/Debug Message")]
public class DebugMessageActivation : ItemEffectActivation
{
    [SerializeField]
    private string _message;

    public override void Activate()
    {
        Debug.Log(_message);
        Debug.Log($"player {_player}\nactivator {_activator}\nbullet {_bullet}\ntarget {_target}\ndmgEvent {_dmgEvent}\nplayerInventory {_playerInventory}\nplayerStats {_playerStats}\nbulletLauncher {_bulletLauncher}");
    }
}
