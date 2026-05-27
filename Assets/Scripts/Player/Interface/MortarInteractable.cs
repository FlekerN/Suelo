using UnityEngine;
public class MortarInteractable : LinearProjectileWeapon, IInteractable 
{
    public void Interact()
    {
        Debug.Log("Mortero interactuado");
        TryAttack();
    }
}
