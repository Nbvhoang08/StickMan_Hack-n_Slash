using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastState : IState
{
    float timer; 
    public void onEnter(Enemies enemies)
    {
        enemies.Cast();
        timer = 0;
       
    }

    public void onExcute(Enemies enemies)
    {
        timer += Time.deltaTime;
        if(timer > 3f)
        {
            if(enemies.Target != null)
            {
                enemies.changeState(new AtkState());
            }
            else
            {
                enemies.changeState(new PatrolState());
            }
        }
    }

    public void onExit(Enemies enemies)
    {

    }
}
