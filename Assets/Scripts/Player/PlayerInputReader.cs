using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour, IPlayerInput
{
    private FPSInputactions _gameInput;

    public event Action OnShootEvent;
    public event Action<float> OnSwitchWeaponEvent;
    public event Action OnInteractEvent;

    void Awake()
    {
        _gameInput = new FPSInputactions();

        _gameInput.Player.Shoot.performed += DoShoot;
        _gameInput.Player.Interact.performed += DoInteract;
        _gameInput.Player.SwitchWeapon.performed += DoSwitchWeapon;
    }

    void OnEnable() => _gameInput.Enable();
    void OnDisable() => _gameInput.Disable();

    private void DoShoot(InputAction.CallbackContext context)
    {
        OnShootEvent?.Invoke();
    }
    private void DoInteract(InputAction.CallbackContext context)
    {
        OnInteractEvent?.Invoke();
    }
    private void DoSwitchWeapon(InputAction.CallbackContext context)
    {
        float scrollvalue = context.ReadValue<float>();
        OnSwitchWeaponEvent?.Invoke(scrollvalue);
    }

    public Vector2 MoveInput => _gameInput.Player.Movement.ReadValue<Vector2>();

    public Vector2 LookInput => _gameInput.Player.Look.ReadValue<Vector2>();

    public bool isJumping =>  _gameInput.Player.Jump.WasPressedThisFrame();

    public bool isRunning => _gameInput.Player.Sprint.IsPressed();

    public Vector2 MoveValue;
    public Vector2 LookValue;
    public bool jumpValue;
    public bool sprintValue;
    void Update()
    {
        MoveValue = MoveInput;
        LookValue = LookInput;
        jumpValue = isJumping;
        sprintValue = isRunning;

    }
}
