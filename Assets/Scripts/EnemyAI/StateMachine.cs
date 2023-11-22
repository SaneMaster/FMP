using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState;
    

    public void Initialise()
    {        
        ChangeState(new PatrolState());  // what state the enemy is in, updates state if conditions are met
    }

    void Update()
    {
        if (activeState != null)
        {
            activeState.Perform();
        }
    }

    public void ChangeState(BaseState newState)
    {
        if (activeState != null)
        {
            activeState.Exit();
        }

        activeState = newState;

        if (activeState != null)
        {
            activeState.stateMachine = this;
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();

        }


    }
}
