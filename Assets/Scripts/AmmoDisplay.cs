using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    public int ammo;
    public bool isFiring;
    public TextMeshProUGUI ammoDisplay;

    void Start()
    {
        
    }

    
    void Update()
    {
        ammoDisplay.text = ammo.ToString();
        if(Input.GetMouseButtonDown(0) && !isFiring && ammo > 0)
        {
            isFiring = true;
            ammo--;
            isFiring = false;
        }
    }
}
