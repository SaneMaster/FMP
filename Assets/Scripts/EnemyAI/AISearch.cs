using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISearch : BaseState
{
    private float searchTimer;
    private float AIFloatAround;



    public override void Enter()
    {
        enemy.Agent.SetDestination(enemy.LastKnownPos);


    }

    public override void Perform()
    {
        if (enemy.SeenPlayer())       
            stateMachine.ChangeState(new AIAttack());
            
        
        if (enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance)
        {
            searchTimer += Time.deltaTime;
            AIFloatAround += Time.deltaTime;
            if (AIFloatAround > Random.Range(3, 5)) // if move timer is greater than the random range
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 9));  // move enemy around the location within 4 units
                AIFloatAround = 0;  // reset move timer
            }
            if (searchTimer > 8)
            {
                stateMachine.ChangeState(new PatrolState());               
            }
        }
    }

    public override void Exit()
    {

    }

}

