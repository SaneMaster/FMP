using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    
    private float health;
    private float lerpTimer;
    private float durationTimer;   // timer to check duration

    [Header("Health Properties")]
    public float playerHealth = 100;   // player health
    public float maxPlayerHealth = 100;  // max health the player can have at any time
    public float regenAmount = 10;  // amount of health that is regenerated
    public float regenInterval = 5;   // how long it takes to increase the regened healths
    public float chipSpeed = 2f;   // control how quickly delay bar takes too catch up to bar
    public Image frontHealth;
    public Image backHealth;

    [Header("Damage Properties")]
    public Image overlay;  // damage overlay image
    public float duration;  // amount of time damage overlay image stays opaque
    public float damageSpeed;  // how long it takes damage image to fade

  

    void Start()
    {
        health = playerHealth;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);   // set to 0 so image is clear when the player starts the game
        StartCoroutine(RegenHealthOverTime());
    }

    
    void Update()
    {
        health = Mathf.Clamp(health, 0, playerHealth);
        UpdateHealthUI();

        if (health <= 0)
        {
            Die();
        }

        if(overlay.color.a > 0)  // checking if the alpha is greater than 0
        {
            if (health < 30)
                return;  // pauses duration timer so that it wont stop and start the fade process
            durationTimer += Time.deltaTime;
            if(durationTimer > duration)
            {
                // fades the image
                float tempAlpha = overlay.color.a;  // for every tick of update temp value gets set to alpha of image
                tempAlpha -= Time.deltaTime * damageSpeed;   // decrement temp value
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);   // asigned back into alpha channel to create fading effect
            }
        }
      
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealth.fillAmount;
        float fillB = backHealth.fillAmount;
        float hFraction = health / playerHealth;
        if(fillB > hFraction)
        {
            frontHealth.fillAmount = hFraction;
            backHealth.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealth.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if(fillF < hFraction)
        {
            backHealth.color = Color.green;
            backHealth.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontHealth.fillAmount = Mathf.Lerp(fillF, backHealth.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)   // take damage function
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0;   // set to 0 so each time player takes damage the timer resets
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
    }

    private IEnumerator RegenHealthOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenInterval);
            if (health < maxPlayerHealth)
            {
                health += regenAmount;
                health = Mathf.Clamp(health, 0, maxPlayerHealth);
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player dead");
    }
}
