using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class IdleState : IState
{
    public float timer;
    public float radTime;
    public void onEnter(Enemies enemies)
    {
        enemies.StopMoving();
        timer = 0 ;
        radTime = Random.Range(1, 2);
    }

    public void onExcute(Enemies enemies)
    {
        timer += Time.deltaTime;
        
        if (timer > radTime)
        {
            enemies.changeState(new PatrolState());
        }
    }

    public void onExit(Enemies enemies)
    {

    }
}
