using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGun : MonoBehaviour
{
    public Gun gunToPickup; // Reference to the Gun script 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player collides with the pickup
        {
            // Access the Gun script of the player and provide the pickup gun
            Gun playerGun = other.GetComponentInChildren<Gun>(); 
            if (playerGun != null)
            {
                playerGun.spreadFactor = gunToPickup.spreadFactor; // Assign the spread factor to the player's gun

                Destroy(gameObject); // Destroy the pickup object after it's collected
            }
        }
    }
}
