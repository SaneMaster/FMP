using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack : BaseState
{
    private float AIFloatAround;   // so the enemy can float around a bit and not just be idle whilst shooting
    private float lostPlayer;   // how long enemy remains in attack state before searching for player
    private float shotTime;

    public float bulletSpeed = 40f;


    public override void Enter()
    {
        Debug.Log("Entering AIAttack state");
    }


    public override void Exit()
    {

    }


    public override void Perform()
    {
        Debug.Log("Performing AIAttack");
        if (enemy.SeenPlayer())   // if the player is seen by AI perform the following
        {
            lostPlayer = 0;
            AIFloatAround += Time.deltaTime;  // increment move timer
            shotTime += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);

            if (shotTime > enemy.fireRate)
            {
                Shoot();
            }
            if (AIFloatAround > Random.Range(3, 6)) // if move timer is greater than the random range
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 4));  // move enemy around the location within 4 units
                AIFloatAround = 0;  // reset move timer
            }
            enemy.LastKnownPos = enemy.Player.transform.position;   // if enemy loses sight of player it stores last known position in the LastKnownPos variable
        }
        else
        {
            lostPlayer += Time.deltaTime;
            if (lostPlayer > 6)
            {
                // change to search state
                stateMachine.ChangeState(new AISearch());  // return to AI patrol state if ai cannot find the player after 5 seconds
            }
        }

    }


    public void Shoot()
    {
        Transform gunbarrel = enemy.gunBarrel;  // reference stored to gun barrel
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/EnemyBullet") as GameObject, gunbarrel.position, enemy.transform.rotation);   // instantiate new bullet
        Vector3 shootDirection = (enemy.Player.transform.position - gunbarrel.transform.position).normalized;  // calculates direction to the player

       // bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-3f,3f),Vector3.up) * shootDirection * bulletSpeed;  // using random.range to add variation into enemies shooting accuracy
        // Disable gravity for the bullet's Rigidbody component
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = shootDirection * bulletSpeed;
        bulletRigidbody.useGravity = false;
        Debug.Log("Shooting");
        shotTime = 0;
    }
}