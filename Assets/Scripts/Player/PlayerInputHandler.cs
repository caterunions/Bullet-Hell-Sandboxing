using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    [SerializeField]
    private PlayerMove _playerMove;

    [SerializeField]
    private PlayerAim _playerAim;

    [SerializeField]
    private WeaponHolder _weaponHolder;

    [SerializeField]
    private AbilityHolder _abilityHolder;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private InventoryDisplay _inventoryDisplay;

    private void OnEnable()
    {
        if (_camera == null) _camera = Camera.main;

        _playerInput.actions.FindAction("Move").performed += HandleMove;
        _playerInput.actions.FindAction("Move").canceled += StopMove;

        _playerInput.actions.FindAction("Aim").performed += HandleAim;

        _playerInput.actions.FindAction("Fire").performed += BeginFire;
        _playerInput.actions.FindAction("Fire").canceled += StopFire;

        _playerInput.actions.FindAction("Ability").performed += BeginAbility;
        _playerInput.actions.FindAction("Ability").canceled += StopAbility;

        _playerInput.actions.FindAction("Inventory").performed += ToggleInventory;
    }

    private void OnDisable()
    {
        _playerInput.actions.FindAction("Move").performed -= HandleMove;
        _playerInput.actions.FindAction("Move").canceled -= StopMove;

        _playerInput.actions.FindAction("Aim").performed -= HandleAim;

        _playerInput.actions.FindAction("Fire").performed -= BeginFire;
        _playerInput.actions.FindAction("Fire").canceled -= StopFire;

        _playerInput.actions.FindAction("Ability").performed -= BeginAbility;
        _playerInput.actions.FindAction("Ability").canceled -= StopAbility;

        _playerInput.actions.FindAction("Inventory").performed -= ToggleInventory;
    }

    private void HandleMove(InputAction.CallbackContext ctx)
    {
        _playerMove.HandleMove(ctx.ReadValue<Vector2>());
    }

    private void StopMove(InputAction.CallbackContext ctx)
    {
        _playerMove.HandleMove(Vector2.zero);
    }

    private void HandleAim(InputAction.CallbackContext ctx)
    {
        _playerAim.HandleAim(_camera.ScreenToWorldPoint(ctx.ReadValue<Vector2>()));
    }

    private void BeginFire(InputAction.CallbackContext ctx)
    {
        _weaponHolder.StartFiring();
    }

    private void StopFire(InputAction.CallbackContext ctx)
    {
        _weaponHolder.StopFiring();
    }

    private void BeginAbility(InputAction.CallbackContext ctx)
    {
        _abilityHolder.StartAbility();
    }

    private void StopAbility(InputAction.CallbackContext ctx)
    {
        _abilityHolder.StopAbility();
    }

    private void ToggleInventory(InputAction.CallbackContext ctx)
    {
        _inventoryDisplay.ToggleInventory();
    }
}
