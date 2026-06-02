using System;
using UnityEngine;

public interface IPlayerInput
{
    Vector2 MoveInput { get; }
    Vector2 LookInput { get; }
    bool isJumping { get; }
    bool isRunning { get; }

    event Action OnShootEvent;
    event Action<float> OnSwitchWeaponEvent;
    event Action OnInteractEvent; 
}
