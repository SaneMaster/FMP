using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameObject player;   // reference to the player component
    private StateMachine stateMachine;  // reference to the state machine script
    private NavMeshAgent agent;   // reference to the nav mesh agent attached to the enemy capsule
    private Vector3 lastKnownPos;

    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }
    public Vector3 LastKnownPos { get => lastKnownPos; set => lastKnownPos = value; }

    [Header("Sight Value")]
    public float sightRange = 20f;   // distance at which the enemy can see the player
    public float fOV = 90f;   // enemy fov
    public float EyesHeight;
    [Header("Weapon Value")]
    public Transform gunBarrel;    // location from where the enemy projectile will shoot from
    [Range(0.1f, 10)]
    public float fireRate;   // rate at which bullets will spawn
    public Path path;       // defines the path in which the enemy can move around in the patrol state
    
    [SerializeField]
    private string currentState;

    private void Start()
    {
        stateMachine = GetComponent<StateMachine>();  // gets the state machine script
        agent = GetComponent<NavMeshAgent>();   // gets the nav mesh component attached to the enemy
        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");   // checks for the player tag
    }


    private void Update()
    {
        SeenPlayer();
        currentState = stateMachine.activeState.ToString();
    }


    public bool SeenPlayer()
    {
        Debug.Log("Seen player");
        if (player != null)   // check if the player is equal to null
        {
            if (Vector3.Distance(transform.position, player.transform.position) < sightRange) // checks if the player is close enough for enemy to see
            {
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * EyesHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if (angleToPlayer >= -fOV && angleToPlayer <= fOV)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * EyesHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    Debug.DrawRay(ray.origin, ray.direction * sightRange);
                    if (Physics.Raycast(ray, out hitInfo, sightRange))
                    {
                        if (hitInfo.transform.gameObject == player)
                        {
                            return true;
                        }
                    }
                   
                }
            }
        }
        return false;
    }
}