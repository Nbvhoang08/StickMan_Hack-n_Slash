using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AtkState : IState
{

    private float timer;
    public void onEnter(Enemies enemies)
    {
        if (enemies.Target != null)
        {
            // doi huong theo player
            enemies.ChangedDirection(enemies.Target.transform.position.x > enemies.transform.position.x);
            enemies.StopMoving();
            enemies.Atk();
           
        }
        timer = 0;
    }

    public void onExcute(Enemies enemies)
    {
        timer += Time.deltaTime;
       
        if (enemies.Target != null)
        {
            if (timer >= 1)
            {
                enemies.changeState(new CastState());
            }


        }
        else
        {
            if (timer >= 3f)
            {
                enemies.changeState(new PatrolState());

            }
            else
            {
                enemies.changeState(new IdleState());
            }
        }

        
    }

    public void onExit(Enemies enemies)
    {

    }

}
