using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class WeaponPickup : MonoBehaviour , IInteractable
{

    public Weapon WeaponToBePickedUp;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void DisplayInteraction()
    {
        throw new System.NotImplementedException();
    }

    public void HideInteraction()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        if(!WeaponToBePickedUp) { return; }

        Player player = LevelManager.Instance.GetPlayer();
        WeaponHandler weaponHandler = player.GetComponentInChildren<WeaponHandler>();

        if (!weaponHandler) { return; }

        weaponHandler.AddWeapon(WeaponToBePickedUp);

        Destroy(this.gameObject);
    }

}
