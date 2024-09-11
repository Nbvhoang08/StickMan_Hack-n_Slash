using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    public float timer;
    public float radTime;
    public void onEnter(Enemies enemies)
    {
        timer = 0;
        radTime = Random.Range(1,2);
       
    }

    public void onExcute(Enemies enemies)
    {
        timer += Time.deltaTime;
        if (enemies is Boss || enemies is Orc) // neu la boss haoc orc
        {
            if (enemies.Target != null)
            {
                enemies.ChangedDirection(enemies.Target.transform.position.x > enemies.transform.position.x);
                if (enemies.targetInRange())
                {
                    enemies.changeState(new AtkState());
                }
                else
                {
                    enemies.Moving();

                }

            }
            else
            {
                if (timer < radTime)
                {
                    enemies.Moving();

                }
                else
                {
                    enemies.changeState(new IdleState());
                }
            }
        }
        else if (enemies is Soldier) // linh
        {
            if (enemies.Target != null)
            {
                enemies.ChangedDirection(enemies.Target.transform.position.x > enemies.transform.position.x);
                enemies.changeState(new AtkState()); // co muc tieu , trong tam danh  -> can chien
            }
            else
            {
                if (timer < radTime)
                {
                    enemies.Moving();

                }
                else
                {
                    enemies.changeState(new IdleState());
                }
            }
        }
    }

    public void onExit(Enemies enemies)
    {
        
    }

    

}
