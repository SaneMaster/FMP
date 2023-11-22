using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoru : MonoBehaviour
{
    public Gun[] guns; // Array to hold the player's guns
    private int currentGunIndex = 0; // Index to keep track of the currently equipped gun

    public void AddGun(Gun gunToAdd)
    {
        // Add the gun to the player's inventory
        // Here you might check for available space or specific conditions for adding the gun
        // For example, you might check if the player already has the gun in their inventory
        // Then add the gun to the inventory or increase ammo count, etc.
    }

    // Method to switch between equipped guns
    public void SwitchGun(int newIndex)
    {
        if (newIndex >= 0 && newIndex < guns.Length)
        {
            currentGunIndex = newIndex;
            // Implement logic to switch guns, change visuals, etc.
        }
    }
}
