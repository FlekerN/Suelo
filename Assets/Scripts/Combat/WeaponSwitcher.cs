using System;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Arsenal")]
    public Weapon[] weapons; // Asignar los GameObjects de las armas en el Inspector
    private int _currentWeaponIndex = 0;

    private IPlayerInput _input;

    private void Awake()
    {
        _input = GetComponent<IPlayerInput>();
    }

    private void OnEnable()
    {
        if (_input != null)
        {
            _input.OnShootEvent += HandleShoot;
            _input.OnSwitchWeaponEvent += HandleSwitchWeapon;
        }
    }

    private void HandleShoot()
    {
        if (weapons.Length > 0)
        {
            weapons[_currentWeaponIndex].TryAttack();
        }
    }

    private void HandleSwitchWeapon(float scrollValue)
    {
        if (scrollValue > 0)
        {
            _currentWeaponIndex = (_currentWeaponIndex + 1) % weapons.Length;
            EquipWeapon(_currentWeaponIndex);
        }
        else if (scrollValue < 0) 
        {
            _currentWeaponIndex--;
            if(_currentWeaponIndex < 0 ) _currentWeaponIndex = weapons.Length - 1;
            EquipWeapon( _currentWeaponIndex);
        }


    }

    void Start()
    {
        EquipWeapon(_currentWeaponIndex);
    }

    private void EquipWeapon(int index)
    {
        // Apagamos todas las armas y encendemos solo la seleccionada
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == index);
        }

        Debug.Log($"Arma equipada: {weapons[index].weaponName}");
    }

    private void OnDisable()
    {
        if (_input != null)
        {
            _input.OnShootEvent -= HandleShoot;
            _input.OnSwitchWeaponEvent -= HandleSwitchWeapon;
        }
    }
}
