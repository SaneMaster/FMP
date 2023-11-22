using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public int currentWeapon = 0;

    public GameObject[] weapons; // Array of weapon game objects
    public GameObject[] crosshairs;

    private bool[] weaponsPickedUp; // Tracks whether each weapon has been picked up
    private bool[] crosshairsEnabled; // Tracks whether each crosshair is enabled

    void Start()
    {
        InitializeWeapons();
        CurrentWeapon();
    }

    void InitializeWeapons()
    {
        // Initialize the array to track picked up status for each weapon
        weaponsPickedUp = new bool[weapons.Length];
        crosshairsEnabled = new bool[crosshairs.Length];

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
            weaponsPickedUp[i] = false; // weapons not initially picked up

            crosshairs[i].SetActive(false);   // crosshair not initially enabled
            crosshairsEnabled[i] = false;
        }
    }


    void Update()
    {
        int previousSelectedWeapon = currentWeapon;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (currentWeapon >= transform.childCount - 1)
                currentWeapon = 0;
            else
                currentWeapon++;
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (currentWeapon <= 0)
                currentWeapon = transform.childCount -1;
            else
                currentWeapon--;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1) && weaponsPickedUp[0])
        {
            currentWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && weaponsPickedUp[1])
        {
            currentWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && weaponsPickedUp[2])
        {
            currentWeapon = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && weaponsPickedUp[3])
        {
            currentWeapon = 3;
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            // Attempt to pick up a weapon when 'E' is pressed
            TryPickupWeapon();
        }


        if (previousSelectedWeapon != currentWeapon)
        {

            SwitchWeapon(currentWeapon);
        }
    }

    void CurrentWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == currentWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }

    void SwitchWeapon(int weaponIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            // Enable only the picked up weapon and disable hands if another weapon is picked up
            weapons[i].SetActive(i == weaponIndex && weaponsPickedUp[i]);

            // Enable only the picked up weapon's crosshair
            crosshairs[i].SetActive(i == weaponIndex && crosshairsEnabled[i] && weaponsPickedUp[i]);
        }

        // Ensure the hands are enabled if the picked weapon is not yet picked up
        if (!weaponsPickedUp[weaponIndex])
        {
            weapons[0].SetActive(true); // Enable hands
            crosshairs[0].SetActive(true); // Enable hands' crosshair
        }
    }


    void SwitchCrosshair(int weaponIndex)
    {
        for (int i = 0; i < crosshairs.Length; i++)
        {
            crosshairs[i].SetActive(i == weaponIndex && crosshairsEnabled[i] && weaponsPickedUp[i]); // Enable only the picked up weapon's crosshair

        }
    }


    void TryPickupWeapon()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            // Check if the object hit by the raycast is a pickup object
            WeaponPickup pickup = hit.collider.GetComponent<WeaponPickup>();

            if (pickup != null)
            {
                int pickedWeaponIndex = pickup.weaponIndex;

                // Check if the picked weapon index is within the available range of weapons
                if (pickedWeaponIndex >= 0 && pickedWeaponIndex < weapons.Length && !weaponsPickedUp[pickedWeaponIndex])
                {
                    currentWeapon = pickedWeaponIndex;
                    weaponsPickedUp[pickedWeaponIndex] = true; // Mark the weapon as picked up
                    crosshairsEnabled[pickedWeaponIndex] = true;
                    SwitchWeapon(pickedWeaponIndex);
                    Destroy(pickup.gameObject); // Destroy the pickup object after collecting the weapon
                }
            }
        }
    }

}
