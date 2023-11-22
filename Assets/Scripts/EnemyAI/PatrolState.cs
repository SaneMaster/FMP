using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int waypointIndex;  // keeps a note of which waypoint is being targeted
    public float idleTime;



    public override void Enter()
    {

    }

    public override void Perform()
    {
        PatrolCycle();
        if (enemy.SeenPlayer())
        {
            stateMachine.ChangeState(new AIAttack());
        }
    }



    public override void Exit()
    {

    }

    public void PatrolCycle()
    {
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            idleTime += Time.deltaTime;
            if (idleTime > 3)
            {
                if (waypointIndex < enemy.path.waypoints.Count - 1)
                    waypointIndex++;
                else
                    waypointIndex = 0;
                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
                idleTime = 0;
            }
        }
    }
}