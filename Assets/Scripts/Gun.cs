using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Gun : MonoBehaviour
{
    public float damage = 10f;  // max damage that can be dealt to whatever object the projectile is hitting
    public float range = 100f;   // max range at which the bullet can reach
    public float fireRate = 15f;   // rate at which projectile can spawn from the gun muzzel

    public int maxAmmo = 30;   // max total number of projectiles that can be shot without reloading
    private int currentAmmo;   // current ammo in the magazine that updates after a bullet is shot
    public float reloadTime = 1f;   // how long it takes the player to reload the magazine
    private bool isReloading = false;

    public TextMeshProUGUI ammoText;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    public Animator animator;
    public float spreadFactor;


    void Start()
    {

        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        ammoText.text = " " + currentAmmo + " / " + maxAmmo;
    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKey(KeyCode.R) && currentAmmo <= maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }


    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloaded");

        animator.SetBool("Reloading", true);



        yield return new WaitForSeconds(reloadTime - .30f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.30f);

        currentAmmo = maxAmmo;
        UpdateAmmoUI();
        isReloading = false;
    }


    void Shoot()
    {
        muzzleFlash.Play();

        currentAmmo--;

        // Apply spread to the shot direction
        Vector3 shotDirection = ApplySpread(fpsCam.transform.forward);


        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, shotDirection, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 0.5f);

            UpdateAmmoUI();
        }


    }

    IEnumerator ResetCameraPosition()
    {
        yield return new WaitForSeconds(0.1f); // Adjust this delay based on the desired recovery time

        // Reset camera position and rotation
        fpsCam.transform.localPosition = Vector3.zero;
        fpsCam.transform.localRotation = Quaternion.identity;
    }


    Vector3 ApplySpread(Vector3 originalDirection)
    {
        // Calculate a random spread direction based on the spread factor
        float spreadX = Random.Range(-spreadFactor, spreadFactor);
        float spreadY = Random.Range(-spreadFactor, spreadFactor);

        // Apply the spread to the original direction
        Vector3 spread = fpsCam.transform.right * spreadX + fpsCam.transform.up * spreadY;
        Vector3 newDirection = originalDirection + spread;

        // Return the new direction with spread applied
        return newDirection.normalized; // Normalize to maintain direction but adjust magnitude
    }
}
